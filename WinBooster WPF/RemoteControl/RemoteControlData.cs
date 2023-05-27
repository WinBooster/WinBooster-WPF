namespace WinBooster_WPF.RemoteControl
{
    public class RemoteControlData
    {
        public Main main = new Main();
        public Discord discord = new Discord();
        public class Main
        {
            public string ip;
            public string hardwareID;
        }
        public class Discord
        {
            public ulong id = 0;
            public string name;
        }
    }
}
