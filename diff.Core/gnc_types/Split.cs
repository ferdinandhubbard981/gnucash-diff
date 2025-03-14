using NC = NetCash;
namespace GNCDiff;
public class Split : IEquatable<Split> {
    public Guid guid {get;}
    public Account account {get;}
    public NC.GncNumeric amount {get;}
    public String memo {get;}
    public Split(Guid guid, Account account, NC.GncNumeric quantity, String memo)
    {
        this.guid = guid;
        this.account = account;
        this.amount = quantity;
        this.memo = memo;
    }

    public static Split FromNCSplit(NC.Split ncSplit, in Book book)
    {
        Account? account = book.GetAccount(Util.GetGuidFromGNCEntity(ncSplit.Account));
        if (account == null) 
        {
            throw new ArgumentException("Account not found: " + ncSplit.Account.FullName);
        }
        Guid guid = Util.GetGuidFromGNCEntity(ncSplit);
        return new Split(guid, account, ncSplit.Amount, ncSplit.Memo);
    }
    public bool Equals(Split? other)
    {
        if (other == null) return false; // tocheck that this does not return false if this and other are both null
        if (this.guid != other.guid) return false;
        if (this.account.guid != other.account.guid) return false;
        if (this.amount != other.amount) return false;
        if (this.memo != other.memo) return false;
        return true;

    }

    public override int GetHashCode()
    {
        // only including account.guid, because if something about the account changes, that is not important. We only care if the account that this split is pointing to has changed.
        return HashCode.Combine(this.guid, this.account.guid, this.amount, this.memo);
    }

}