using NC = NetCash;
namespace GNCDiff;
public class Book
{
    public Account root {get;}
    public List<Split> splits {get;}
    public Book(Account root, List<Split> splits)
    {
        this.root = root;
        this.splits = splits;
    }

    public static Book FromGNCFile(string file)
    {
        file = Path.GetFullPath(file); // get absolute path, otherwise gnucash will look for it somewhere else
        GNCInitialiseTracker.InitialiseGNCEngine();
        NC.Book ncBook = NC.Book.OpenRead(file);
        Book output = Book.FromNCBook(ncBook);
        ncBook.Close();
        return output;
    }
    public static Book FromNCBook(NC.Book ncBook)
    {
        Account root = Account.FromNCAccount(ncBook.RootAccount);
        // query all splits
        NC.SplitQuery splitQuery = new NC.SplitQuery(ncBook);
        IReadOnlyCollection<NC.Split> ncSplits = splitQuery.Run(); // tocheck: I don't know if not setting any filters will query for all splits
        List<Split> splits = new List<Split>();
        Book newBook = new Book(root, splits);
        foreach (NC.Split ncSplit in ncSplits)
        {
            newBook.splits.Add(Split.FromNCSplit(ncSplit, newBook));
        }
        return newBook;
    }

    public List<Account> GetAccounts()
    {
        return GetAccounts(this.root);
    }

    List<Account> GetAccounts(Account searchRoot)
    {
        List<Account> accounts = new List<Account>();
        accounts.Add(searchRoot);
        foreach (Account child in searchRoot.children)
        {
            accounts.AddRange(GetAccounts(child));
        }
        return accounts;
    }


    public Account? GetAccount(Guid guid)
    {
        return Book.GetAccount(guid, this.root);
    }

    private static Account? GetAccount(Guid guid, Account searchRootAccount)
    {
        if (searchRootAccount.guid == guid)
        {
            return searchRootAccount;
        }
        foreach (Account child in searchRootAccount.children)
        {
            Account? result = Book.GetAccount(guid, child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public Account? GetAccountFirstOccurence(String full_name)
    {
        return Book.GetAccountFirstOccurence(full_name, this.root);
    }
    private static Account? GetAccountFirstOccurence(String full_name, Account searchRootAccount)
    {
        if (searchRootAccount.fullName == full_name)
        {
            return searchRootAccount;
        }
        foreach (Account child in searchRootAccount.children)
        {
            Account? result = Book.GetAccountFirstOccurence(full_name, child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public Split? GetSplit(Guid guid)
    {
        foreach (Split split in this.splits)
        {
            if (split.guid == guid) return split;
        }
        return null;
    }

}