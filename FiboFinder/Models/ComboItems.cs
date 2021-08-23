using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FiboFinder
{
    public class ComboItems
    {
        private string itemName;
        private Brush color;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public Brush Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
