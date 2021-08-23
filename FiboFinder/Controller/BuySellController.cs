using FiboFinder.quikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (operation.Equals("Long"))
            {
                try
                {
                    if (!checkToolExistInCollection(existsLimitOrders, tool))
                    {
                        new Thread(() =>
                        {
                            var limitOrder = quikConnection.getQuikExamplar()
                           .Orders
                           .SendLimitOrder(quikConnection.getToolClass(toolInfo.SecCode), toolInfo.SecCode, tool.AccountID, Operation.Buy, toolInfo.PreisPlane, volume);
                            existsLimitOrders.Add(limitOrder.Result);
                        });
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
                        var limitOrder = quikConnection.getQuikExamplar()
                       .Orders
                       .SendLimitOrder(
                           quikConnection.getToolClass(toolInfo.SecCode), toolInfo.SecCode, tool.AccountID, Operation.Sell, toolInfo.PreisPlane, volume);
                        existsLimitOrders.Add(limitOrder.Result);
                    }).Start();

                    }
                }
                catch (Exception)
                {
                    new Exception("не удалось выставить ордер на продажу");
                }
            }
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
                            quikConnection.getQuikExamplar()
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
                            var order = quikConnection.getQuikExamplar()
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
    }
}
