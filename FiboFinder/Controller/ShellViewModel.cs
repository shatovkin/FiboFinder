using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Screens;

namespace FiboFinder
{
    public class ShellViewModel : Screen
    {
        public BindableCollection<ComboItems> ComboItemsF { get; set; }

        public ShellViewModel()
        {
            DataAccess data = new DataAccess();
            ComboItemsF = new BindableCollection<ComboItems>(data.getComboItems()); 
        }
    }
}
