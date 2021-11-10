using FiboFinder.quikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiboFinder.Controller
{
    public class PositionCalculation
    {
        private List<ToolInfo> boughtStocksList = new List<ToolInfo>();

        public void calculateEntryPosition(Tool tool, QuikConnection quikConnection, List<ToolInfo> fillToolInfoListsWithData)
        {
            BuySellController buySellController = new BuySellController();

            foreach (ToolInfo orderInfoFromGui in fillToolInfoListsWithData)
            {
                tool = new Tool(quikConnection.getQuikExemplar(), orderInfoFromGui.SecCode, quikConnection.getToolClass(orderInfoFromGui.SecCode));

                decimal toolLastPrice = decimal.Parse(quikConnection.getQuikExemplar().Trading.GetParamEx(orderInfoFromGui.ClassCode, orderInfoFromGui.SecCode, "LAST").Result.ParamValue);
                decimal currentPrice = Math.Round(tool.LastPrice + 10 * tool.Step, tool.PriceAccuracy);

                if (currentPrice == orderInfoFromGui.PreisPlane || calculatePerSentDifferenz(currentPrice,orderInfoFromGui.PreisPlane))
                {
                    buySellController.setLimitOrder(orderInfoFromGui, tool, orderInfoFromGui.Direction, int.Parse(orderInfoFromGui.Quantity));
                    boughtStocksList.Add(orderInfoFromGui);
                }
            }
        }

        private bool calculatePerSentDifferenz(decimal currentPrise, decimal pricePlane) {

            decimal differenceInPercentOne = (currentPrise / pricePlane - 1) * 100;

            if (differenceInPercentOne < 0) {
                differenceInPercentOne = differenceInPercentOne * -1;
            }

            if (double.Parse(differenceInPercentOne.ToString()) <= 0.30) {
                return true;
            }
            return false;
        }
    }
}
