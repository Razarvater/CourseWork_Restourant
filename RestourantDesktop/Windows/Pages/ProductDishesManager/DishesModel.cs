using RestourantDesktop.Database;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using DependencyChecker;
using System.Collections.Generic;
using System.Linq;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal static class DishesModel
    {
        public static ObservableCollection<DishItem> dishes = new ObservableCollection<DishItem>();

        public static event EventHandler<DishItem> DishChanged;
        public static event EventHandler<DishItem> DishDeleted;
        public static event EventHandler<DishItem> DishAdded;
        
        public static event EventHandler<DishProductItem> DishProductChanged;
        public static event EventHandler<DishProductItem> DishProductDeleted;
        public static event EventHandler<DishProductItem> DishProductAdded;

        public static void DishListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);

                item.TryGetValue("Name", out value);
                string Name = value.ToString();
                item.TryGetValue("Description", out value);
                string Desc = value.ToString();
                item.TryGetValue("Pictures", out value);
                string Pictures = value.ToString();
                
                item.TryGetValue("TimeCooking", out value);
                int TimeCooking = Convert.ToInt32(value);

                item.TryGetValue("Cost", out value);
                double.TryParse(value.ToString().Replace('.', ','), out double cost);

                DishItem updatedItem = dishes.FirstOrDefault(x => x.ID == ID);

                if (updatedItem == null)
                {
                    DishItem newItem = new DishItem(ID, Name, Desc, Pictures, TimeCooking, cost);
                    dishes.Add(newItem);
                    DishAdded?.Invoke(null, newItem);
                }
                else
                {
                    updated.Add(ID);
                    updatedItem.cost = cost;
                    updatedItem.SetNewPicture(Pictures);
                    updatedItem.cookingTime = TimeCooking;
                    updatedItem.name = Name;
                    updatedItem.description = Desc;
                    updatedItem.OnPropertyChanged("Name");
                    updatedItem.OnPropertyChanged("CookingTime");
                    updatedItem.OnPropertyChanged("Description");
                    updatedItem.OnPropertyChanged("Cost");

                    DishChanged?.Invoke(null, updatedItem);
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                if (updated.Contains(ID)) continue;

                DishItem deleted = dishes.FirstOrDefault(x => x.ID == ID);
                if (deleted == null) continue;

                dishes.Remove(deleted);
                DishDeleted?.Invoke(null, deleted);
            }
        }

        public static void DishProductListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<(int ID, int dishID)> updated = new List<(int ID, int dishID)>();

            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                
                item.TryGetValue("Dish_ID", out  value);
                int DishID = Convert.ToInt32(value);

                item.TryGetValue("Product_ID", out value);
                int ProductID = Convert.ToInt32(value == null ? "-1" : value);

                item.TryGetValue("Count", out value);
                double.TryParse(value.ToString().Replace('.', ','), out double count);

                DishItem dish = dishes.FirstOrDefault(x => x.ID == DishID);
                if (dish == null) continue;

                ProductItem product = ProductsModel.products.FirstOrDefault(x => x.ID == ProductID);

                DishProductItem updatedItem = dish.Products.FirstOrDefault(x => x.ID == ID);
                if (updatedItem == null)
                {
                    DishProductItem newItem = new DishProductItem(ID, DishID, count, product);
                    dish.products.Add(newItem);
                    DishProductAdded?.Invoke(null, newItem);
                }
                else
                {
                    updated.Add((ID, DishID));
                    updatedItem.selectedProduct = product;
                    updatedItem.count = count;
                    updatedItem.OnPropertyChanged(nameof(updatedItem.Count));
                    updatedItem.OnPropertyChanged(nameof(updatedItem.SelectedProduct));
                    updatedItem.OnPropertyChanged(nameof(updatedItem.Picture));

                    DishProductChanged?.Invoke(null, updatedItem);
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);

                item.TryGetValue("Dish_ID", out value);
                int DishID = Convert.ToInt32(value);

                if (updated.Contains((ID, DishID))) continue;

                DishItem dish = dishes.FirstOrDefault(x => x.ID == DishID);
                if (dish == null) continue;

                DishProductItem deletedItem = dish.Products.FirstOrDefault(x => x.ID == ID);
                if (deletedItem == null) continue;

                dish.products.Remove(deletedItem);

                DishProductDeleted?.Invoke(null, deletedItem);
            }
        }

        private static bool IsInited = false;
        public static async Task InitModel()
        {
            if (IsInited) return;
            IsInited = true;

            dishes = new ObservableCollection<DishItem>();
            DataTable DishesTable = new DataTable();
            DataTable DishesProductTable = new DataTable();
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetDishesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(DishesTable);
                    }
                });

                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetDishesProductList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(DishesProductTable);
                    }
                });

                for (int i = 0; i < DishesTable.Rows.Count; i++)
                {
                    int.TryParse(DishesTable.Rows[i]["ID"].ToString(), out int ID);
                    string Name = DishesTable.Rows[i]["Name"].ToString();
                    string Desc = DishesTable.Rows[i]["Description"].ToString();
                    string Pictures = DishesTable.Rows[i]["Pictures"].ToString();
                    int.TryParse(DishesTable.Rows[i]["TimeCooking"].ToString(), out int cookTime);
                    double.TryParse(DishesTable.Rows[i]["Cost"].ToString().Replace('.',','), out double cost);

                    DishItem added = new DishItem(ID, Name, Desc, Pictures, cookTime, cost);
                    dishes.Add(added);

                    DataRow[] products = DishesProductTable.Select($"Dish_ID = {ID}");
                    foreach (DataRow row in products) 
                    {
                        int ProductID = -1;
                        if (!(row["Product_ID"] is DBNull))
                            ProductID = Convert.ToInt32(row["Product_ID"]);

                        int link_ID = Convert.ToInt32(row["ID"]);
                        double Count = Convert.ToInt32(row["Count"]);
                        ProductItem product = ProductsModel.products.FirstOrDefault(x=>x.ID == ProductID);
                        
                        DishProductItem newItem = new DishProductItem(link_ID, ID, Count, product);

                        added.Products.Add(newItem);
                    }

                }
            }
            catch (Exception) {/*TODO: Сообщение об ошибке*/ }

            ProductsModel.ProductChanged += (obj, e) =>
            {
                foreach (DishItem item in dishes)
                {
                    foreach(DishProductItem row in item.Products) 
                    {
                        if (row.SelectedProduct == null || row.selectedProduct.ID != e.ID) continue;

                        var temp = row.SelectedProduct;

                        //Костыль, что-бы сменялось имя
                        row.selectedProduct = null;
                        row.OnPropertyChanged(nameof(row.SelectedProduct));
                        row.selectedProduct = temp;
                        row.OnPropertyChanged(nameof(row.SelectedProduct));

                        row.OnPropertyChanged(nameof(row.Picture));
                    }
                }
            };

            Dependency.manager.ListenTable("Dishes", DishListChanged);
            Dependency.manager.ListenTable("ProductForDishes", DishProductListChanged);
        }

        public static async Task ChangeDishAsync(DishItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeDish", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        command.Parameters.Add(new SqlParameter("@Name", item.Name));
                        command.Parameters.Add(new SqlParameter("@cost", item.cost));
                        command.Parameters.Add(new SqlParameter("@time", item.cookingTime));
                        command.Parameters.Add(new SqlParameter("@description", item.description));

                        string Pictures = string.Empty;
                        foreach (var picture in item.pictures)
                            Pictures += picture + ";";

                        command.Parameters.Add(new SqlParameter("@pictures", Pictures));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        public static async Task CreateEmptyDishAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("CreateDish", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        public static async Task DeleteDishAsync(DishItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeleteDish", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        public static async Task AddNewDishProductAsync(int DishID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("CreateDishesProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Dish_ID", DishID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
        
        public static async Task DeleteDishProductAsync(int ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeleteDishProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@ID", ID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
        
        public static async Task ChangeDishProductAsync(DishProductItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeDishesProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@ID", item.ID));
                        command.Parameters.Add(new SqlParameter("@count", item.count));
                        command.Parameters.Add(new SqlParameter("@Product_ID", item.selectedProduct == null ? null : (int?)item.SelectedProduct.ID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
    }
}