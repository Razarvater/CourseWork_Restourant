using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<ProductItem> Products { get => ProductsModel.products; }

        public async Task InitModel()
        { 
            await ProductsModel.InitModel();
            OnPropertyChanged(nameof(Products));
        }
    }
}
