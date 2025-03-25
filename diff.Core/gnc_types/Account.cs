using NC = NetCash;
namespace GNCDiff;
public class Account : IEquatable<Account>
{
    public Guid guid {get;}
    public String fullName {get;}
    public String name {get;}
    public List<Account> children {get;}
    public Account? parent {get;}

    public bool isPlaceholder {get;}
    public Account(Guid guid, String fullName, String name, List<Account> children, Account? parent, bool isPlaceholder)
    {
        this.guid = guid;
        this.fullName = fullName;
        this.name = name;
        this.children = children;
        this.parent = parent;
        this.isPlaceholder = isPlaceholder;
    }

    public static Account FromNCAccount(NC.Account ncAccount, Account? parent = null)
    {
        Guid guid = Util.GetGuidFromGNCEntity(ncAccount);
        List<Account> children = new List<Account>();
        Account newAccount = new Account(guid, ncAccount.FullName, ncAccount.Name, children, parent, ncAccount.IsPlaceholder);
        foreach (NC.Account ncChild in ncAccount.Children)
        {
            Account child = Account.FromNCAccount(ncChild, newAccount);
            newAccount.children.Add(child);
        }
        return newAccount;
    }

    public bool Equals(Account? other)
    {
        if (other == null) return false; // tocheck that this does not return false if this and other are both null
        if (this.guid != other.guid) return false;
        if (this.fullName != other.fullName) return false;
        if (this.name != other.name) return false;
        if (this.parent != null && other.parent != null)
        {
            if (this.parent.guid != other.parent.guid)
            {
                return false;
            }
        }
        else if (this.parent != null || other.parent != null) return false; // if they are not both null, then one of them is null and the other isn't
        // children do not make up the account, they are just there for convenience, so they are not checked
        if (this.isPlaceholder != other.isPlaceholder) return false;
        return true;

    }
    public override int GetHashCode()
    {
        Guid? parent_guid = this.parent == null ? null : this.parent.guid; // we don't care if the parent's data members change, as long as its guid remains the same
        return HashCode.Combine(this.guid, this.fullName, this.name, parent_guid, this.isPlaceholder);
    }
}