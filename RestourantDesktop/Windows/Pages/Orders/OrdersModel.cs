using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using RestourantDesktop.Windows.Pages.Orders.Items;
using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using RestourantDesktop.Windows.Pages.ProductDishesManager;
using System.Linq;
using System.Collections.Generic;

namespace RestourantDesktop.Windows.Pages.Orders
{
    internal static class OrdersModel
    {
        public static ObservableCollection<OpenedOrderItem> OrderList = new ObservableCollection<OpenedOrderItem>();



        public static async Task InitOrderList()
        {
            if (OrderList.Count != 0) return;


            OrderList = new ObservableCollection<OpenedOrderItem>();
            DataTable OrdersTable = new DataTable();
            DataTable DishOrderTable = new DataTable();
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetOpenedOrderList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(OrdersTable);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetOpenedDishOrderList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(DishOrderTable);
                    }
                });

                for (int i = 0; i < OrdersTable.Rows.Count; i++)
                {
                    int.TryParse(OrdersTable.Rows[i]["ID"].ToString(), out int ID);
                    int.TryParse(OrdersTable.Rows[i]["EmployeeUserID"].ToString(), out int EmpID);
                    DateTime.TryParse(OrdersTable.Rows[i]["CreateDateTime"].ToString(), out DateTime CreateDatetime);
                    string tableinfo = OrdersTable.Rows[i]["TableInfo"].ToString();
                    int.TryParse(OrdersTable.Rows[i]["CookingTime"].ToString(), out int CookingTime);
                    double.TryParse(OrdersTable.Rows[i]["Sum"].ToString(), out double Sum);

                    DataRow[] dishOrders = DishOrderTable.Select($"OrderID = {ID}");
                    ObservableCollection<OrderDishItem> items = new ObservableCollection<OrderDishItem>();
                    for (int j = 0; j < dishOrders.Length; j++)
                    {
                        int.TryParse(dishOrders[i]["DishID"].ToString(), out int dishID);
                        int.TryParse(dishOrders[i]["ProductCount"].ToString(), out int count);
                        DishItem dishItem = DishesModel.dishes.FirstOrDefault(x => x.ID == dishID);
                        OrderDishItem newItem = new OrderDishItem(dishItem, count);
                        items.Add(newItem);
                    }

                    OpenedOrderItem item = new OpenedOrderItem(ID, EmpID, CreateDatetime, tableinfo, CookingTime, Sum, items);
                    OrderList.Add(item);
                }
            }
            catch (Exception) {/*TODO: Сообщение об ошибке*/ }
        }

        public static async Task CloseOrderAsync(OpenedOrderItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("CloseOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@orderID", item.ID));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        public static async Task CreateOpenedOrder(IEnumerable<SelecteDish> list, int empID, string tableInfo, int cookingTime, double sum)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    Guid guid = Guid.NewGuid();
                    await connection.OpenAsync();
                    foreach (var item in list)
                    {
                        using (SqlCommand command = new SqlCommand("CreateOpenedOrderList", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@guid", guid));
                            command.Parameters.Add(new SqlParameter("@dishID", item.SelectedDish.ID));
                            command.Parameters.Add(new SqlParameter("@count", item.Count));
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    using (SqlCommand command = new SqlCommand("CreateOpenedOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@EmpID", empID));
                        command.Parameters.Add(new SqlParameter("@TableInfo", tableInfo));
                        command.Parameters.Add(new SqlParameter("@cookingTime",cookingTime ));
                        command.Parameters.Add(new SqlParameter("@sum", sum));
                        command.Parameters.Add(new SqlParameter("@guid", guid));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
    }
}
