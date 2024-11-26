namespace WinBoosterNative.database.error_fix
{
    public interface IErrorFixerWorker
    {
        string GetName();

        bool IsAvalible();

        bool TryFix();
    }
}
