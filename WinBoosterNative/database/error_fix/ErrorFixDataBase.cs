using WinBoosterNative.database.error_fix.workers;

namespace WinBoosterNative.database.error_fix
{
    public class ErrorFixDataBase
    {

        public List<IErrorFixerWorker> workers = new List<IErrorFixerWorker>();

        public ErrorFixDataBase()
        {
            workers.Add(new TaskManager());
            workers.Add(new Regedit());
            workers.Add(new No_Windows_Close());
            workers.Add(new No_Context_Menu());
            workers.Add(new Incorrect_Auto_Run());
        }
    }
}
