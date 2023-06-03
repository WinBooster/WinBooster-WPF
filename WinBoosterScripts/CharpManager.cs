using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterScripts
{
    public class CharpManager
    {
        public CharpManager(CharpUpdater updater)
        {
            updater.SetHandler(this);
            updater.StartUpdater();
        }
        public void Quit()
        {
            foreach (Script p in plugins)
            {
                ScriptUnLoad(p);
            }
        }
        private readonly Dictionary<string, List<Script>> registeredPluginsPluginChannels = new Dictionary<string, List<Script>>();
        public readonly List<Script> plugins = new List<Script>();

        #region Системное
        public static bool isUsingMono
        {
            get
            {
                return Type.GetType("Mono.Runtime") != null;
            }
        }
        public void OnUpdate()
        {
            foreach (Script bot in plugins.ToArray())
            {
                try
                {
                    bot.Update();
                }
                catch { }
            }
        }
        #endregion

        #region Получение и отправка данных от плагина
        public Action<Script> OnScriptUnload { set; get; }
        public Action<Script, object> OnScriptPostObject { set; get; }
        public Action<Script> OnScriptLoad { set; get; }
        public void OnPluginPostObjectMethod(Script plugin, object ob)
        {
            if (OnScriptPostObject != null)
            {
                OnScriptPostObject(plugin, ob);
            }
        }

        #endregion

        #region Управление плагином
        public void ScriptLoad(Script b, bool init = true)
        {
            b.SetHandler(this);
            plugins.Add(b);
            if (init)
            {
                List<Script> temp = new List<Script>();
                temp.Add(b);
                //new Plugin[] { b }
                DispatchScriptEvent(bot => bot.Initialize(), temp);

                if (OnScriptLoad != null)
                {
                    OnScriptLoad(b);
                }
            }
        }
        public void ScriptPostObject(Script b, object obj)
        {
            foreach (Script bot in plugins.ToArray())
            {
                try
                {
                    bot.ReceivedObject(obj);
                }
                catch { }
            }
        }
        public void ScriptUnLoad(Script b)
        {
            plugins.RemoveAll(item => object.ReferenceEquals(item, b));

            var botRegistrations = registeredPluginsPluginChannels.Where(entry => entry.Value.Contains(b)).ToList();
            foreach (var entry in botRegistrations)
            {
                UnregisterPluginChannel(entry.Key, b);
            }
        }
        #endregion

        #region Регистрация плагинов
        private void DispatchScriptEvent(Action<Script> action, IEnumerable<Script> botList = null)
        {
            Script[] selectedBots;

            if (botList != null)
            {
                selectedBots = botList.ToArray();
            }
            else
            {
                selectedBots = plugins.ToArray();
            }

            foreach (Script bot in selectedBots)
            {
                try
                {
                    action(bot);
                }
                catch (Exception e)
                {
                    if (!(e is ThreadAbortException))
                    {
                        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                        System.Reflection.MethodBase method = frame.GetMethod();
                        string parentMethodName = method.Name;
                        Console.WriteLine(parentMethodName + ": Got error from " + bot.ToString() + ": " + e.ToString());
                    }
                    else throw;
                }
            }
        }
        public void UnregisterPluginChannel(string channel, Script bot)
        {
            if (registeredPluginsPluginChannels.ContainsKey(channel))
            {
                List<Script> registeredBots = registeredPluginsPluginChannels[channel];
                registeredBots.RemoveAll(item => object.ReferenceEquals(item, bot));
                if (registeredBots.Count == 0)
                {
                    registeredPluginsPluginChannels.Remove(channel);
                }
            }
        }
        #endregion
    }
}
