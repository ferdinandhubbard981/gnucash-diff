using NC = NetCash;
namespace GNCDiff;
class Transaction
{
    public Guid guid;
    public List<Split> splits {get; set;}
    public Transaction(Guid guid, List<Split> splits)
    {
        this.guid = guid;
        this.splits = splits;
    }

    public static Transaction FromNCTransaction(in Book book, NC.Transaction ncTransaction)
    {
        throw new NotImplementedException();
        /*
        Guid guid = Util.GetGuidFromGNCEntity(ncTransaction);
        List<Split> splits = new List<Split>();
        Transaction transaction = new Transaction(guid, splits);
        transaction.splits = book.GetSplits(transaction);
        return transaction;
        */
    }

}