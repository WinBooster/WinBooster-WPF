using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterNative
{
    public class FileBytes
    {
        public static string GetFileSizeString(long size)
        {
            if (size <= 0) return "0 B";
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB" };
            int digitGroups = (int)(Math.Log(size) / Math.Log(1024));
            return string.Format("{0:0.#} {1}", size / Math.Pow(1024, digitGroups), units[digitGroups]);
        }
    }
}
