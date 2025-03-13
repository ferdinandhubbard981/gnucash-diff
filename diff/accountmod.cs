using NC = NetCash;
namespace GNCDiff;
public class RemoveAccountMod : IBookMod
{
    Account account;
    public RemoveAccountMod(Account account)
    {
        this.account = account;
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

public class AddAccountMod : IBookMod
{
    Account account;
    public AddAccountMod(Account account)
    {
        this.account = account;
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