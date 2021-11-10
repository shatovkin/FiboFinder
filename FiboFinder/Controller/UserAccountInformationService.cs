using FiboFinder.quikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiboFinder.Controller
{
    public class UserAccountInformationService
    {
        public bool checkConnectionToWorkPlace(QuikConnection connectionQuik)
        {
            if (int.Parse(connectionQuik.getQuikExemplar().Service.IsConnected().Result.ToString()) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkMoneyLimitOnUserAccount(QuikConnection connectionQuik) {

            connectionQuik.getQuikExemplar();
            return true;
        }
    }
}
