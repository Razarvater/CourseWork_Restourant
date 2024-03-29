﻿using Microsoft.Win32;
using mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager.Items
{
    internal class DishItem : NotifyPropertyChanged
    {
        public class PictureDishItem : NotifyPropertyChanged
        {
            public string name;
            public string Adress
            {
                get => Environment.CurrentDirectory + name;
                set
                {
                    name = value;
                    OnPropertyChanged();
                }
            }

            private Command deletePicture;
            public Command DeletePicture
            {
                get => deletePicture;
                set
                {
                    deletePicture = value;
                    OnPropertyChanged();
                }
            }

            public PictureDishItem(string Name, Action<PictureDishItem> delete)
            {
                name = Name;
                deletePicture = new Command((obj) => 
                {
                    delete?.Invoke(this);
                });
            }

            public override string ToString() => name;
        }

        public int ID { get; private set; }

        public string name;
        public string Name
        { 
            get => name;
            set
            { 
                name = value;
                OnPropertyChanged();

                ChangeDish();
            }
        }

        public string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();

                ChangeDish();
            }
        }

        public ObservableCollection<PictureDishItem> pictures;
        public ObservableCollection<PictureDishItem> Pictures
        {
            get => pictures;
            set
            {
                pictures = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DishProductItem> products;
        public ObservableCollection<DishProductItem> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }

        public double cost;
        public double Cost
        {
            get => cost;
            set
            {
                cost = value;
                OnPropertyChanged();

                ChangeDish();
            }
        }

        public int cookingTime;
        public int CookingTime
        {
            get => cookingTime;
            set
            {
                cookingTime = value;
                OnPropertyChanged();

                ChangeDish();
            }
        }

        private Command deleteCommand;
        public Command DeleteCommand
        {
            get => deleteCommand;
            set
            { 
                deleteCommand = value;
                OnPropertyChanged();
            }
        }
        
        private Command addNewDishProductCommand;
        public Command AddNewDishProductCommand
        {
            get => addNewDishProductCommand;
            set
            {
                addNewDishProductCommand = value;
                OnPropertyChanged();
            }
        }
        
        private Command addImage;
        public Command AddImage
        {
            get => addImage;
            set
            {
                addImage = value;
                OnPropertyChanged();
            }
        }

        private async void ChangeDish() => await DishesModel.ChangeDishAsync(this);

        public DishItem(int ID, string name, string desc, string Pictures, int cookingTime, double cost)
        {
            this.ID = ID;
            this.name = name;
            this.description = desc;
            this.pictures = new ObservableCollection<PictureDishItem>();

            foreach (var item in Pictures.Split(';'))
            {
                if(!string.IsNullOrEmpty(item))
                    pictures.Add(new PictureDishItem(item, (obj) => 
                    { 
                        pictures.Remove(obj);
                        ChangeDish();
                    }));
            }
            this.cookingTime = cookingTime;
            this.cost = cost;

            DeleteCommand = new Command(async (obj) => await DishesModel.DeleteDishAsync(this));
            AddImage = new Command
            (
                (obj) =>
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "Images (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                    if (!(bool)dialog.ShowDialog()) return;
                    string newName = $"/Images/dish_{ID}_{Guid.NewGuid()}.{dialog.FileName.Split('.').Last()}";

                    File.Copy(dialog.FileName, Environment.CurrentDirectory + newName);
                    this.pictures.Add(new PictureDishItem(newName, (item) =>
                    {
                        this.Pictures.Remove(item);
                        if (File.Exists(item.Adress))
                            new Thread(() =>
                            {
                                int time = 0;
                                while (true)
                                {
                                    try
                                    {
                                        //Ждём до 50 попыток, дабы если файл открыт ещё где-то, не пытаться сделать это вечно 
                                        if (time > 50) return;
                                        File.Delete(item.Adress);
                                        break;
                                    }
                                    catch (Exception) { Thread.Sleep(1000); time += 1; }
                                }
                            }).Start();

                        ChangeDish();
                    }));


                    ChangeDish();
                }
            );

            AddNewDishProductCommand = new Command( async (obj) => await DishesModel.AddNewDishProductAsync(ID) );
            Products = new ObservableCollection<DishProductItem>();
        }

        public void SetNewPicture(string Pictures)
        {
            ObservableCollection<PictureDishItem> items = new ObservableCollection<PictureDishItem>();
            foreach (var item in Pictures.Split(';'))
            {
                if (!string.IsNullOrEmpty(item))
                    items.Add(new PictureDishItem(item, (obj) =>
                    {
                        items.Remove(obj);
                        ChangeDish();
                    }));
            }
            pictures = items;
            OnPropertyChanged("Pictures");
        }

        public override string ToString() => Name;
    }

    public class DishProductItem : NotifyPropertyChanged
    {
        public int ID { get; private set; }
        public int Dish_ID { get; private set; }

        public string Picture { get => SelectedProduct == null ? "" : SelectedProduct.Picture; }
        public ObservableCollection<ProductItem> ProductItems { get => ProductsModel.products; } 

        public ProductItem selectedProduct;
        public ProductItem SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged();
                OnPropertyChanged("Picture");

                ChangeDishProduct();
            }
        }

        public double count;
        public double Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();

                ChangeDishProduct();
            }
        }

        private Command deleteDishProductCommand;
        public Command DeleteDishProductCommand
        {
            get => deleteDishProductCommand;
            set
            {
                deleteDishProductCommand = value;
                OnPropertyChanged();
            }
        }

        private async void ChangeDishProduct() => await DishesModel.ChangeDishProductAsync(this);
        public DishProductItem(int ID,int Dish_ID, double Count, ProductItem selectedProduct)
        {
            this.ID = ID;
            this.count = Count;
            this.Dish_ID = Dish_ID;
            this.selectedProduct = selectedProduct;

            DeleteDishProductCommand = new Command(async (obj) => await DishesModel.DeleteDishProductAsync(ID));
        }
    }
}