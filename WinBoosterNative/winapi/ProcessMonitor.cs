using System.Diagnostics;
using System.Management;

namespace WinBoosterNative.winapi
{
    public class ProcessMonitor
    {
        public event EventHandler<Process> onProcessStarted;

        public ProcessMonitor()
        {
            var query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");

            using (var watcher = new ManagementEventWatcher(query))
            {
                watcher.Options.Timeout = new TimeSpan(0, 0, 5);
                watcher.EventArrived += OnProcessStarted;

                watcher.Start();

            }
        }
        private void OnProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var targetInstance = (ManagementBaseObject)e.NewEvent.GetPropertyValue("TargetInstance");
            var processId = Convert.ToInt32(targetInstance.GetPropertyValue("ProcessId"));
            try
            {
                var process = Process.GetProcessById(processId);
                onProcessStarted?.Invoke(null, process);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving process information: {ex.Message}");
            }
        }
    }
}
