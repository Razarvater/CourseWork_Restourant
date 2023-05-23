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

    }
}
