using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal static class RolesModel
    {
        public static ObservableCollection<PageItem> PagesList { get; private set; }

        public static async Task GetPagesListAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetPagesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        DataTable dt = new DataTable();
                        PagesList = new ObservableCollection<PageItem>();
                        adapter.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            PagesList.Add(new PageItem(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["PageName"].ToString()));
                    }
                });
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/}
        }

        public static async Task CreateNewEmptyPageAsync()
        {
            int newIndex = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("AddNewPage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        newIndex = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }

            PagesList.Add(new PageItem(newIndex,""));
        }

        public static async Task UpdatePageAsync(PageItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("UpdatePage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        command.Parameters.Add(new SqlParameter("@NewName", item.PageName));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }
    }
}
