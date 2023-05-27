using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterNative.database.cleaner
{
    public interface ICleanerCategory
    {
        bool IsAvalible();
        string GetCategory();
        List<ICleanerWorker> GetWorkers();
    }
}
