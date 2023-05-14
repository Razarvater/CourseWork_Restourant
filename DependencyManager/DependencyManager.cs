using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static DependencyChecker.DBchangeListener;

namespace DependencyChecker
{
    /// <summary>
    /// Database dependency manager
    /// </summary>
    public class DependencyManager : IDisposable
    {
        /// <summary>
        /// connection string for SqlServer
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Database name
        /// </summary>
        private string DataBaseName;
        /// <summary>
        /// Shema name
        /// </summary>
        private string shema;

        /// <summary>
        /// Database listeners dictionary
        /// </summary>
        /// <remarks>
        /// <see cref="Dictionary{TKey, TValue}.Keys"/> - <paramref name="TableNames"/>
        /// </remarks>
        private Dictionary<string, DBchangeListener> listeners = new Dictionary<string, DBchangeListener>();

        /// <summary>
        /// Create <see cref="DependencyManager"/> instance
        /// </summary>
        /// <param name="ConnectionString">ConnectionString to SqlServer</param>
        /// <param name="shema">Shema name</param>
        /// <param name="applicationClosed">Action to assign an application exit event method</param>
        public DependencyManager(string ConnectionString, string shema, Action<EventHandler> applicationClosed)
        {
            this.connectionString = ConnectionString;
            this.shema = shema;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataBaseName = connection.Database;
            }
            applicationClosed?.Invoke((sender,e) => CloseAll());
        }

        /// <summary>
        /// Start listen table
        /// </summary>
        /// <param name="TableName">Table name for listening</param>
        /// <param name="dbChanged">Method for notifications about database changes</param>
        public void ListenTable(string TableName, EventHandler<DbChangeEventArgs> dbChanged)
        {
            if (listeners.TryGetValue(TableName, out DBchangeListener value)) return;
            DBchangeListener listener = new DBchangeListener(DataBaseName,shema,TableName, connectionString);

            listener.Changed += dbChanged;
            listener.start();

            listeners.Add(TableName,listener);
        }

        /// <summary>
        /// Stopping listening process <paramref name="TableName"/>
        /// </summary>
        /// <param name="TableName">Listening table name</param>
        public void StopListenProcess(string TableName)
        {
            if (listeners.TryGetValue(TableName, out DBchangeListener listener))
            {
                listener.Dispose();
                listeners.Remove(TableName);
            }
        }

        /// <summary>
        /// Close all listening process
        /// </summary>
        public void CloseAll()
        {
            foreach (var item in listeners)
                item.Value.Dispose();
            listeners.Clear();
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose() => CloseAll();
    }
}