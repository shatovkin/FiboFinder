using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FiboFinder.views
{
    /// <summary>
    /// Interaction logic for UserOptions.xaml
    /// </summary>
    public partial class UserOptions : Window
    {
        public UserOptions(MainWindow main)
        {
            InitializeComponent();
            clientCodeTxt.Text = Properties.Settings.Default.ClientCode;
            this.Left = main.Left+main.Width/4;
            this.Top = main.Top+50;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void saveSettings_click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ClientCode = clientCodeTxt.Text;
            Properties.Settings.Default.Save();
        }
    }
}
