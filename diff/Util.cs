namespace GNCDiff;

public static class GNCInitialiseTracker
{
    static bool initialised = false;
    public static void InitialiseGNCEngine()
    {
        if (initialised == false)
        {
            initialised = true;
            NetCash.GnuCashEngine.Initialize();
        }
    }
}