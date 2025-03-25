using NC = NetCash;
namespace GNCDiff;
public class RemoveAccountMod : IBookMod
{
    Account account;
    public RemoveAccountMod(Account account)
    {
        this.account = account;
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

    public String ToDiffString()
    {
        return $"Moved Account {this.accountBefore.fullName} to {this.accountAfter.fullName}";
    }
}

public class IsPlaceholderAccountMod : IBookMod
{
    Account accountBefore;
    Account accountAfter;
    public IsPlaceholderAccountMod(Account accountBefore, Account accountAfter)
    {
        this.accountBefore = accountBefore;
        this.accountAfter = accountAfter;
    }

    public String ToDiffString()
    {
        return $"The account {this.accountBefore.name} placeholder status changed from {accountBefore.isPlaceholder} to {accountAfter.isPlaceholder}";
    }
}