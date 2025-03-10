using NC = NetCash;
class Account
{
    public String fullName {get;}
    String name;
    public List<Account> children {get;}
    Account? parent;
    public Account(String name, String fullName, List<Account> children, Account? parent)
    {
        this.name = name;
        // fullName serves as a unique ID for this account
        this.fullName = fullName;
        this.children = children;
        this.parent = parent;
    }

    public static Account FromNCAccount(NC.Account ncAccount, Account? parent = null)
    {
        List<Account> children = new List<Account>();
        Account newAccount = new Account(ncAccount.Name, ncAccount.FullName, children, parent);
        foreach (NC.Account ncChild in ncAccount.Children)
        {
            Account child = Account.FromNCAccount(ncChild, newAccount);
            newAccount.children.Add(child);
        }
        return newAccount;
    }
}