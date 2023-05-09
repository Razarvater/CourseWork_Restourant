using RestourantDesktop.Windows.Pages.UserManager.Items;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal static class UserManagerModel
    {
        public static ObservableCollection<PositionItem> PositionsList { get; private set; }
        public static ObservableCollection<UserItem> UsersList { get; private set; }

        public static async Task InitModel()
        {
            PositionsList = new ObservableCollection<PositionItem>();
            DataTable positions = new DataTable();
            //DataTable UserPositions = new DataTable();
            await Task.Run(() =>
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetPositionList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.Fill(positions);
                }
            });

            UsersList = new ObservableCollection<UserItem>();
            DataTable users = new DataTable();
            await Task.Run(() =>
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetEmployeeUsersList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.Fill(users);
                }
            });

            //TODO: список ролей пользователя + список должностей пользователя так же добавить UserPositions TABLE
            for (int i = 0; i < users.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(users.Rows[i]["ID"]);
                string login = users.Rows[i]["Login"].ToString();
                string passport = users.Rows[i]["Passport"].ToString();
                string FullName = users.Rows[i]["FullName"].ToString();
                string PhoneNumber = users.Rows[i]["PhoneNum"].ToString();
                //string PositionID = users.Rows[i]["Position_ID"].ToString();

                UsersList.Add(new UserItem(ID, login, passport, FullName, PhoneNumber, new List<UserRoleItem>()));
            }


            for (int i = 0; i < positions.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(positions.Rows[i]["ID"]);
                string roleName = positions.Rows[i]["Name"].ToString();
                double Salary = Convert.ToDouble(positions.Rows[i]["Salary"]);

                PositionsList.Add(new PositionItem(ID, roleName, Salary));
            }
        }


        public static async Task UpdatePositionAsync(PositionItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangePositionStats", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));
                        command.Parameters.Add(new SqlParameter("@PositionName", item.PositionName));
                        command.Parameters.Add(new SqlParameter("@Salary", item.Salary));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }

            for (int i = 0; i < UsersList.Count; i++)
            {
                UsersList[i].OnPropertyChanged("Positions");
                UsersList[i].OnPropertyChanged("SelectedPosItem");
            }
        }
        
        public static async Task DeletePositionAsync(PositionItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeletePosition", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }

            PositionsList.Remove(item);

            //Удалим данную страницу у всех отображённых прав
            //for (int i = 0; i < RoleList.Count; i++)
            //    RoleList[i].Rights.Remove(RoleList[i].Rights.FirstOrDefault(x => x.PageID == item.ID));
        }
        
        public static async Task AddNewEmptyPositionAsync()
        {
            int newIndex = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("AddNewPosition", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        newIndex = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }

            PositionsList.Add(new PositionItem(newIndex, "", 0));

            //Добавим Должность для всех отображённых пользователей
            //for (int i = 0; i < RoleList.Count; i++)
            //    RoleList[i].Rights.Add(new PageRoleItem(newIndex, "", false));
        }

        public static async Task ChangeUserAsync(UserItem item)
        { 
        
        }
        
        public static async Task DeleteUserAsync(UserItem item)
        { 
        
        }
        
        public static async Task ChangeUserRole(UserRoleItem item)
        { 
        
        }
    }
}