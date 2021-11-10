using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FiboFinder.views
{
    public partial class UserOptions : Window
    {
        public UserOptions(MainWindow main)
        {
            InitializeComponent();
            clientCodeTxt.Text = Properties.Settings.Default.ClientCode;
            firmId.Text = Properties.Settings.Default.FirmId;
            this.Left = main.Left + main.Width / 4;
            this.Top = main.Top + 50;
        }

        private void saveSettings_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ClientCode = clientCodeTxt.Text;
            Properties.Settings.Default.FirmId = firmId.Text;
            Properties.Settings.Default.Save();
        }

        private void clientCodeTxt_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
