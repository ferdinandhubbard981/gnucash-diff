using NC = NetCash;
namespace GNCDiff;

class TransactionMod : BookMod
{
    NC.Transaction transaction;

    public TransactionMod(ModType typeOfModification, NC.Transaction transaction) : base(typeOfModification)
    {
        this.transaction = transaction;
    }
    public override void ApplyMod(NC.Book book)
    {
        throw new NotImplementedException();
    }

    public override void DisplayMod()
    {
        throw new NotImplementedException();
    }

}