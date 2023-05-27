using System.Collections.Generic;
using WinBoosterNative;

namespace WinBooster_WPF.Forms.OptimizeClasses
{
    public class GybernateClass
    {
        /* Проверяем включение */
        public static bool Activated()
        {
            List<string> cmdtext = new ProcessUtils().StartCmd("chcp 1251 & powercfg /a");
            foreach (string text in cmdtext)
            {
                string text1 = text;
                if (!string.IsNullOrEmpty(text1))
                {
                    if (text1.Contains("Режим гибернации не включен"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /* Включаем и выключаем */
        public void Enable(bool on)
        {
            if (on)
            {
                new ProcessUtils().StartCmd("powercfg /h on");
            }
            else
            {
                new ProcessUtils().StartCmd("powercfg /h off");
            }
        }
    }
}
