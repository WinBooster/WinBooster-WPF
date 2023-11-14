namespace WinBoosterNative.data
{
    public class Settings : ISavable<Settings>
    {
        public override string GetPath()
        {
            return "C:\\Program Files\\WinBooster\\settings.json";
        }
        public string password = string.Empty;
        public bool? discordRich = false;
        public bool? DisableScreenCapture = true;
        public bool? first_run = true;
    }
}
