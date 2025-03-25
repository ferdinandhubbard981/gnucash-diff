namespace GNCDiff;

public class Diff
{
    // contains a list of steps to transform book A into book B
    public List<IBookMod> steps {get;}

    public Diff(List<IBookMod> steps)
    {
        this.steps = steps;
    }

    public static Diff FromBooks(string oldBookPath, string newBookPath)
    {
        Book oldBook = Book.FromGNCFile(oldBookPath);
        Book newBook = Book.FromGNCFile(newBookPath);
        return Diff.FromBooks(oldBook, newBook);
    }
    public static Diff FromBooks(Book oldBook, Book newBook)
    {
        List<IBookMod> steps = new List<IBookMod>();
        steps.AddRange(GenerateAccountSteps(oldBook, newBook));
        steps.AddRange(GenerateSplitSteps(oldBook, newBook));
        // steps.AddRange(GenerateTransactionSteps(oldBook, newBook));
        return new Diff(steps);
    }

    static List<IBookMod> GenerateAccountSteps(Book oldBook, Book newBook)
    {
        List<IBookMod> account_steps = new List<IBookMod>();
        List<Account> oldAccounts = (List<Account>) oldBook.GetAccounts();
        List<Account> newAccounts = (List<Account>) newBook.GetAccounts();
        // let intersection be oldBook intersects newBook
        List<Account> intersection = oldAccounts.Intersect(newAccounts).ToList();
        // let accountsRemoved be oldBook \ intersection
        List<Account> accountsRemoved = oldAccounts.Except(intersection).ToList();
        // let accountsAdded be newBook \ intersection
        List<Account> accountsAdded = newAccounts.Except(intersection).ToList();

        // iterate backwards, so that we can remove items without affecting loop
        for (int i = accountsAdded.Count-1; i >= 0; i--)
        {
            Account addedAccount = accountsAdded[i];
            for (int j = accountsRemoved.Count - 1; j >= 0; j--)
            {
                Account removedAccount = accountsRemoved[j];
                if (addedAccount.guid == removedAccount.guid)
                {
                    List<IBookMod> modifications = Diff.FindAccountMods(removedAccount, addedAccount);
                    if (modifications.Count > 0)
                    {
                        account_steps.AddRange(modifications);
                        // we found a set of modifications that map the old account to the new account, therefore we remove the added and removed account steps
                        accountsAdded.Remove(addedAccount);
                        accountsRemoved.Remove(removedAccount);
                    }
                }
            }
        }

        Comparison<Account> comparisonFunction = delegate (Account account1, Account account2)
        {
            Func<Account, int> getAccountDepth = delegate (Account account)
            {
                Account? parent = account.parent;
                int depth = 0;
                while (parent != null)
                {
                    depth++;
                    parent = parent.parent;
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
        // for accounts in accountsAdded and accountsRemoved: make into BookMod and add to list
        foreach (Account account in accountsAdded)
        {
            IBookMod step = new AddAccountMod(account);
            account_steps.Add(step);
        }
        foreach (Account account in accountsRemoved)
        {
            IBookMod step = new RemoveAccountMod(account);
            account_steps.Add(step);
        }
        return account_steps;
    }

    public static List<IBookMod> GenerateSplitSteps(Book oldBook, Book newBook)
    {
        List<Split> oldSplits = oldBook.splits;
        List<Split> newSplits = newBook.splits;
        List<Split> intersection = oldSplits.Intersect(newSplits).ToList();
        List<Split> removedSplits = oldSplits.Except(intersection).ToList();
        List<Split> addedSplits = newSplits.Except(intersection).ToList();
        List<IBookMod> split_steps = new List<IBookMod>();
        foreach (Split split in removedSplits)
        {
            split_steps.Add(new RemoveSplitMod(split));
        }
        foreach (Split split in addedSplits)
        {
            split_steps.Add(new AddSplitMod(split));
        }
        return split_steps;
    }

    // this function tries to map the old account into the new account. It assumes that their GUIDs are identical.
    public static List<IBookMod> FindAccountMods(Account oldAccount, Account newAccount)
    {
        List<IBookMod> accountModifications = new List<IBookMod>();
        if (oldAccount.fullName != newAccount.fullName || oldAccount.name != newAccount.name) 
        {
            accountModifications.Add(new MoveAccountMod(oldAccount, newAccount));
        }
        if (oldAccount.isPlaceholder != newAccount.isPlaceholder)
        {
            accountModifications.Add(new IsPlaceholderAccountMod(oldAccount, newAccount));
        }
        return accountModifications;

    }

    public String ToDiffString()
    {
        List<String> stepStrings = new List<string>();
        foreach (IBookMod step in this.steps) stepStrings.Add(step.ToDiffString());
        String output = String.Join('\n', stepStrings);
        output = "\n" + output;
        return output;
    }
}