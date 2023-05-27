using mvvm;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal class ViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<ProductItem> Products { get => ProductsModel.products; }
        public ObservableCollection<DishItem> FoodList { get => DishesModel.dishes; }

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

        private Command addNewDishCommand;
        public Command AddNewDishCommand
        {
            get => addNewDishCommand;
            set
            {
                addNewDishCommand = value;
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            addNewProductCommand = new Command
            (
                async (obj) => await ProductsModel.CreateNewProductAsync()
            );
            AddNewDishCommand = new Command
            (
                async (obj) => await DishesModel.CreateEmptyDishAsync()
            );
        }

        public async Task InitModel()
        { 
            await ProductsModel.InitModel();
            OnPropertyChanged(nameof(Products));

            await DishesModel.InitModel();
            OnPropertyChanged(nameof(FoodList));
        }
    }
}
