using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal static class ProductsModel
    {
        public static ObservableCollection<ProductItem> products = new ObservableCollection<ProductItem>();

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