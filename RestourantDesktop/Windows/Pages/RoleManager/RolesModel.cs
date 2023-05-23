using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RestourantDesktop.Database;
using DependencyChecker;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal static class RolesModel
    {
        public static event EventHandler<EventArgs> RoleChanged;
        public static event EventHandler<EventArgs> RoleDeleted;
        public static event EventHandler<EventArgs> RoleAdded;

        public static event EventHandler<EventArgs> RightRoleChanged;
        public static event EventHandler<EventArgs> RightRoleDeleted;
        public static event EventHandler<EventArgs> RightRoleAdded;

        public static event EventHandler<EventArgs> PageChanged;
        public static event EventHandler<EventArgs> PageDeleted;
        public static event EventHandler<EventArgs> PageAdded;

        public static ObservableCollection<PageItem> PagesList { get; private set; }

        public static ObservableCollection<RoleItem> RoleList { get; private set; }

        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void PagesListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                item.TryGetValue("PageName", out object PageName);
                int ID = Convert.ToInt32(value);
                string Name = Convert.ToString(PageName);
                PageItem updatedElement = PagesList.FirstOrDefault(x => x.ID == ID);
                if (updatedElement == null)
                {
                    PageItem insertedItem = new PageItem(ID, Name);
                    PagesList.Add(insertedItem);
                    PageAdded?.Invoke(insertedItem, new EventArgs());
                }
                else
                {
                    updated.Add(ID);
                    updatedElement.pageName = Name;
                    updatedElement.OnPropertyChanged("pageName");
                    PageChanged?.Invoke(updatedElement, new EventArgs());
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                if (updated.Contains(ID)) continue;

                PageItem deletedItem = PagesList.FirstOrDefault(x => x.ID == ID);
                if (deletedItem == null) continue;
                PagesList.Remove(deletedItem);
                PageDeleted?.Invoke(deletedItem, new EventArgs());
            }
        }
        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void RoleListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<int> updated = new List<int>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                item.TryGetValue("RoleName", out value);
                string RoleName = Convert.ToString(value);
                RoleItem updatedElement = RoleList.FirstOrDefault(x => x.roleID == ID);
                if (updatedElement == null)
                {
                    List<PageRoleItem> rights = new List<PageRoleItem>();
                    for (int i = 0; i < PagesList.Count; i++)
                        rights.Add(new PageRoleItem(PagesList[i].ID, ID, PagesList[i].PageName, false));
                    RoleItem newRole = new RoleItem(ID, RoleName, rights);

                    RoleList.Add(newRole);
                    RoleAdded?.Invoke(newRole, new EventArgs());
                    IEnumerable<(int, int ,bool)> cache = CachedPageRoleListItems.Where(x => x.Item1 == ID);
                    if (cache.Count() != 0)
                    {
                        foreach (var cachedItem in cache)
                        {
                            PageRoleItem pageRoleItem = newRole.Rights.FirstOrDefault(x => x.PageID == cachedItem.Item2);
                            pageRoleItem.isCan = cachedItem.Item3;
                            pageRoleItem.OnPropertyChanged("IsCan");
                            RightRoleChanged?.Invoke(pageRoleItem, new EventArgs());
                        }
                        for (int i = 0; i < CachedPageRoleListItems.Count; i++)
                        {
                            if (CachedPageRoleListItems[i].Item1 == ID)
                            {
                                CachedPageRoleListItems.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    updated.Add(ID);
                    updatedElement.roleName = RoleName;
                    updatedElement.OnPropertyChanged("RoleName");
                    RoleChanged?.Invoke(updatedElement, new EventArgs());
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("ID", out object value);
                int ID = Convert.ToInt32(value);
                if (updated.Contains(ID)) continue;

                RoleItem roleItem = RoleList.FirstOrDefault(x => x.roleID == ID);
                RoleList.Remove(roleItem);
                RoleDeleted?.Invoke(roleItem, new EventArgs());
            }
        }

        private static List<(int, int, bool)> CachedPageRoleListItems = new List<(int, int, bool)> ();
        /// <summary>
        /// Handling changes from the database
        /// </summary>
        /// <param name="sender">Listener object <see cref="DBchangeListener"/></param>
        /// <param name="e">Message data</param>
        private static void PageRoleListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            List<(int, int)> updatedPages = new List<(int, int)>();
            foreach (var item in e.Inserted)
            {
                item.TryGetValue("Role_ID", out object value);
                int RoleID = Convert.ToInt32(value);
                item.TryGetValue("Page_ID", out value);
                int PageID = Convert.ToInt32(value);

                item.TryGetValue("IsCan", out value);
                bool IsCan = Convert.ToInt32(value) == 1;

                RoleItem roleItem = RoleList.FirstOrDefault(x => x.roleID == RoleID);
                PageItem pageItem = PagesList.FirstOrDefault(x => x.ID == PageID);
                //The trigger for adding linking records can work out earlier than the Listener,
                //so we remove the received data in the cache if there is no such user yet
                if (roleItem == null || pageItem == null)
                {
                    CachedPageRoleListItems.Add((RoleID, PageID, IsCan));
                    continue;
                }
                PageRoleItem pageRoleItem = roleItem.Rights.FirstOrDefault(x => x.PageID == PageID);
                if (pageRoleItem == null)
                {
                    PageRoleItem newItem = new PageRoleItem(PageID, roleItem.roleID, pageItem.PageName, IsCan);
                    roleItem.Rights.Add(newItem);
                    RightRoleAdded?.Invoke(newItem, new EventArgs());
                }
                else
                {
                    updatedPages.Add((RoleID, PageID));
                    pageRoleItem.isCan = IsCan;
                    pageRoleItem.OnPropertyChanged("IsCan");
                    RightRoleChanged?.Invoke(pageRoleItem, new EventArgs());
                }
            }

            foreach (var item in e.Deleted)
            {
                item.TryGetValue("Role_ID", out object value);
                int RoleID = Convert.ToInt32(value);
                item.TryGetValue("Page_ID", out value);
                int PageID = Convert.ToInt32(value);
                if (updatedPages.Contains((RoleID, PageID))) continue;

                RoleItem roleItem = RoleList.FirstOrDefault(x => x.roleID == RoleID);
                if (roleItem == null) continue;
                PageRoleItem pageRoleItem = roleItem.Rights.FirstOrDefault(x => x.PageID == PageID);
                if (pageRoleItem == null) continue;

                roleItem.Rights.Remove(pageRoleItem);
                RightRoleDeleted?.Invoke(pageRoleItem, new EventArgs());
            }
        }
        static RolesModel()
        {
            PageChanged += (sender, e)=>
            {
                PageItem item = sender as PageItem;
                for (int i = 0; i < RoleList.Count; i++)
                {
                    PageRoleItem roleitem = RoleList[i].Rights.FirstOrDefault(x => x.PageID == item.ID);
                    roleitem.pageName = item.pageName;
                    roleitem.OnPropertyChanged("PageName");
                }
            };

            Dependency.RoleRightChangedEvent += (sender, e) => PageRoleListChanged(sender, e);
            Dependency.RoleChangedEvent += (sender, e) => RoleListChanged(sender, e);
            Dependency.PagesChangedEvent += (sender, e) => PagesListChanged(sender, e);
        }

        public static async Task InitModel()
        {
            if (PagesList == null)
                await GetPagesListAsync();

            if (RoleList == null)
                await GetRolesList();
        }

        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <returns></returns>
        private static async Task GetRolesList()
        {
            try
            {
                RoleList = new ObservableCollection<RoleItem>();

                DataTable roles = new DataTable();
                DataTable rolesPages = new DataTable();
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetRolesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(roles);
                    }
                });
                await Task.Run(() =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetRolesPagesList", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                    {
                        adapter.Fill(rolesPages);
                    }
                });

                for (int i = 0; i < roles.Rows.Count; i++)
                {
                    int ID = Convert.ToInt32(roles.Rows[i]["ID"]);
                    string roleName = roles.Rows[i]["RoleName"].ToString();

                    DataRow[] pageRoles = rolesPages.Select($"Role_ID = {ID}");
                    List<PageRoleItem> rights = new List<PageRoleItem>();
                    for (int j = 0; j < pageRoles.Length; j++)
                    {
                        int CurrentPageID = Convert.ToInt32(pageRoles[j]["Page_ID"]);
                        PageItem page = PagesList.FirstOrDefault(x => x.ID == CurrentPageID);
                        rights.Add(new PageRoleItem(CurrentPageID, ID, page.PageName, Convert.ToBoolean(pageRoles[j]["IsCan"])));
                    }

                    RoleList.Add(new RoleItem(ID, roleName, rights));
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        /// <summary>
        /// Получение списка страниц
        /// </summary>
        /// <param name="deleteAction"></param>
        /// <returns></returns>
        private static async Task GetPagesListAsync()
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
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        /// <summary>
        /// Создаём новую страницу (право на доступ к ней)
        /// </summary>
        /// <param name="deleteAction">Действие для удаления страницы</param>
        /// <returns></returns>
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
        }

        /// <summary>
        /// Обновление имени страницы
        /// </summary>
        /// <param name="item">Страница с обновлённым именем</param>
        /// <returns></returns>
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
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        /// <summary>
        /// Удаление страницы из бд (права на доступ к ней у всех)
        /// </summary>
        /// <param name="item">Страница</param>
        /// <returns></returns>
        public async static Task DeletePageAsync(PageItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeletePage", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.ID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        /// <summary>
        /// Изменить статус права у пользователя на доступ к странице
        /// </summary>
        /// <param name="item">обхект представляющий право на доступ</param>
        /// <returns></returns>
        public async static Task ChangeRight(PageRoleItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeRight", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@rid", item.RoleID));
                        command.Parameters.Add(new SqlParameter("@pid", item.PageID));
                        command.Parameters.Add(new SqlParameter("@can", item.IsCan));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        /// <summary>
        /// Изменить название роли
        /// </summary>
        /// <param name="role">Изменяемая роль</param>
        /// <returns></returns>
        public async static Task ChangeRoleName(RoleItem role)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ChangeRoleName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", role.roleID));
                        command.Parameters.Add(new SqlParameter("@roleName", role.RoleName));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }
        }

        /// <summary>
        /// Удаление страницы из бд (права на доступ к ней у всех)
        /// </summary>
        /// <param name="item">Страница</param>
        /// <returns></returns>
        public async static Task DeleteRoleAsync(RoleItem item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("DeleteRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@id", item.roleID));

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }

        /// <summary>
        /// Создание новой роли
        /// </summary>
        /// <returns></returns>
        public async static Task CreateNewEmptyRole()
        {
            int newIndex = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("AddNewRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        newIndex = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ return; }
        }
    }
}