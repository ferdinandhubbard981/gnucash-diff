using NC = NetCash;
namespace GNCDiff;
public class Account
{
    public String fullName {get;}
    public String name {get;}
    public List<Account> children {get;}
    public Account? parent {get;}
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