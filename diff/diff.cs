using NetCash;

class Diff
{
    // contains a list of steps to transform book A into book B
    private List<BookMod> steps;

    public Diff(List<BookMod> steps)
    {
        this.steps = steps;
    }

    public static Diff FromBooks(Book oldBook, Book newBook)
    {
        List<BookMod> steps = new List<BookMod>();
        steps.AddRange(GenerateAccountSteps(oldBook, newBook));
        // steps.AddRange(GenerateTransactionSteps(oldBook, newBook));
        return new Diff(steps);
    }

    static List<BookMod> GenerateAccountSteps(Book oldBook, Book newBook)
    {
        List<Account> oldAccounts = (List<Account>) oldBook.Accounts;
        List<Account> newAccounts = (List<Account>) newBook.Accounts;
        // let intersection be oldBook intersects newBook
        List<Account> intersection = (List<Account>) oldAccounts.Intersect(newAccounts);
        // let accountsRemoved be oldBook \ intersection
        List<Account> accountsRemoved = (List<Account>) oldAccounts.Except(intersection);
        // let accountsAdded be newBook \ intersection
        List<Account> accountsAdded = (List<Account>) newAccounts.Except(intersection);
        Comparison<Account> comparisonFunction = delegate (Account account1, Account account2)
        {
            Func<Account, int> getAccountDepth = delegate (Account account)
            {
                Account parent = account.Parent;
                int depth = 0;
                while (parent != null)
                {
                    depth++;
                    parent = parent.Parent;
                }
                return depth;
            };

            int account1Depth = getAccountDepth(account1);
            int account2Depth = getAccountDepth(account2);
            if (account1Depth > account2Depth)
            {
                return 1;
            }
            else if (account1Depth < account2Depth)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        };
        // sort accountsRemoved with descending depth (delete leaf accounts first, work up tree)
        accountsRemoved.Sort(comparisonFunction);
        accountsRemoved.Reverse();
        // sort accountsAdded with ascending depth (add top-level accounts first, work down tree)
        accountsAdded.Sort(comparisonFunction);
        List<BookMod> account_steps = new List<BookMod>();
        // for accounts in accountsAdded and accountsRemoved: make into BookMod and add to list
        foreach (Account account in accountsAdded)
        {
            BookMod step = new AccountMod(ModType.ADD, account);
            account_steps.Add(step);
        }
        foreach (Account account in accountsRemoved)
        {
            BookMod step = new AccountMod(ModType.REMOVE, account);
            account_steps.Add(step);
        }
        return account_steps;
    }

    static List<BookMod> GenerateTransactionSteps(Book oldBook, Book newBook)
    {
        throw new NotImplementedException();
        /*
        ignore removed accounts
        foreach account:
            let T(n) be the account's transactions in newBook
            let T(o) be the account's transactions in oldBook
            let intersection be T(n) intersects T(o)
            let transactionsRemoved be T(o) \ intersection
            let transactionsAdded be T(n) \ intersection
            return transactionsAdded union transacionsRemoved
        */

    }
}

// The type of modification that is being done
enum ModType
{
    ADD,
    REMOVE
    // TODO: add EDIT

}
// A single modification to a book
abstract class BookMod
{
    protected ModType typeOfModification;

    public BookMod(ModType typeOfModification)
    {
        this.typeOfModification = typeOfModification;
    }

    public abstract void ApplyMod(Book book);
    public abstract void DisplayMod();

}

class AccountMod : BookMod
{
    Account account;
    public AccountMod(ModType typeOfModification, Account account) : base(typeOfModification)
    {
        this.account = account;
    }

    public override void ApplyMod(Book book)
    {
        throw new NotImplementedException();
    }

    public override void DisplayMod()
    {
        throw new NotImplementedException();
    }

}

class TransactionMod : BookMod
{
    Transaction transaction;

    public TransactionMod(ModType typeOfModification, Transaction transaction) : base(typeOfModification)
    {
        this.transaction = transaction;
    }
    public override void ApplyMod(Book book)
    {
        throw new NotImplementedException();
    }

    public override void DisplayMod()
    {
        throw new NotImplementedException();
    }

}