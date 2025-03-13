using NC = NetCash;
namespace GNCDiff;
class AccountMod : BookMod
{
    NC.Account account;
    public AccountMod(ModType typeOfModification, NC.Account account) : base(typeOfModification)
    {
        this.account = account;
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