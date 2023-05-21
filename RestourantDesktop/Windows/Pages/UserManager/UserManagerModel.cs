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
using RestourantDesktop.Database;
using DependencyChecker;
using RestourantDesktop.UserController;

namespace RestourantDesktop.Windows.Pages.UserManager
{
    internal static class UserManagerModel
    {
        public static event EventHandler<EventArgs> PositionDeleted;
        public static event EventHandler<EventArgs> PositionChanged;
        public static event EventHandler<EventArgs> PositionAdded;

        public static event EventHandler<EventArgs> UserDeleted;
        public static event EventHandler<EventArgs> UserChanged;
        public static event EventHandler<EventArgs> UserAdded;

        public static event EventHandler<EventArgs> UserRoleAdded;
        public static event EventHandler<EventArgs> UserRoleChanged;
        public static event EventHandler<EventArgs> UserRoleDeleted;

        public static ObservableCollection<PositionItem> PositionsList { get; private set; }
        public static ObservableCollection<UserItem> UsersList { get; private set; }

        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void PositionsListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                
                item.TryGetValue("Name", out value);
                string PosName = Convert.ToString(value);

                item.TryGetValue("Salary", out value);
                double Salary = Convert.ToDouble(value.ToString().Replace('.', ','));
                
                PositionItem UpdatedItem = PositionsList.FirstOrDefault(x => x.ID == ID);
                if (UpdatedItem != null)
                {
                    updated.Add(ID);
                    UpdatedItem.salary = Salary;
                    UpdatedItem.positionName = PosName;
                    UpdatedItem.OnPropertyChanged("Salary");
                    UpdatedItem.OnPropertyChanged("PositionName");
                    PositionChanged?.Invoke(UpdatedItem, new EventArgs());
                }
                else
                {
                    PositionItem NewPosition = new PositionItem(ID, PosName, Salary);
                    PositionsList.Add(NewPosition);
                    PositionAdded?.Invoke(NewPosition, new EventArgs());
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);

                if (updated.Contains(ID)) continue;

                PositionItem DeletedItem = PositionsList.FirstOrDefault(x => x.ID == ID);
                PositionsList.Remove(DeletedItem);
                PositionDeleted?.Invoke(DeletedItem, new EventArgs());
            }
        }
        
        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void UserListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                int UserID = Convert.ToInt32(value);
                item.TryGetValue("Login", out value);
                string Login = Convert.ToString(value);
                item.TryGetValue("FullName", out value);
                string FullName = Convert.ToString(value);
                item.TryGetValue("Passport", out value);
                string Passport = Convert.ToString(value);
                item.TryGetValue("PhoneNum", out value);
                string PhoneNum = Convert.ToString(value);
                item.TryGetValue("Position_ID", out value);
                int PositionID = Convert.ToInt32(value);
                PositionItem PosItem = PositionsList.FirstOrDefault(x => x.ID == PositionID);

                UserItem updatedItem = UsersList.FirstOrDefault(x => x.UserID == UserID);
                if (updatedItem == null)
                {
                    List<UserRoleItem> Roles = new List<UserRoleItem>();
                    for (int i = 0; i < RolesModel.RoleList.Count; i++)
                        Roles.Add(new UserRoleItem(UserID, RolesModel.RoleList[i].roleID, RolesModel.RoleList[i].RoleName, false));
                    
                    UserItem newItem = new UserItem(UserID, Login, Passport, FullName, PhoneNum, PosItem, Roles);
                    UserAdded?.Invoke(newItem, new EventArgs());

                    IEnumerable<(int, int, bool)> cache = CachedUserRolesListItems.Where(x => x.Item1 == UserID);
                    if (cache.Count() != 0)
                    {
                        foreach (var cachedItem in cache)
                        {
                            UserRoleItem userRoleItem = newItem.Roles.FirstOrDefault(x => x.RoleID == cachedItem.Item2);
                            userRoleItem.isCan = cachedItem.Item3;
                            userRoleItem.OnPropertyChanged("IsCan");
                            UserRoleChanged?.Invoke(userRoleItem, new EventArgs());
                        }
                        for (int i = 0; i < CachedUserRolesListItems.Count; i++)
                        {
                            if (CachedUserRolesListItems[i].Item1 == UserID)
                            {
                                CachedUserRolesListItems.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    updated.Add(UserID);
                    updatedItem.login = Login;
                    updatedItem.fullName = FullName;
                    updatedItem.passport = Passport;
                    updatedItem.phoneNum = PhoneNum;
                    updatedItem.selectedPosItem = PosItem;
                    updatedItem.OnPropertyChanged("Login");
                    updatedItem.OnPropertyChanged("FullName");
                    updatedItem.OnPropertyChanged("Passport");
                    updatedItem.OnPropertyChanged("PhoneNum");
                    updatedItem.OnPropertyChanged("SelectedPosItem");
                    
                    UserChanged?.Invoke(updated, new EventArgs());
                }
            }
            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int UserID = Convert.ToInt32(value);
                if (updated.Contains(UserID)) continue;

                UserItem deletedItem = UsersList.FirstOrDefault(x=>x.UserID == UserID);
                UsersList.Remove(deletedItem);
                UserDeleted?.Invoke(deletedItem, new EventArgs());
            }
        }

        private static List<(int, int, bool)> CachedUserRolesListItems = new List<(int, int, bool)>();
        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void UserRolesChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<(int, int)> updated = new List<(int, int)>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("User_ID", out object value);
                int UserID = Convert.ToInt32(value);
                item.TryGetValue("Role_ID", out value);
                int RoleID = Convert.ToInt32(value);
                item.TryGetValue("IsCan", out value);
                bool IsCan = Convert.ToInt32(value) == 1;

                UserItem userItem = UsersList.FirstOrDefault(x => x.UserID == UserID);
                if (userItem == null)
                {
                    CachedUserRolesListItems.Add((UserID, RoleID, IsCan));
                    continue;
                }

                UserRoleItem UserRoleItem = userItem.Roles.FirstOrDefault(x => x.RoleID == RoleID);

                if (UserRoleItem == null)
                {
                    UserRoleItem NewItem = new UserRoleItem(UserID, RoleID, RolesModel.RoleList.FirstOrDefault(x => x.roleID == RoleID).RoleName ,IsCan);
                    userItem.Roles.Add(NewItem);
                    UserRoleAdded?.Invoke(NewItem, new EventArgs());
                }
                else
                {
                    updated.Add((UserID, RoleID));
                    UserRoleItem.isCan = IsCan;
                    UserRoleItem.OnPropertyChanged("IsCan");
                    UserRoleChanged?.Invoke(UserRoleItem, new EventArgs());
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("User_ID", out object value);
                int UserID = Convert.ToInt32(value);
                item.TryGetValue("Role_ID", out value);
                int RoleID = Convert.ToInt32(value);

                if (updated.Contains((UserID, RoleID))) continue;
                UserItem userItem = UsersList.FirstOrDefault(x => x.UserID == UserID);
                if (userItem == null) continue;
                UserRoleItem DeletedItem = userItem.Roles.FirstOrDefault(x => x.RoleID == RoleID);
                if (DeletedItem == null) continue;

                userItem.Roles.Remove(DeletedItem);
                UserRoleDeleted?.Invoke(DeletedItem,new EventArgs());
            }
        }

        public static async Task InitModelAsync()
        {
            if (UsersList != null) return;
            await RolesModel.InitModel();

            RolesModel.RoleChanged += (sender, e) =>
            {
                RoleItem ChangedRole = sender as RoleItem;
                for (int i = 0; i < UsersList.Count; i++)
                {
                    UserRoleItem Changed = UsersList[i].Roles.FirstOrDefault(x=>x.RoleID == ChangedRole.roleID);
                    Changed.RoleName = ChangedRole.roleName;
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
                UsersList.Add(new UserItem(ID, login, passport, FullName, PhoneNumber, pos,UserRolesList));
            }

            Dependency.manager.ListenTable("Positions", PositionsListChanged);

            Dependency.EmployeeUsersChangedEvent += (sender, e) => UserListChanged(sender, e);
            Dependency.UserRolesChangedEvent += (sender, e) => UserRolesChanged(sender, e);
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
        
        public static async Task ChangeUserRoleAsync(UserRoleItem item)
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

        public static async Task AddNewUserAsync(string login, string fullName, string passport, string phoneNumber, PositionItem selectedPos)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("CreateEmployeeUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@login", login));
                        command.Parameters.Add(new SqlParameter("@fullName", fullName));
                        command.Parameters.Add(new SqlParameter("@passport", passport));
                        command.Parameters.Add(new SqlParameter("@phoneNum", phoneNumber));
                        command.Parameters.Add(new SqlParameter("@posID", selectedPos.ID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
        
        public static async Task ChangeUserPasswordAsync(UserItem item)
        {
            try
            {
                (string HashedPassword, string salt) data = await UserController.UserController.CalculateNewPassword(item.Password);

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeUserPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@userID", item.UserID));
                        command.Parameters.Add(new SqlParameter("@password", data.HashedPassword));
                        command.Parameters.Add(new SqlParameter("@salt", data.salt));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO: Сообщение об ошибке*/ return; }
        }
    }
}