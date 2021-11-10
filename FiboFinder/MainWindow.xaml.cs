using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using FiboFinder.Controller;
using FiboFinder.quikSharp;
using FiboFinder.views;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using QuikSharp;
using FiboFinder.Models;

namespace FiboFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        MoneyLimitEx moneyLimit;
        private QuikConnection connectionQuik;
        private BindableCollection<ComboItems> comboUnits { get; set; }
        private Tool tool;
        private AccountInfo accountInfo; 

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataAccess();
            connectionQuik = new QuikConnection();

            writeInstrumentListToSettings();
            connectionQuik.getQuikExemplar().Events.OnMoneyLimit += Events_OnMoneyLimit;
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
            PositionCalculation posCalc = new PositionCalculation();
            posCalc.calculateEntryPosition(tool, connectionQuik, fillToolInfoListsWithData());
        }

        private void Events_OnMoneyLimit(MoneyLimitEx mLimit)
        {
            moneyLimit = mLimit;
            Properties.Settings.Default.OpenBalance = moneyLimit.CurrentBal;
            Properties.Settings.Default.CurrentLimit = moneyLimit.CurrentLimit;
            Properties.Settings.Default.Save();

            this.Dispatcher.Invoke((Action)(() =>
            {
                txt_logs.Text = "Осталось денег: " + moneyLimit.CurrentBal + " Баланс: "+ moneyLimit.OpenBal;
            }));
        }

        private List<ToolInfo> fillToolInfoListsWithData()
        {
            var clientCode = Properties.Settings.Default.ClientCode;
            var firmId = Properties.Settings.Default.FirmId;

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

                var variantComboBox = (ComboBox)this.FindName("variant" + counter);

                if (!currentComboBox.SelectedIndex.Equals(0))
                {
                    ToolInfo tool = new ToolInfo();
                    tool.Variant = variantComboBox.Text;

                    foreach (var textBox in txtList)
                    {
                        if (textBox.Name == "tool" + counter)
                        {
                            tool.ToolName = textBox.Name;
                            tool.SecCode = textBox.Text;
                            tool.ClassCode = connectionQuik.getToolClass(tool.SecCode);
                            tool.Direction = setDirection(currentComboBox.SelectedIndex);
                            tool.ClientCode = clientCode;
                            tool.FirmId = firmId;
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
            saveIntrumentListToSettings(toolInfoList);
            return toolInfoList;
        }

        private string setDirection(int indexOfComboBox)
        {
            switch (indexOfComboBox)
            {
                case 1:
                    return UtilClass.directionLong;
                case 2:
                    return UtilClass.directionShort;
            }
            return null;
        }

        private void menu_click_userOptions(object sender, RoutedEventArgs e)
        {
            UserOptions view = new UserOptions(this);
            view.Owner = System.Windows.Application.Current.MainWindow;
            view.Show();
        }

        private void saveIntrumentListToSettings(List<ToolInfo> tools)
        {

            foreach (var tool in tools)
            {
                if (tool.ToolName == "tool1")
                {
                    Properties.Settings.Default.Instr1ClassCode = tool.ClassCode;
                    Properties.Settings.Default.Instr1Direction = tool.Direction;
                    Properties.Settings.Default.Instr1Quantity = tool.Quantity;
                    Properties.Settings.Default.Instr1PreisPlane = tool.PreisPlane.ToString();
                    Properties.Settings.Default.Instr1SecCode = tool.SecCode;
                    Properties.Settings.Default.Instr1Variant = tool.Variant;

                }
                else if (tool.ToolName == "tool2")
                {
                    Properties.Settings.Default.Instr2ClassCode = tool.ClassCode;
                    Properties.Settings.Default.Instr2Direction = tool.Direction;
                    Properties.Settings.Default.Instr2Quantity = tool.Quantity;
                    Properties.Settings.Default.Instr2PreisPlane = tool.PreisPlane.ToString();
                    Properties.Settings.Default.Instr2SecCode = tool.SecCode;
                    Properties.Settings.Default.Instr2Variant = tool.Variant;
                }
            }
            Properties.Settings.Default.Save();
        }

        private void writeInstrumentListToSettings()
        {
            cbox_1.SelectedIndex = Properties.Settings.Default.Instr1Direction.Equals("Long") ? 1 : Properties.Settings.Default.Instr1Direction.Equals("Short") ? 2 : 0;
            variant1.SelectedIndex = Properties.Settings.Default.Instr1Variant.Equals("Сверху") ? 0 : 1;
            tool1.Text = Properties.Settings.Default.Instr1SecCode;
            preis1.Text = Properties.Settings.Default.Instr1PreisPlane;
            amount1.Text = Properties.Settings.Default.Instr1Quantity;


            cbox_2.SelectedIndex = Properties.Settings.Default.Instr2Direction.Equals("Long") ? 1 : Properties.Settings.Default.Instr2Direction.Equals("Short") ? 2 : 0;
            variant2.SelectedIndex = Properties.Settings.Default.Instr2Variant.Equals("Сверху") ? 0 : 1;
            tool2.Text = Properties.Settings.Default.Instr2SecCode;
            preis2.Text = Properties.Settings.Default.Instr2PreisPlane;
            amount2.Text = Properties.Settings.Default.Instr2Quantity;
        }
    }
}
