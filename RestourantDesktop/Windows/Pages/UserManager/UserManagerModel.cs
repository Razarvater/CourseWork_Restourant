using RestourantDesktop.Windows.Pages.UserManager.Items;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using RestourantDesktop.Windows.Pages.RoleManager;
using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System.Runtime.CompilerServices;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal static class UserManagerModel
    {
        public static event EventHandler<EventArgs> PositionChanged;
        public static event EventHandler<EventArgs> PositionDeleted;
        public static event EventHandler<EventArgs> PositionAdded;

        public static ObservableCollection<PositionItem> PositionsList { get; private set; }
        public static ObservableCollection<UserItem> UsersList { get; private set; }

        public static async Task InitModel()
        {
            await RolesModel.InitModel();

            RolesModel.RoleAdded += (sender,e) =>
            {
                RoleItem item = sender as RoleItem;
                for (int i = 0; i < UsersList.Count; i++)
                    UsersList[i].Roles.Add(new UserRoleItem(UsersList[i].UserID, item.roleID, item.RoleName, false));
            };
            RolesModel.RoleDeleted += (sender, e) =>
            {
                RoleItem item = sender as RoleItem;
                for (int i = 0; i < UsersList.Count; i++)
                    UsersList[i].Roles.Remove(UsersList[i].Roles.FirstOrDefault(x => x.RoleID == item.roleID));
            };
            RolesModel.RoleChanged += (sender, e) =>
            {
                RoleItem item = sender as RoleItem;
                for (int i = 0; i < UsersList.Count; i++)
                    UsersList[i].Roles.FirstOrDefault(x => x.RoleID == item.roleID).RoleName = item.RoleName;
            };

            PositionAdded += (sender, e) =>
            {
                for (int i = 0; i < UsersList.Count; i++)
                {
                    UsersList[i].OnPropertyChanged("Positions");
                    UsersList[i].OnPropertyChanged("SelectedPosItem");
                }
            };
            PositionDeleted += (sender, e) =>
            {
                PositionItem item = sender as PositionItem;
                //Удалим всех пользователей с этой ролью
                for (int i = 0; i < UsersList.Count; i++)
                {
                    if (UsersList[i].SelectedPosItem.ID == item.ID)
                    {
                        UsersList.Remove(UsersList[i]);
                        i--;
                    }
                }
                PositionsList.Remove(item);
                for (int i = 0; i < UsersList.Count; i++)
                {
                    UsersList[i].OnPropertyChanged("Positions");
                    UsersList[i].OnPropertyChanged("SelectedPosItem");
                }
            };
            PositionChanged += (sender, e) =>
            {
                for (int i = 0; i < UsersList.Count; i++)
                {
                    UsersList[i].OnPropertyChanged("Positions");
                    UsersList[i].OnPropertyChanged("SelectedPosItem");
                }
            };

            UsersList = new ObservableCollection<UserItem>();
            DataTable users = new DataTable();
            DataTable UserRoles = new DataTable();
            await Task.Run(() =>
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetEmployeeUsersList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.Fill(users);
                }
            });

            await Task.Run(() =>
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetUserRolesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.Fill(UserRoles);
                }
            });

            PositionsList = new ObservableCollection<PositionItem>();
            DataTable positions = new DataTable();

            await Task.Run(() =>
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetPositionList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.Fill(positions);
                }
            });

            for (int i = 0; i < positions.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(positions.Rows[i]["ID"]);
                string roleName = positions.Rows[i]["Name"].ToString();
                double Salary = Convert.ToDouble(positions.Rows[i]["Salary"]);

                PositionsList.Add(new PositionItem(ID, roleName, Salary));
            }

            //TODO: список ролей пользователя
            for (int i = 0; i < users.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(users.Rows[i]["ID"]);
                string login = users.Rows[i]["Login"].ToString();
                string passport = users.Rows[i]["Passport"].ToString();
                string FullName = users.Rows[i]["FullName"].ToString();
                string PhoneNumber = users.Rows[i]["PhoneNum"].ToString();
                string PositionID = users.Rows[i]["Position_ID"].ToString();
                PositionItem pos = PositionsList.FirstOrDefault(x => x.ID == Convert.ToInt32(PositionID));

                DataRow[] roles = UserRoles.Select($"User_ID={ID}");
                List<UserRoleItem> UserRolesList = new List<UserRoleItem>();
                for (int j = 0; j < roles.Length; j++)
                {
                    RoleItem item = RolesModel.RoleList.FirstOrDefault(x => x.roleID == Convert.ToInt32(roles[j]["Role_ID"]));
                   
                    UserRolesList.Add(new UserRoleItem(ID, item.roleID, item == null ? "" : item.RoleName, Convert.ToBoolean(roles[j]["IsCan"])));
                }

                UsersList.Add(new UserItem(ID, login, passport, FullName, PhoneNumber, pos, UserRolesList));
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

            PositionChanged?.Invoke(item, new EventArgs());
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

            PositionDeleted?.Invoke(item, new EventArgs());
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

            PositionItem newitem = new PositionItem(newIndex, "", 0);
            PositionsList.Add(newitem);
            PositionAdded?.Invoke(newitem, new EventArgs());
        }

        public static async Task ChangeUserStatsAsync(UserItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeUserStats", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@userid", item.UserID));
                        command.Parameters.Add(new SqlParameter("@login", item.Login));
                        command.Parameters.Add(new SqlParameter("@fullName", item.FullName));
                        command.Parameters.Add(new SqlParameter("@passport", item.Passport));
                        command.Parameters.Add(new SqlParameter("@phoneNum", item.PhoneNum));
                        command.Parameters.Add(new SqlParameter("@posID", item.SelectedPosItem.ID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
        
        public static async Task DeleteUserAsync(UserItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@userid", item.UserID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }

            UsersList.Remove(item);
        }
        
        public static async Task ChangeUserRole(UserRoleItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeUserRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@userid", item.UserID));
                        command.Parameters.Add(new SqlParameter("@roleid", item.RoleID));
                        command.Parameters.Add(new SqlParameter("@isCan", item.IsCan));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        public static async Task AddNewEmptyUserAsync()
        { 
            //TODO: это отдельной формой которая будет плавно вылезать снизу
        }
    }
}