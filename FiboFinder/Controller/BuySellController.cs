using FiboFinder.quikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace FiboFinder
{
    public class BuySellController
    {
        List<Order> existsLimitOrders = new List<Order>();
        QuikConnection quikConnection;

        public BuySellController()
        {
            quikConnection = new QuikConnection();
        }

        public void setLimitOrder(ToolInfo toolInfo, Tool tool, string operation, int volume)
        {
            volume = getLotAmounts(tool, volume);

            if (operation.Equals("Long"))
            {
                try
                {
                    if (!checkToolExistInCollection(existsLimitOrders, tool))
                    {
                        createStopOrder(tool, toolInfo, volume);
                    }
                }
                catch (Exception)
                {
                    new Exception("не удалось выставить ордер на покупку");
                }
            }
            else
            {
                try
                {
                    if (!checkToolExistInCollection(existsLimitOrders, tool))
                    {
                        createStopOrder(tool, toolInfo, volume);
                    }
                }
                catch (Exception)
                {
                    new Exception("не удалось выставить ордер на продажу");
                }
            }
        }

        private int getLotAmounts(Tool tool, int volume)
        {
            if (volume == 0)
            {
                var currentBalance = Properties.Settings.Default.OpenBalance; //48 000
                var preisProLot = (tool.LastPrice * tool.Lot); // 1000

                var maxLotAvaiability = (currentBalance / double.Parse(preisProLot.ToString()) - 2); // 48
                int maxLots = (int)Math.Floor(maxLotAvaiability);

                return maxLots;
            }
            return volume;
        }

        public void removeLimitOrder(ToolInfo toolInfo, Tool tool, string operation, int volume)
        {
            if (operation.Equals("Buy"))
            {
                try
                {
                    if (!checkToolExistInCollection(existsLimitOrders, tool))
                    {
                        new Thread(() =>
                        {
                            quikConnection.getQuikExemplar()
                        .Orders
                        .KillOrder(
                                existsLimitOrders.Find(x => x.SecCode == tool.SecurityCode));
                        }).Start();
                        existsLimitOrders.RemoveAll(x => x.SecCode == tool.SecurityCode);
                    }
                }
                catch (Exception)
                {
                    new Exception("не удалось выставить ордер на покупку");
                }
            }
            else
            {
                try
                {
                    if (!checkToolExistInCollection(existsLimitOrders, tool))
                    {
                        new Thread(() =>
                        {
                            var order = quikConnection.getQuikExemplar()
                        .Orders
                        .SendLimitOrder(
                            quikConnection.getToolClass(toolInfo.SecCode), toolInfo.SecCode, tool.AccountID, Operation.Sell, toolInfo.PreisPlane, volume).Result;
                            existsLimitOrders.Add(order);
                        }).Start();
                    }
                }
                catch (Exception)
                {
                    new Exception("не удалось выставить ордер на продажу");
                }
            }
        }

        private bool checkToolExistInCollection(List<Order> orderCollection, Tool tool)
        {
            foreach (var order in orderCollection)
            {
                if (order.SecCode.Equals(tool.SecurityCode))
                {
                    return true;
                }
            }
            return false;
        }

        public void removeLimitOrder()
        {

            //foreach (Order toolInfo in existsLimitOrders)
            //{
            //    Tool tool = new Tool(quikConnection.getQuikExamplar(), toolInfo.SecCode, quikConnection.getToolClass(toolInfo.SecCode));

            //    if (toolInfo.Direction.Equals("Long"))
            //    {
            //        var difference = UtilClass.calculateDefferenceBetweenPrices(double.Parse(toolInfo.PreisPlane.ToString()), double.Parse(tool.LastPrice.ToString()));

            //        if (difference > UtilClass.differenceToRemoveOrder)
            //        {

            //        }
            //    }
            //    else
            //    {

            //    }
            //}

        }

        public void createStopOrder()
        {
            //StopOrder stopOrder = new StopOrder()
            //{
            //    Account = tool.AccountID,
            //    ClassCode = tool.ClassCode,
            //    ClientCode = Properties.Settings.Default.ClientCode,
            //    Quantity = 1,
            //    StopOrderType = StopOrderType.StopLimit,
            //    SecCode = tool.SecurityCode,
            //    ConditionPrice = toolInfo.PreisPlane + 0.5m,
            //    Price = toolInfo.PreisPlane,
            //    Operation = Operation.Buy
            //};
            //var limitOrder = quikConnection.getQuikExamplar()
            //  .StopOrders.CreateStopOrder(stopOrder);


        }

        private void createStopOrder(Tool tool, ToolInfo toolInfo, int volume)
        {
            StopOrder stopOrder = new StopOrder
            {
                Account = tool.AccountID,
                ClassCode = tool.ClassCode,
                ClientCode = toolInfo.ClientCode,
                Quantity = volume,
                StopOrderType = StopOrderType.StopLimit,
                SecCode = tool.SecurityCode,
                Condition = QuikSharp.DataStructures.Condition.LessOrEqual,
                Operation = toolInfo.Direction == "Long" ? Operation.Buy : Operation.Sell,
                Price = toolInfo.PreisPlane
            };

            new Thread(() =>
           {
               quikConnection.getQuikExemplar().StopOrders.CreateStopOrder(stopOrder);
           }).Start();
        }
    }
}
