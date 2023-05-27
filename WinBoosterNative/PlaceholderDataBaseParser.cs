namespace WinBoosterNative
{
    public class PlaceholderDataBaseParser
    {
        public static string Parse(string original)
        {
            string temp = original;
            temp = temp.Replace("{username}", Environment.UserName);
            DirectoryInfo steamDirectory = WinBoosterUtils.FindSteamDirectory();
            if (steamDirectory != null)
            {
                temp = temp.Replace("{steam}", steamDirectory.FullName);
            }
            return temp;
        }
    }
}
