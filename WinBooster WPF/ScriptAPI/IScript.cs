using WinBoosterNative.database.cleaner;

namespace WinBooster_WPF.ScriptAPI
{
    public class IScript
    {
        public virtual string GetScriptName() { return ""; }
        public virtual void OnEnabled() { }
        public virtual void OnCleanerInit(CleanerDataBase dataBase) { }
        public virtual void OnCleanerDone(CleanerResult result) { }
    }

    //public class ScriptNative
    //{

    //}
}
