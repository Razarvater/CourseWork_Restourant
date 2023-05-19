using DependencyChecker;
using System;
using System.Configuration;

namespace RestourantDesktop.Database
{
    internal static class Dependency
    {
        #region events
        public static event EventHandler<DBchangeListener.DbChangeEventArgs> RoleChangedEvent;
        public static event EventHandler<DBchangeListener.DbChangeEventArgs> RoleRightChangedEvent;
        public static event EventHandler<DBchangeListener.DbChangeEventArgs> PagesChangedEvent;
        public static event EventHandler<DBchangeListener.DbChangeEventArgs> UserRolesChangedEvent;
        public static event EventHandler<DBchangeListener.DbChangeEventArgs> EmployeeUsersChangedEvent;
        #endregion

        public static DependencyManager manager;
        private static event EventHandler onAppExit;
        static Dependency()
        {
            manager = new DependencyManager
            (
                ConnectionString: ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString,
                shema: "dbo",
                SetupHandler
            );

            Dependency.manager.ListenTable("PagesList", PagesListChanged);
            Dependency.manager.ListenTable("RightRole", PageRoleListChanged);
            Dependency.manager.ListenTable("Roles", RoleListChanged);
            Dependency.manager.ListenTable("UserRoles", UserRolesChanged);
            Dependency.manager.ListenTable("EmployeeUsers", UserListChanged);
        }

        #region RightUpdates
        private static void UserListChanged(object sender, DBchangeListener.DbChangeEventArgs e) =>
            EmployeeUsersChangedEvent?.Invoke(sender, e);

        private static void UserRolesChanged(object sender, DBchangeListener.DbChangeEventArgs e) =>
           UserRolesChangedEvent?.Invoke(sender, e);

        private static void RoleListChanged(object sender, DBchangeListener.DbChangeEventArgs e) =>
            RoleChangedEvent?.Invoke(sender, e);

        private static void PageRoleListChanged(object sender, DBchangeListener.DbChangeEventArgs e) =>
            RoleRightChangedEvent?.Invoke(sender, e);

        private static void PagesListChanged(object sender, DBchangeListener.DbChangeEventArgs e) =>
            PagesChangedEvent?.Invoke(sender, e);
        #endregion
        public static void SetupHandler(EventHandler handler) =>
            onAppExit += handler;

        public static void CloseApplication() =>
            onAppExit?.Invoke(null, null);
    }
}
