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
            }
        }

        private int productCount;
        public int ProductCount
        {
            get => productCount;
            set
            {
                productCount = value;
                OnPropertyChanged();
            }
        }

        public ProductItem(int ID, string ProductName, string Picture, int ProductCount)
        {
            this.ID = ID;
            this.ProductName = ProductName;
            this.Picture = Picture;
            this.ProductCount = ProductCount;
        }
    }
}
