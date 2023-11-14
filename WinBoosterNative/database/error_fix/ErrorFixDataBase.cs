using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinBoosterNative.database.error_fix.workers;

namespace WinBoosterNative.database.error_fix
{
    public class ErrorFixDataBase
    {

        public List<IErrorFixerWorker> workers = new List<IErrorFixerWorker>();

        public ErrorFixDataBase()
        {
            workers.Add(new TaskManager());
        }
    }
}
