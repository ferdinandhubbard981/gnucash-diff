using NC = NetCash;
namespace GNCDiff;
// A single modification to a book
public abstract class BookMod
{
    protected ModType typeOfModification;

    public BookMod(ModType typeOfModification)
    {
        this.typeOfModification = typeOfModification;
    }

    public abstract void ApplyMod(NC.Book book);
    public abstract void DisplayMod();

}