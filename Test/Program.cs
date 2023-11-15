using Microsoft.Win32;

internal class Program
{
    private static void Main(string[] args)
    {
        var CurrentUserSoftware2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\WinRAR\\DialogEditHistory\\ArcName", true);


            foreach (var value2 in CurrentUserSoftware2.GetValueNames())
            {
            CurrentUserSoftware2.DeleteValue(value2);
        }
        
    }
}