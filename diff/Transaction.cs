using NC = NetCash;
namespace GNCDiff;
class Transaction
{
    public List<Split> splits {get;}
    public Transaction(List<Split> splits)
    {
        this.splits = splits;
    }

    public static Transaction FromNCTransaction(NC.Transaction ncTransaction)
    {
        throw new NotImplementedException();
    }

}