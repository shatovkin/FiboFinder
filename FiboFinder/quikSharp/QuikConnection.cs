using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public static implicit operator Quik(QuikConnection v)
        {
            throw new NotImplementedException();
        }

        public List<Candle> getCandleList(string toolClass, string toolCode, string timeFrame, int candlesAmount)
        {
            classAndCodeDetected = _quik.Class.GetSecurityClass(toolClass, toolCode).Result;

            if (!classAndCodeDetected.Equals(""))
            {
                return _quik.Candles.GetLastCandles(toolClass, toolCode, getCandleTimeFrame(timeFrame), candlesAmount).Result;
            }

            return null;
        }

        private CandleInterval getCandleTimeFrame(string timeFrame)
        {
            if (timeFrame.Equals("H1"))
            {
                return CandleInterval.H1;
            }
            else
            if (timeFrame.Equals("H4"))
            {
                return CandleInterval.H4;
            }
            else if (timeFrame.Equals("D1"))
            {
                return CandleInterval.H1;
            }
            return CandleInterval.H1;
        }
    }
}
