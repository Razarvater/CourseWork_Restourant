using RestourantDesktop.Database;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using DependencyChecker;
using System.Xml.Linq;

namespace RestourantDesktop.Windows.Pages.ProductDishesManager
{
    internal static class DishesModel
    {
        public static ObservableCollection<DishItem> dishes = new ObservableCollection<DishItem>();

        public static event EventHandler<DishItem> DishChanged;
        public static event EventHandler<DishItem> DishDeleted;
        public static event EventHandler<DishItem> DishAdded;

        public static void DishListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        { 
            
        }

        public static async Task InitModel()
        {
            if (dishes.Count != 0) return;

            dishes = new ObservableCollection<DishItem>();
            DataTable productTable = new DataTable();
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetDishesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(productTable);
                    }
                });

                for (int i = 0; i < productTable.Rows.Count; i++)
                {
                    int.TryParse(productTable.Rows[i]["ID"].ToString(), out int ID);
                    string Name = productTable.Rows[i]["Name"].ToString();
                    string Desc = productTable.Rows[i]["Description"].ToString();
                    string Pictures = productTable.Rows[i]["Pictures"].ToString();
                    int.TryParse(productTable.Rows[i]["TimeCooking"].ToString(), out int cookTime);
                    double.TryParse(productTable.Rows[i]["Cost"].ToString().Replace('.',','), out double cost);
                    dishes.Add(new DishItem(ID, Name, Desc, Pictures, cookTime, cost));
                }
            }
            catch (Exception) {/*TODO: Сообщение об ошибке*/ }

            Dependency.manager.ListenTable("ProductsInStock", DishListChanged);
        }

        public static async Task ChangeDish(DishItem item)
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
    }
}