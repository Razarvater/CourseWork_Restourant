﻿using RestourantDesktop.Windows.Pages.RoleManager.Items;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RestourantDesktop.Windows.Pages.RoleManager
{
    internal static class RolesModel
    {
        public static ObservableCollection<PageItem> PagesList { get; private set; }

        private static ObservableCollection<RoleItem> roleList;
        public static ObservableCollection<RoleItem> RoleList 
        {
            get
            {
                if (roleList == null)
                    GetRolesList();

                return roleList;
            }
            private set { roleList = value; }
        }

        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <returns></returns>
        public static async Task GetRolesList()
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
            catch (Exception) { /*TODO Сообщение об ошибке*/}
        }

        /// <summary>
        /// Получение списка страниц
        /// </summary>
        /// <param name="deleteAction"></param>
        /// <returns></returns>
        public static async Task GetPagesListAsync(Func<PageItem, Task> deleteAction)
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
                            PagesList.Add(new PageItem(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["PageName"].ToString(), deleteAction));
                    }
                });
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/}
        }

        /// <summary>
        /// Создаём новую страницу (право на доступ к ней)
        /// </summary>
        /// <param name="deleteAction">Действие для удаления страницы</param>
        /// <returns></returns>
        public static async Task CreateNewEmptyPageAsync(Func<PageItem, Task> deleteAction)
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

            PagesList.Add(new PageItem(newIndex, "", deleteAction));

            //Добавим страницу для всех отображённых ролей
            for (int i = 0; i < RoleList.Count; i++)
                RoleList[i].Rights.Add(new PageRoleItem(newIndex, "", false));
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

            //Назначим для всех отображённых ролей
            for (int i = 0; i < RoleList.Count; i++)
                RoleList[i].Rights.FirstOrDefault(x => x.PageID == item.ID).PageName = item.PageName;
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

            //Удалим данную страницу у всех отображённых прав
            for (int i = 0; i < RoleList.Count; i++)
                RoleList[i].Rights.Remove(RoleList[i].Rights.FirstOrDefault(x => x.PageID == item.ID));
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

            RoleList.Remove(item);
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
        }
    }
}