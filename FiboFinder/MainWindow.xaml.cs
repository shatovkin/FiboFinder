using FiboFinder.quikSharp;
using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiboFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QuikConnection quik;
        private Tool tool;

        public MainWindow()
        {
            InitializeComponent();
            quik = new QuikConnection();
        }

        private void btn_connectToQuick(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.Invoke((Action)(() =>
                {
                    txt_logs.Text = quik.connectToQuik();
                }));

        List<Candle> candleList = quik.getCandleList("TQBR", "SBER", "H1", 30);

           decimal max = candleList.Max(x => x.High);
           decimal min = candleList.Max(x => x.Close);

        }
    }
}
