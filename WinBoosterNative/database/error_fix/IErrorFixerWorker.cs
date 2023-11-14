using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterNative.database.error_fix
{
    public interface IErrorFixerWorker
    {
        string GetName();

        bool IsAvalible();

        bool TryFix();
    }
}
