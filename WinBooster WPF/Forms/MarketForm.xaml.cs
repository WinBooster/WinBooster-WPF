namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для MarkerForm.xaml
    /// </summary>
    public partial class MarketForm : HandyControl.Controls.Window
    {
        public MarketForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
