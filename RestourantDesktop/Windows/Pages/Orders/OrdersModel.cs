using RestourantDesktop.Windows.Pages.ProductDishesManager.Items;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;

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
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetOpenedOrderList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(OrdersTable);
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

                    OpenedOrderItem item = new OpenedOrderItem(ID, EmpID, CreateDatetime, tableinfo, CookingTime, Sum);
                    OrderList.Add(item);
                }
            }
            catch (Exception) {/*TODO: Сообщение об ошибке*/ }
        }
    }
}
