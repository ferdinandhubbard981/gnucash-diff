using NC = NetCash;
namespace GNCDiff;
public class Split {
    Account account;
    NC.GncNumeric amount;
    String memo;
    public Split(Account account, NC.GncNumeric quantity, String memo)
    {
        this.account = account;
        this.amount = quantity;
        this.memo = memo;
    }

    public static Split FromNCSplit(NC.Split ncSplit, in Book book)
    {
        Account? account = book.GetAccount(ncSplit.Account.FullName);
        if (account == null) 
        {
            throw new ArgumentException("Account not found: " + ncSplit.Account.FullName);
        }
        return new Split(account, ncSplit.Amount, ncSplit.Memo);
    }
}