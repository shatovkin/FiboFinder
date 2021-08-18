using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiboFinder
{
    public class UtilClass
    {
        public static string directionShort = "Short";
        public static string directionLong = "Long";
        public static double differenceToRemoveOrder = 0.5;

        public static double calculateDefferenceBetweenPrices(double preis, double toolLastPrice)
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
    }
}
