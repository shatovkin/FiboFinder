using Caliburn.PresentationFramework;
using FiboFinder.quikSharp;
using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace FiboFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private QuikConnection connectionQuik;
        public BindableCollection<ComboItems> comboUnits { get; set; }

        private Tool tool;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataAccess();
            connectionQuik = new QuikConnection();

        }

        private void btn_connectToQuik(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    txt_logs.Text = connectionQuik.connectToQuik();
                }));
        }

        private void btn_clickStart(object sender, RoutedEventArgs e)
        {
            foreach (ToolInfo toolInfo in fillToolInfoListsWithData())
            {
                tool = new Tool(connectionQuik.getQuikExamplar(), toolInfo.SecCode, connectionQuik.getToolClass(toolInfo.SecCode));

                decimal toolLastPrice = decimal.Parse(connectionQuik.getQuikExamplar().Trading.GetParamEx("TQBR", toolInfo.SecCode, "LAST").Result.ParamValue);

                decimal priceIn = Math.Round(tool.LastPrice + 10 * tool.Step, tool.PriceAccuracy); 
                //double difference = calculateDefferenceAmongPrices(double.Parse(tool.Preis), toolLastPrice);

                if (toolInfo.Direction == "Long")
                {
                    if (toolLastPrice == toolInfo.PreisPlane || toolLastPrice < toolInfo.PreisPlane)
                    {
                        try
                        {
                            new Thread(() =>
                            {
                                connectionQuik.getQuikExamplar()
                            .Orders
                            .SendLimitOrder(
                                connectionQuik.getToolClass(toolInfo.SecCode), toolInfo.SecCode, tool.AccountID, Operation.Buy, toolInfo.PreisPlane, 1);
                            }).Start();
                        }
                        catch (Exception)
                        {
                            new Exception("не удалось выставить ордер на покупку");
                        }
                    }
                }
                else
                {
                    try
                    {
                        new Thread(() =>
                        {
                            connectionQuik.getQuikExamplar()
                        .Orders
                        .SendLimitOrder(
                            connectionQuik.getToolClass(toolInfo.SecCode), toolInfo.SecCode, tool.AccountID, Operation.Sell, toolInfo.PreisPlane, 1);
                        }).Start();
                    }
                    catch (Exception)
                    {
                        new Exception("не удалось выставить ордер на продажу");
                    }
                }
            }

        }

        private double calculateDefferenceBetweenPrices(double preis, double toolLastPrice)
        {
            if (preis < toolLastPrice)
            {
                return ((toolLastPrice - preis) / preis) * 100;
            }
            else
            {
                return ((preis - toolLastPrice) / preis) * 100;
            }
        }

        private List<ToolInfo> fillToolInfoListsWithData()
        {
            List<TextBox> textBoxCollection1 = stackPan1.Children.OfType<TextBox>().ToList();
            List<TextBox> textBoxCollection2 = stackPan2.Children.OfType<TextBox>().ToList();

            List<List<TextBox>> listOfTextBoxes = new List<List<TextBox>>();
            listOfTextBoxes.Add(textBoxCollection1);
            listOfTextBoxes.Add(textBoxCollection2);

            List<ToolInfo> toolInfoList = new List<ToolInfo>();

            int counter = 1;
            foreach (List<TextBox> txtList in listOfTextBoxes)
            {
                var currentComboBox = (ComboBox)this.FindName("cbox_" + counter);

                if (!currentComboBox.SelectedIndex.Equals(0))
                {
                    ToolInfo tool = new ToolInfo();

                    foreach (var textBox in txtList)
                    {
                        if (textBox.Name == "tool" + counter)
                        {
                            tool.SecCode = textBox.Text;
                            tool.ClassCode = connectionQuik.getToolClass(tool.SecCode);
                            tool.Direction = setDirection(currentComboBox.SelectedIndex);
                        }
                        else if (textBox.Name == "preis" + counter)
                        {
                            tool.PreisPlane = decimal.Parse(textBox.Text);
                        }
                        else if (textBox.Name == "amount" + counter)
                        {
                            tool.Quantity = textBox.Text;
                        }
                    }
                    toolInfoList.Add(tool);

                }
                counter++;
            }
            return toolInfoList;
        }

        private string setDirection(int indexOfComboBox)
        {

            switch (indexOfComboBox)
            {
                case 1:
                    return "Long";

                case 2:
                    return "Short";
            }
            return null;
        }
    }
}
