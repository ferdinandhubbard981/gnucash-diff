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

    public String ToDiffString()
    {
        return $"Removed Account {this.account.fullName}";
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

    public String ToDiffString()
    {
        return $"Added Account {this.account.fullName}";
    }
}

public class MoveAccountMod : IBookMod
{
    Account accountBefore;
    Account accountAfter;
    public MoveAccountMod(Account accountBefore, Account accountAfter)
    {
        this.accountBefore = accountBefore;
        this.accountAfter = accountAfter;
    }

    public void ApplyMod(out NC.Book book)
    {
        throw new NotImplementedException();
    }

    public String ToDiffString()
    {
        return $"Moved Account {this.accountBefore.fullName} to {this.accountAfter.fullName}";
    }
}