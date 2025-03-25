using NC = NetCash;
namespace GNCDiff;
// A single modification to a book
public interface IBookMod
{
    // ApplyMod is commented out because it will only be useful when I implement a merging feature
    // public abstract void ApplyMod(out NC.Book book);
    public abstract String ToDiffString();

}