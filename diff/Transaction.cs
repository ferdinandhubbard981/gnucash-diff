using NC = NetCash;
class Transaction
{
    List<Split> splits;
    public Transaction(List<Split> splits)
    {
        this.splits = splits;
    }

    public static Transaction FromNCTransaction(NC.Transaction ncTransaction)
    {
        throw new NotImplementedException();
    }

}