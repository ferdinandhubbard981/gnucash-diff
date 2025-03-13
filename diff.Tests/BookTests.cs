namespace diff.Tests;
using GNCDiff;
public class BookTests
{
    [Fact]
    public void TestBookLoaded()
    {
        string inputFile = "../../../test_data/single_account.gnucash";
        inputFile = Path.GetFullPath(inputFile); // get absolute path, otherwise gnucash will look for it somewhere else
        Book book = Book.FromGNCFile(inputFile);
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
        string inputFile = "../../../test_data/two_accounts_with_transactions.gnucash";
        inputFile = Path.GetFullPath(inputFile);
        Book book = Book.FromGNCFile(inputFile);
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

    // TODO add test with duplicate accounts (to test if guid is working properly and is being used properly)
}
