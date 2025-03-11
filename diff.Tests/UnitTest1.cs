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
        Account? Expenses = book.GetAccount("Expenses");
        Assert.False(Expenses == null, "This book should have an 'Expenses' account");
    }

    [Fact]
    public void TestBookLoadedTwice()
    {
        // to make sure there are no issues with opening and closing books repeatedly.
        TestBookLoaded();
        TestBookLoaded();
    }
}
