using NC = NetCash;
namespace GNCDiff;

public class RemoveSplitMod: IBookMod
{
    public Split split {get;}
    public RemoveSplitMod(Split split)
    {
        this.split = split;
    }
    public String ToDiffString()
    {
        return $"Removed split: {(double) this.split.amount} to {this.split.account.fullName}";
    }
}
public class AddSplitMod: IBookMod
{
    public Split split {get;}
    public AddSplitMod(Split split)
    {
        this.split = split;
    }
    public String ToDiffString()
    {
        return $"Added split: {(double) this.split.amount} to {this.split.account.fullName}";
    }
}

public class ChangeAmountSplitMod: IBookMod
{
    public Split before {get;}
    public Split after {get;}

    public ChangeAmountSplitMod(Split before, Split after)
    {
        this.before = before;
        this.after = after;
    }

    public String ToDiffString()
    {
        return $"Split amount changed from {(double) before.amount} to {(double) after.amount} to {after.account.fullName}"; // this isn't very good. I must change the way the diff is dislpayed to strikethrough the old amount/account and add the new one next to it.
    }
}