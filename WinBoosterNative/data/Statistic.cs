using WinBoosterNative.data.statistics;

namespace WinBoosterNative.data
{
    public class Statistic : ISavable<Statistic>
    {
        public override string GetPath()
        {
            return "C:\\Program Files\\WinBooster\\Statistic\\statistic.json";
        }

        public Cleaner cleaner = new Cleaner();
    }
}
