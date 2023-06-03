using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterScripts
{
    public class Script
    {
        #region Системное
        private CharpManager _handler = null;

        private CharpManager Handler
        {
            get
            {
                if (master != null)
                    return master.Handler;
                if (_handler != null)
                    return _handler;
                throw new InvalidOperationException("Error");
            }
        }
        public void SetHandler(CharpManager handler) { this._handler = handler; }
        private Script master = null;
        protected void SetMaster(Script master) { this.master = master; }
        #endregion

        #region Загрузка и выгрузка плагина
        protected void LoadScript(Script bot)
        {
            Handler.ScriptUnLoad(bot); Handler.ScriptLoad(bot);
        }
        protected void UnLoadScript(Script plugin)
        {
            Handler.ScriptUnLoad(plugin);

            if (Handler.OnScriptUnload != null)
            {
                Handler.OnScriptUnload(plugin);
            }
        }
        protected void UnLoadScript()
        {
            UnLoadScript(this);
        }
        protected void RunFile(string filename)
        {
            Handler.ScriptLoad(new File(filename));
        }
        #endregion

        #region Ивенты плагина

        public virtual void Initialize() { }

        public virtual void Update() { }

        public virtual void OnClearStart(bool cheats, bool logs, bool cache, bool lastactivity, bool registry, bool images, bool video) { }
        public virtual void OnClearDone(bool cheats, bool logs, bool cache, bool lastactivity, bool registry, bool images, bool video, long size) { }

        public virtual void OnErrorFixerStart() { }
        public virtual void OnErrorFixerDone() { }

        public virtual void OnGameOptimizeStart() { }
        public virtual void OnGameOptimizeDone() { }

        public virtual void ReceivedObject(object s) { }

        public virtual void ReceivedObject<T>(T s) { }
        #endregion

        #region Методы плагина

        protected void PluginPostObject(object obj)
        {
            Handler.OnPluginPostObjectMethod(this, obj);
        }
        #endregion
    }
}
