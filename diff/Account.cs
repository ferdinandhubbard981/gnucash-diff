using NC = NetCash;
namespace GNCDiff;
public class Account
{
    public Guid guid {get;}
    public String fullName {get;}
    public String name {get;}
    public List<Account> children {get;}
    public Account? parent {get;}
    public Account(Guid guid, String fullName, String name, List<Account> children, Account? parent)
    {
        this.guid = guid;
        this.fullName = fullName;
        this.name = name;
        this.children = children;
        this.parent = parent;
    }

    public static Account FromNCAccount(NC.Account ncAccount, Account? parent = null)
    {
        Guid guid = Util.GetGuidFromGNCEntity(ncAccount);
        List<Account> children = new List<Account>();
        Account newAccount = new Account(guid, ncAccount.FullName, ncAccount.Name, children, parent);
        foreach (NC.Account ncChild in ncAccount.Children)
        {
            Account child = Account.FromNCAccount(ncChild, newAccount);
            newAccount.children.Add(child);
        }
        return newAccount;
    }
}