using DependencyChecker;
using RestourantDesktop.Database;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal static class ProductsModel
    {
        public static ObservableCollection<ProductItem> products = new ObservableCollection<ProductItem>();

        public static event EventHandler<ProductItem> ProductChanged;
        public static event EventHandler<ProductItem> ProductDeleted;
        public static event EventHandler<ProductItem> ProductAdded;

        public static void productListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted) 
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                item.TryGetValue("Name", out value);
                string productName = value.ToString();
                item.TryGetValue("ProductPicture", out value);
                string productPicture = value.ToString();
                item.TryGetValue("CountInStock", out value);
                double CountInStock = Convert.ToDouble(value.ToString().Replace('.', ','));

                ProductItem updatedItem = products.FirstOrDefault(x => x.ID == ID);
                if (updatedItem == null)
                {
                    ProductItem newItem = new ProductItem(ID, productName, productPicture, CountInStock);
                    products.Add(newItem);
                    ProductAdded?.Invoke(null, newItem);
                }
                else
                {
                    updated.Add(ID);
                    updatedItem.productName = productName;
                    updatedItem.picture = productPicture;
                    updatedItem.productCount = CountInStock;
                    updatedItem.OnPropertyChanged("productName");
                    updatedItem.OnPropertyChanged("picture");
                    updatedItem.OnPropertyChanged("productCount");

                    ProductChanged?.Invoke(null, updatedItem);
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                if (updated.Contains(ID)) continue;

                ProductItem deletedItem = products.FirstOrDefault(x => x.ID == ID);
                if (deletedItem == null) continue;

                products.Remove(deletedItem);
                ProductDeleted?.Invoke(null, deletedItem);
            }
        }

        public static async Task InitModel()
        {
            if (products.Count != 0) return;

            products = new ObservableCollection<ProductItem>();
            DataTable productTable = new DataTable();
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetProductList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(productTable);
                    }
                });

                for (int i = 0; i < productTable.Rows.Count; i++)
                {
                    int.TryParse(productTable.Rows[i]["ID"].ToString(), out int ID);
                    double.TryParse(productTable.Rows[i]["CountInStock"].ToString(), out double ProductCount);
                    string ProductName = productTable.Rows[i]["Name"].ToString();
                    string Picture = productTable.Rows[i]["ProductPicture"].ToString();

                    products.Add(new ProductItem(ID, ProductName, Picture, ProductCount));
                }
            }
            catch (Exception) {/*TODO: Сообщение об ошибке*/ }

            Dependency.manager.ListenTable("ProductsInStock", productListChanged);
        }

        public static async Task UpdateProduct(ProductItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        command.Parameters.Add(new SqlParameter("@productName", item.ProductName));
                        command.Parameters.Add(new SqlParameter("@picture", item.Picture));
                        command.Parameters.Add(new SqlParameter("@count", item.ProductCount));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        public static async Task CreateNewProductAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("CreateProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        public static async Task DeleteProductAsync(ProductItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeleteProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }
    }
}