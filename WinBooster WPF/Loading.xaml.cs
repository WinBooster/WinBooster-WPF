using HandyControl.Controls;
using System;

namespace WinBooster_WPF
{
    /// <summary>
    /// Логика взаимодействия для Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.SuperExit();
        }
    }
}
