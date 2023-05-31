using RestourantDesktop.Database;
using System;
using System.Collections.Generic;
using System.Data;

namespace RestourantDesktop.UserController
{
    internal class User : IDisposable
    {
        public string UserID = string.Empty;
        public List<string> PagesListForUser = new List<string>();

        public User()
        {
            Dependency.UserRolesChangedEvent += (sender, e) => GetUserPagesList();
            Dependency.EmployeeUsersChangedEvent += (sender, e) => GetUserPagesList();
            Dependency.RoleRightChangedEvent += (sender, e) => GetUserPagesList();
            Dependency.RoleChangedEvent += (sender, e) => GetUserPagesList();
            Dependency.PagesChangedEvent += (sender, e) => GetUserPagesList();
        }

        public void Dispose()
        {
            Dependency.UserRolesChangedEvent -= (sender, e) => GetUserPagesList();
            Dependency.EmployeeUsersChangedEvent -= (sender, e) => GetUserPagesList();
            Dependency.RoleRightChangedEvent -= (sender, e) => GetUserPagesList();
            Dependency.RoleChangedEvent -= (sender, e) => GetUserPagesList();
            Dependency.PagesChangedEvent -= (sender, e) => GetUserPagesList();
        }

        public void GetUserPagesList()
        {
            PagesListForUser.Clear();
            DataTable pages = UserController.GetPagesListAsync(UserID);

            for (int i = 0; i < pages.Rows.Count; i++)
                PagesListForUser.Add(pages.Rows[i]["PageName"].ToString());

            UserController.AuthorizedUserStatsChanged();
        }
    }
}