namespace GNCDiff;
using NC = NetCash;

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

public static class Util
{
    public static Guid GetGuidFromGNCEntity(NC.IGnuCashEntity entity)
    {
        nint guidPointer = NC.Bindings.qof_instance_get_guid(entity.NativeHandle);
        Guid guid = NC.Marshalling.Guid.fromPointer.Invoke(guidPointer);
        return guid;
    }
}