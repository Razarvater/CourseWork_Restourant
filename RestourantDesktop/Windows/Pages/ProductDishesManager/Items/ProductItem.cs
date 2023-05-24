using Microsoft.Win32;
using mvvm;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager.Items
{
    internal class ProductItem : NotifyPropertyChanged
    {
        public int ID { get; private set; }

        private string picture;
        public string Picture
        {
            get => picture;
            set
            {
                picture = value;
                OnPropertyChanged();

                Changestats();
            }
        }

        private string productName;
        public string ProductName
        {
            get => productName;
            set
            { 
                productName = value;
                OnPropertyChanged();

                Changestats();
            }
        }

        private double productCount;
        public double ProductCount
        {
            get => productCount;
            set
            {
                productCount = value;
                OnPropertyChanged();

                Changestats();
            }
        }

        private Command getNewPicture;
        public Command GetNewPicture
        { 
            get => getNewPicture;
            set
            { 
                getNewPicture = value;
                OnPropertyChanged();
            }
        }

        private Command deleteProductCommand;
        public Command DeleteProductCommand
        {
            get => deleteProductCommand;
            set
            {
                deleteProductCommand = value;
                OnPropertyChanged();
            }
        }


        private async void Changestats() => await ProductsModel.UpdateProduct(this);

        public ProductItem(int ID, string ProductName, string Picture, double ProductCount)
        {
            this.ID = ID;
            this.productName = ProductName;
            this.picture = Picture;
            this.productCount = ProductCount;

            DeleteProductCommand = new Command
            (
                async (obj) => await ProductsModel.DeleteProductAsync(this)
            );

            getNewPicture = new Command
            (
                (obj) => 
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png";
                    if (!(bool)dialog.ShowDialog()) return;
                    this.Picture = dialog.FileName;
                }
            );
        }
    }
}
