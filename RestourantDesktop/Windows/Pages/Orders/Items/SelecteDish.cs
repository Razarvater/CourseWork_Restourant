using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;
using System.Linq;

namespace RestourantDesktop.Windows.Pages.Orders.Items
{
    internal class SelecteDish : NotifyPropertyChanged
    {
        public ObservableCollection<DishItem> Dishes { get => DishesModel.dishes; }

        public string Picture { get => selectedDish?.Pictures.FirstOrDefault()?.Adress; }

        private int count;
        public int Count
        { 
            get => count;
            set
            { 
                count = value;
                OnPropertyChanged();
            }
        }

        private DishItem selectedDish;
        public DishItem SelectedDish
        { 
            get => selectedDish;
            set
            { 
                selectedDish = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Picture));
            }
        }

        public Command RemoveDish { get; set; }
    }
}
