using NC = NetCash;
namespace GNCDiff;

public class RemoveSplitMod: IBookMod
{
    public Split split {get;}
    public RemoveSplitMod(Split split)
    {
        this.split = split;
    }
    public void ApplyMod(out NC.Book book)
    {
        throw new NotImplementedException();
    }

    public void DisplayMod()
    {
        throw new NotImplementedException();
    }
}
public class AddSplitMod: IBookMod
{
    public Split split {get;}
    public AddSplitMod(Split split)
    {
        this.split = split;
    }
    public void ApplyMod(out NC.Book book)
    {
        throw new NotImplementedException();
    }

    public void DisplayMod()
    {
        throw new NotImplementedException();
    }
}