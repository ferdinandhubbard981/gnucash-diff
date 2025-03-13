using NC = NetCash;
namespace GNCDiff;
public class Split {
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
}