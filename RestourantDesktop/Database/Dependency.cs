using DependencyChecker;
using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace RestourantDesktop.Database
{
    internal static class Dependency
    {
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
        }

        public static void SetupHandler(EventHandler handler) =>
            onAppExit += handler;

        public static void CloseApplication() =>
            onAppExit?.Invoke(null, null);
    }
}
