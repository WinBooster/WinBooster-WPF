using CSScriptLib;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinBooster_WPF.ScriptAPI;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для ScriptsForm.xaml
    /// </summary>
    public partial class ScriptsForm : HandyControl.Controls.Window
    {
        public ScriptsForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public class DataBaseGrid
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string File { get; set; }

            public bool Enabled
            {
                get
                {
                    return false;
                }
                set
                {
                    string code = System.IO.File.ReadAllText(File);
                    var script = CSScript.Evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly().Location).LoadCode<IScript>(code);
                    script.OnEnabled();
                }
            }
        }
        public List<DataBaseGrid> list = new List<DataBaseGrid>();
        public void UpdateList2()
        {
            DataGrid.ItemsSource = list;
            double wi = 0;
            foreach (var colum in DataGrid.Columns)
            {
                wi += colum.Width.Value;
            }
            //this.Width = wi;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList2();
        }
    }
}
