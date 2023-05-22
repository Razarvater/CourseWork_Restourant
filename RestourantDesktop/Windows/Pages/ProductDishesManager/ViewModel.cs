using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<ProductItem> Products { get; set; }

        public ViewModel()
        {
            Products = new ObservableCollection<ProductItem>
            {
                new ProductItem(0, "ааа", "C:\\Users\\risma\\OneDrive\\Bureau\\БД.png", 10)
            };
        }
    }
}
