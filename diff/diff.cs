using NetCash;

class Diff(BookMod[] steps)
{
    // contains a list of steps to transform book A into book B
    private BookMod[] steps = steps;
}

// The type of modification that is being done
enum ModType
{
    ADD,
    REMOVE
    // TODO: add EDIT

}
// A single modification to a book
abstract class BookMod(ModType typeOfModification)
{
    protected ModType typeOfModification = typeOfModification;

    public abstract void ApplyMod(Book book);
    public abstract void DisplayMod();

}

class AccountMod(ModType typeOfModification, Account account) : BookMod(typeOfModification)
{
    Account account = account;

    public override void ApplyMod(Book book)
    {
        throw new NotImplementedException();
    }

    public override void DisplayMod()
    {
        throw new NotImplementedException();
    }

}

class TransactionMod(ModType typeOfModification, Transaction transaction) : BookMod(typeOfModification)
{
    Transaction transaction = transaction;

    public override void ApplyMod(Book book)
    {
        throw new NotImplementedException();
    }

    public override void DisplayMod()
    {
        throw new NotImplementedException();
    }

}