using NC = NetCash;
class Book
{
    Account root;
    List<Split> splits;
    public Book(Account root, List<Split> splits)
    {
        this.root = root;
        this.splits = splits;
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

    public Account? get_account(String full_name)
    {
        return Book.get_account(full_name, this.root);
    }
    private static Account? get_account(String full_name, Account searchRootAccount)
    {
        if (searchRootAccount.fullName == full_name)
        {
            return searchRootAccount;
        }
        foreach (Account child in searchRootAccount.children)
        {
            Account? result = Book.get_account(full_name, child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}