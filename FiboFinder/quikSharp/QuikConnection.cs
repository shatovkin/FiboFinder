using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Linq;


namespace FiboFinder.quikSharp
{
    class QuikConnection
    {
        Quik _quik;
        bool isServerConnected = false;
        string classAndCodeDetected = "";

        public QuikConnection()
        {
            _quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
        }
        public string connectToQuik()
        {
            if (_quik != null)
            {
                try
                {
                    isServerConnected = _quik.Service.IsConnected().Result;

                    if (isServerConnected)
                    {
                        return "Соединение с сервером установлено.";
                    }
                    else
                    {
                        return "Соединение с сервером НЕ установлено.";
                    }
                }
                catch
                {
                    return "Неудачная попытка получить статус соединения с сервером.";
                }
            }
            return "Соединение с сервером НЕ установлено.";
        }

        public List<Candle> compareTwoLastDays(ToolInfo toolInfo)
        {
            classAndCodeDetected = _quik.Class.GetSecurityClass(toolInfo.ClassCode, toolInfo.SecCode).Result;

            if (!classAndCodeDetected.Equals(""))
            {
                return compareLastTwoDays(toolInfo);
            }

            return null;
        }

        public CandleInterval getCandleTimeFrame(string timeFrame)
        {
            if (timeFrame.Equals("H1"))
            {
                return CandleInterval.H1;
            }
            else if (timeFrame.Equals("H4"))
            {
                return CandleInterval.H4;
            }
            else if (timeFrame.Equals("D1"))
            {
                return CandleInterval.D1;
            }
            return CandleInterval.H1;
        }

        public Quik getQuikExamplar()
        {
            return _quik;
        }

        private List<Candle> compareLastTwoDays(ToolInfo tool)
        {

            List<Candle> candleList = _quik.Candles.GetLastCandles(tool.ClassCode, tool.SecCode, getCandleTimeFrame(tool.TimeFrame), 1).Result;

            List<Candle> first = candleList.GetRange(10, 10);
            List<Candle> second = candleList.GetRange(0, 10);

            first.Reverse(0, first.Count());
            second.Reverse(0, second.Count());


            decimal maxFir = first.Max(x => x.High);
            decimal maxSec = second.Max(x => x.High);

            if (maxFir < maxSec)
            {
                // 1. сравни с третьим отрезком
                // 2. если максимум 2-ого отрезка меньше макс. третьего дня => сравни 3 дней с 4 днем 
            }
            else
            {
                // max первого дня это точка 1
            }

            return candleList;
        }

        public string getToolClass(string secCode) {
            string codesString = "INDX,SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB,QJSIM";
            string code = _quik.Class.GetSecurityClass(codesString, secCode).Result;
            return code; 
        }
    }
}
