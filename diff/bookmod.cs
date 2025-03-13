using NC = NetCash;
namespace GNCDiff;
// A single modification to a book
public interface IBookMod
{
    public abstract void ApplyMod(out NC.Book book);
    public abstract void DisplayMod();
    // public abstract String ToString();

}