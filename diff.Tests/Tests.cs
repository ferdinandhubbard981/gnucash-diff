namespace diff.Tests;
using GNCDiff;
public class Tests
{
    [Fact]
    public void TestBookLoaded()
    {
        Book book = Book.FromGNCFile("../../../test_data/single_account.gnucash");
        List<Account> accounts = book.GetAccounts();
        Assert.True(accounts.Count == 2, "This book should have exactly two accounts."); // including root
        Account? Expenses = book.GetAccountFirstOccurence("Expenses");
        Assert.False(Expenses == null, "This book should have an 'Expenses' account.");
    }

    [Fact]
    public void TestBookLoadedTwice()
    {
        // to make sure there are no issues with opening and closing books repeatedly.
        TestBookLoaded();
        TestBookLoaded();
    }

    [Fact]
    public void TestBookLoadedWithTransactions()
    {
        Book book = Book.FromGNCFile("../../../test_data/two_accounts_with_transactions.gnucash");
        List<Account> accounts = book.GetAccounts();
        Assert.True(accounts.Count == 3, "This book should have exactly 3 accounts.");
        List<(double, string)> expectedSplits = new List<(double, string)>();
        expectedSplits.Add((10025.0, "Expenses"));
        expectedSplits.Add((-10025.0, "Checking"));
        expectedSplits.Add((500, "Checking"));
        expectedSplits.Add((-500, "Expenses"));
        Assert.True(book.splits.Count == 4, "Expected there to be 4 transactions.");
        foreach (var expectedSplit in expectedSplits)
        {
            bool splitFound = false;
            foreach (Split split in book.splits)
            {
                if (split.amount == expectedSplit.Item1 && split.account.fullName == expectedSplit.Item2)
                {
                    splitFound = true;
                    break;
                }
            }
            Assert.True(splitFound, "Should have contained a particular split.");
        }

    }
    // TODO add test with duplicate accounts (to test if guid is working properly and is being used properly)

    [Fact]
    public void TestDiffAccountDeleted()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/account/deleted/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/account/deleted/after.gnucash");
        Account? removedAccount = before.GetAccountFirstOccurence("Checking");
        Assert.False(removedAccount == null, "Expected to find Checking account in before.gnucash.");
        IBookMod accountRemovalStep = new RemoveAccountMod(removedAccount);
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 1, "Expected there to only be 1 step.");
        Assert.True(diff.steps[0].ToDiffString() == accountRemovalStep.ToDiffString(), "Account removal step not as expected.");
    }

    [Fact]
    public void TestDiffAccountCreated()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/account/created/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/account/created/after.gnucash");
        Account? addedAccount = after.GetAccountFirstOccurence("Checking");
        Assert.False(addedAccount == null, "Expected to find Checking account in after.gnucash.");
        IBookMod accountCreationStep = new AddAccountMod(addedAccount);
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 1, "Expected there to only be 1 step.");
        Assert.True(diff.steps[0].ToDiffString() == accountCreationStep.ToDiffString(), "Account creation step not as expected.");
    }

    [Fact]
    public void TestDiffAccountMoved()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/account/moved/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/account/moved/after.gnucash");
        Diff diff = Diff.FromBooks(before, after);
        Account? accountBefore = before.GetAccountFirstOccurence("Expenses.Tax.Income tax");
        Assert.True(accountBefore != null, "Expected to find 'Expenses.Tax.Income tax' in before.gnucash");
        Account? accountAfter = after.GetAccountFirstOccurence("Expenses.Income tax");
        Assert.True(accountAfter != null, "Expected to find 'Expenses.Income tax' in after.gnucash");
        MoveAccountMod accountMovedStep = new MoveAccountMod(accountBefore, accountAfter);
        Assert.True(diff.steps.Count == 1, $"Expected there to be exactly one step, found {diff.steps.Count}.");
        Assert.True(diff.steps[0].ToString() == accountMovedStep.ToString(), $"Expected:\n{accountMovedStep}\nInstead we have:\n{diff.steps[0]}\n");
    }

    [Fact]
    public void TestPlaceholderModified()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/account/placeholder_mod/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/account/placeholder_mod/after.gnucash");
        Diff diff = Diff.FromBooks(before, after);
        Account? accountBefore = before.GetAccountFirstOccurence("Expenses.Tax");
        Assert.True(accountBefore != null, "Expected to find 'Expenses.Tax' in before.gnucash");
        Account? accountAfter = after.GetAccountFirstOccurence("Expenses.Tax");
        Assert.True(accountAfter != null, "Expected to find 'Expenses.Tax' in after.gnucash");
        IsPlaceholderAccountMod placeholderMod = new IsPlaceholderAccountMod(accountBefore, accountAfter);
        Assert.True(diff.steps.Count == 1, $"Expected there to be exactly one step, found {diff.steps.Count}.");
        Assert.True(diff.steps[0].ToString() == placeholderMod.ToString(), $"Expected:\n{placeholderMod}\nInstead we have:\n{diff.steps[0]}\n");
    }

    [Fact]
    public void TestDiffSplitDeleted()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/split/deleted/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/split/deleted/after.gnucash");
        List<Guid> expectedRemovedSplitGuids =
        [
            new Guid("ca794802-425a-4ba5-96fe-bd00d787c5d5"),
            new Guid("4ae7f4e4-907b-490c-824b-68aa5ebbb634"),
        ];
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 2, $"Expected there to be 2 steps. Found {diff.steps.Count}.");
        foreach (IBookMod step in diff.steps)
        {
            Assert.True(step.GetType() == typeof(RemoveSplitMod), $"Expected {typeof(RemoveSplitMod)}, got {step.GetType()}");
            Guid splitGuid = ((RemoveSplitMod) step).split.guid;
            Assert.True(expectedRemovedSplitGuids.Contains(splitGuid), $"Expected split: {splitGuid} to be removed");
        }
    }

    [Fact]
    public void TestDiffSplitCreated()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/split/created/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/split/created/after.gnucash");
        List<Guid> expectedAddedSplitGuids =
        [
            new Guid("7ada3532-cc98-400d-a107-e6f393e895d7"),
            new Guid("e1482c20-a8e8-41d0-b142-72111c4dc04e"),
        ];
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 2, $"Expected there to be 2 steps. Found {diff.steps.Count}.");
        foreach (IBookMod step in diff.steps)
        {
            Assert.True(step.GetType() == typeof(AddSplitMod), $"Expected {typeof(AddSplitMod)}, got {step.GetType()}");
            Guid splitGuid = ((AddSplitMod) step).split.guid;
            Assert.True(expectedAddedSplitGuids.Contains(splitGuid), $"Expected split: {splitGuid} to be added");
        }
    }
    // add a split that remains constant in these 2 tests' data files

    [Fact]
    public void TestDiffSplitAmountModified()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/split/amount_modified/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/split/amount_modified/after.gnucash");
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 2, $"Expected there to be only 2 steps. Found {diff.steps.Count}.");
        Dictionary<Guid, (double, double)> expectedData = [];
        expectedData.Add(new Guid("93179af8-aa21-4d39-b5ff-eb9a7f556088"), (9250, 3000));
        expectedData.Add(new Guid("c343ae29-fbfd-499c-b410-3a304d799dc5"), (-9250, -3000));
        foreach (IBookMod step in diff.steps)
        {
            ChangeAmountSplitMod amountChangedStep = (ChangeAmountSplitMod) step;
            var expected = expectedData[amountChangedStep.before.guid];
            Assert.True(amountChangedStep.before.amount == expected.Item1, $"Expected previous amount to be {expected.Item1} ,found {amountChangedStep.before.amount}");
            Assert.True(amountChangedStep.after.amount == expected.Item2, $"Expected new amount to be {expected.Item2} ,found {amountChangedStep.after.amount}");
        }
        double previousAmount = 9250;
        double actualPreviousAmount = (double)((ChangeAmountSplitMod) diff.steps[0]).before.amount;
        Assert.True(actualPreviousAmount == previousAmount, $"expected previous amount to be {previousAmount}, found {actualPreviousAmount}");
        double newAmount = 3000;
        double actualNewAmount = (double)((ChangeAmountSplitMod) diff.steps[0]).after.amount;
        Assert.True(actualNewAmount == newAmount, $"expected previous amount to be {newAmount}, found {actualNewAmount}");
    }
}