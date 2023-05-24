using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<ProductItem> Products { get => ProductsModel.products; }

        private Command addNewProductCommand;
        public Command AddNewProductCommand
        {
            get => addNewProductCommand;
            set
            { 
                addNewProductCommand = value;
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            addNewProductCommand = new Command
            (
                async (obj) => await ProductsModel.CreateNewProductAsync()
            );
        }

        public async Task InitModel()
        { 
            await ProductsModel.InitModel();
            OnPropertyChanged(nameof(Products));
        }
    }
}
