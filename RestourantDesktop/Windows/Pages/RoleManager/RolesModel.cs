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
using System.Runtime.Remoting.Channels;
using DependencyChecker;
using System.Windows;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal static class RolesModel
    {
        public static event EventHandler<EventArgs> RoleChanged;
        public static event EventHandler<EventArgs> RoleDeleted;
        public static event EventHandler<EventArgs> RoleAdded;

        public static event EventHandler<EventArgs> PageChanged;
        public static event EventHandler<EventArgs> PageDeleted;
        public static event EventHandler<EventArgs> PageAdded;

        public static ObservableCollection<PageItem> PagesList { get; private set; }

        public static ObservableCollection<RoleItem> RoleList { get; private set; }

        private static void PagesListChanged(object sender, DBchangeListener.DbChangeEventArgs e)
        {
            //TODO: сюда переношу все Update и т.д. и так для всех таблиц
            MessageBox.Show("пашет?");
        }

        static RolesModel()
        {
            Dependency.manager.ListenTable("PagesList", PagesListChanged);

            PageAdded += (sender, e) =>
            {
                PageItem newItem = sender as PageItem;
                //Добавим страницу для всех отображённых ролей
                for (int i = 0; i < RoleList.Count; i++)
                    RoleList[i].Rights.Add(new PageRoleItem(newItem.ID, newItem.PageName, false));
            };
            PageChanged += (sender, e) => 
            {
                PageItem newItem = sender as PageItem;
                //Назначим для всех отображённых ролей
                for (int i = 0; i < RoleList.Count; i++)
                    RoleList[i].Rights.FirstOrDefault(x => x.PageID == newItem.ID).PageName = newItem.PageName;
            };
            PageDeleted += (sender, e) =>
            {
                PageItem newItem = sender as PageItem;
                //Удалим данную страницу у всех отображённых прав
                for (int i = 0; i < RoleList.Count; i++)
                    RoleList[i].Rights.Remove(RoleList[i].Rights.FirstOrDefault(x => x.PageID == newItem.ID));
            };
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
                        rights.Add(new PageRoleItem(CurrentPageID, page.PageName, Convert.ToBoolean(pageRoles[j]["IsCan"])));
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

            PageItem newItem = new PageItem(newIndex, "");
            PagesList.Add(newItem);
            PageAdded?.Invoke(newItem, new EventArgs());
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

            PageChanged?.Invoke(item, new EventArgs());
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

            PagesList.Remove(item);
            PageDeleted?.Invoke(item, new EventArgs());
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

            RoleChanged?.Invoke(role , new EventArgs());
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

            RoleList.Remove(item);
            RoleDeleted?.Invoke(item, new EventArgs());
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

            List<PageRoleItem> rights = new List<PageRoleItem>();
            for (int i = 0; i < PagesList.Count; i++)
                rights.Add(new PageRoleItem(PagesList[i].ID, PagesList[i].PageName, false));
            RoleList.Add(new RoleItem(newIndex, "", rights));

            RoleAdded?.Invoke(RoleList.Last(), new EventArgs());
        }
    }
}