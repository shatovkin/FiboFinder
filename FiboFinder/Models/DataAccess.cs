using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FiboFinder
{
    public class DataAccess
    {
        public List<ComboItems> NameCollection { get; set; }
        public DataAccess()
        {
            NameCollection = getComboItems();
        }
        public List<ComboItems> getComboItems()
        {
            List<ComboItems> itemList = new List<ComboItems>();

            ComboItems comItem1 = new ComboItems();
            comItem1.ItemName = "Лонг";
            comItem1.Color = Brushes.Green;

            ComboItems comItem2 = new ComboItems();
            comItem2.ItemName = "Шорт";
            comItem2.Color = Brushes.Red;

            ComboItems comItem3 = new ComboItems();
            comItem3.ItemName = "Откл.";
            comItem3.Color = Brushes.Gray;

            itemList.Add(comItem3);
            itemList.Add(comItem1);
            itemList.Add(comItem2);

            return itemList;
        }
    }
}
