using Microsoft.Win32;
using mvvm;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager.Items
{
    public class ProductItem : NotifyPropertyChanged
    {
        public int ID { get; private set; }

        public string picture;
        public string Picture
        {
            get => Environment.CurrentDirectory + picture;
            set
            {
                picture = value;
                OnPropertyChanged();

                Changestats();
            }
        }

        public string Name { get => productName; }

        public string productName;
        public string ProductName
        {
            get => productName;
            set
            { 
                productName = value;
                OnPropertyChanged();
                OnPropertyChanged("Name");

                Changestats();
            }
        }

        public double productCount;
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
                    dialog.Filter = "Images (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                    if (!(bool)dialog.ShowDialog()) return;
                    string newName = $"/Images/product_{ID}_{Guid.NewGuid()}.{dialog.FileName.Split('.').Last()}";
                    try
                    {
                        if (File.Exists(Picture))
                            File.Delete(Picture);
                    }
                    catch (Exception) { }

                    File.Copy(dialog.FileName, Environment.CurrentDirectory + newName);

                    this.Picture = newName;
                    if (File.Exists(Environment.CurrentDirectory + newName))
                    {
                        new Thread(() =>
                        {
                            int time = 0;
                            while (true)
                            {
                                try
                                {
                                    //Ждём до 50 попыток, дабы если файл открыт ещё где-то, не пытаться сделать это вечно 
                                    if (time > 50) return;
                                    File.Delete(Environment.CurrentDirectory + newName);
                                    break;
                                }
                                catch (Exception) { Thread.Sleep(1000); time += 1; }
                            }
                        }).Start();
                    }
                }
            );
        }

        public override string ToString() => productName;
    }
}
