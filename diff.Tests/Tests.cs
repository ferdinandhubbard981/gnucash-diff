namespace diff.Tests;
using GNCDiff;
public class Tests
{
    [Fact]
    public void TestBookLoaded()
    {
        Book book = Book.FromGNCFile("../../../test_data/single_account.gnucash");
        List<Account> accounts = book.GetAccounts();
        Assert.True(accounts.Count == 2, "This book should have exactly two accounts"); // including root
        Account? Expenses = book.GetAccountFirstOccurence("Expenses");
        Assert.False(Expenses == null, "This book should have an 'Expenses' account");
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
        Assert.True(accounts.Count == 3, "This book should have exactly 3 accounts");
        List<(double, string)> expectedSplits = new List<(double, string)>();
        expectedSplits.Add((10025.0, "Expenses"));
        expectedSplits.Add((-10025.0, "Checking"));
        expectedSplits.Add((500, "Checking"));
        expectedSplits.Add((-500, "Expenses"));
        Assert.True(book.splits.Count == 4, "Expected there to be 4 transactions");
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
            Assert.True(splitFound, "Should have contained a particular split");
        }

    }

    [Fact]
    public void TestAccountDeleted()
    {
        Book before = Book.FromGNCFile("../../../test_data/diff/account_deleted/before.gnucash");
        Book after = Book.FromGNCFile("../../../test_data/diff/account_deleted/after.gnucash");
        Account? removedAccount = before.GetAccountFirstOccurence("Checking");
        Assert.False(removedAccount == null, "Expected to find Checking account");
        IBookMod accountRemovalStep = new RemoveAccountMod(removedAccount);
        Diff diff = Diff.FromBooks(before, after);
        Assert.True(diff.steps.Count == 1, "Expected there to only be 1 step");
        Assert.True(diff.steps[0].ToString() == accountRemovalStep.ToString(), "Account removal step not as expected");
    }

    // TODO add test with duplicate accounts (to test if guid is working properly and is being used properly)
}