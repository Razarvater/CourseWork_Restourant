using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Xml;
using System.Collections.Generic;

namespace DependencyChecker
{
    /// <summary>
    /// DB change listener<br/><br/>
    /// Call event <see cref="Changed"/> when DB has been changed<br/>
    /// Call event <see cref="NotificationProcessStopped"/> when listenProcess stopped
    /// </summary>
    public class DBchangeListener : IDisposable
    {
        /// <summary>
        /// DB change event, with <see cref="Inserted"/> and <see cref="Deleted"/> lists
        /// </summary>
        public class DbChangeEventArgs : EventArgs
        {
            /// <summary>
            /// List with inserted values
            /// </summary>
            /// <remarks>
            /// <see cref="Dictionary{TKey, TValue}.Keys"/> - <paramref name="FieldName"/>
            /// </remarks>
            public List<Dictionary<string, object>> Inserted = new List<Dictionary<string, object>>();
            /// <summary>
            /// List with deleted values
            /// </summary>
            /// <remarks>
            /// <see cref="Dictionary{TKey, TValue}.Keys"/> - <paramref name="FieldName"/>
            /// </remarks>
            public List<Dictionary<string, object>> Deleted = new List<Dictionary<string, object>>();

            /// <summary>
            /// Create <see cref="DbChangeEventArgs"/> instance, and parse data to <see cref="Inserted"/> and <see cref="Deleted"/> lists
            /// </summary>
            /// <param name="data">XML data </param>
            /// <remarks>
            /// Example data:
            /// <code>
            /// &lt;root&gt;<br/>
            ///     &lt;inserted&gt;<br/>
            ///         &lt;row&gt;<br/>
            ///             &lt;FieldName&gt;<br/>
            ///                 value<br/>
            ///             &lt;/FieldName&gt;<br/>
            ///         &lt;/row&gt;<br/>
            ///         &lt;row&gt;<br/>
            ///             &lt;FieldName&gt;<br/>
            ///                 value_2<br/>
            ///             &lt;/FieldName&gt;<br/>
            ///         &lt;/row&gt;<br/>
            ///     &lt;/inserted&gt;<br/>
            ///     &lt;deleted&gt;<br/>
            ///         &lt;row&gt;<br/>
            ///             &lt;FieldName&gt;<br/>
            ///                 value_2<br/>
            ///             &lt;/FieldName&gt;<br/>
            ///         &lt;/row&gt;<br/>
            ///     &lt;/deleted&gt;<br/>
            /// &lt;/root&gt;<br/>
            /// </code>
            /// </remarks>
            public DbChangeEventArgs(string data)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(data);

                XmlNode root = document.DocumentElement;

                foreach (XmlNode item in root.ChildNodes)
                {
                    if (item.Name == "inserted")
                    {
                        for (int i = 0; i < item.ChildNodes.Count; i++)
                        {
                            XmlNode rowNode = item.ChildNodes[i];
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            for (int j = 0; j < rowNode.ChildNodes.Count; j++)
                                values.Add(rowNode.ChildNodes[j].Name, rowNode.ChildNodes[j].InnerText);
                            Inserted.Add(values);
                        }
                    }
                    else if (item.Name == "deleted")
                    {
                        for (int i = 0; i < item.ChildNodes.Count; i++)
                        {
                            XmlNode rowNode = item.ChildNodes[i];
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            for (int j = 0; j < rowNode.ChildNodes.Count; j++)
                                values.Add(rowNode.ChildNodes[j].Name, rowNode.ChildNodes[j].InnerText);
                            Deleted.Add(values);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event with changes data
        /// </summary>
        public event EventHandler<DbChangeEventArgs> Changed;
        /// <summary>
        /// Stopped listening process event
        /// </summary>
        public event EventHandler NotificationProcessStopped;

        /// <summary>
        /// <see cref="CancellationTokenSource"/> for cancel listenProcess
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// ConnectionString for SqlServer
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Database Name
        /// </summary>
        private string databaseName;

        /// <summary>
        /// Shema Name
        /// </summary>
        private string shemaName;

        /// <summary>
        /// Listening table name
        /// </summary>
        private string tableName;

        /// <summary>
        /// Active listening SessionID
        /// </summary>
        private string SessionID;

        /// <summary>
        /// command timeout
        /// </summary>
        private int CommandTimeout = 60000;

        /// <summary>
        /// Create <see cref="DBchangeListener"/> instance
        /// </summary>
        /// <param name="databaseName">Database name</param>
        /// <param name="shemaName">Shema name</param>
        /// <param name="tableName">Listening table name</param>
        /// <param name="connectionstring">ConnectionString for SqlServer</param>
        /// <param name="CommandTimeout">command timeout</param>
        public DBchangeListener(string databaseName,string shemaName,string tableName,string connectionstring, int CommandTimeout = 60000)
        {
            this.databaseName = databaseName;
            this.shemaName = shemaName;
            this.tableName = tableName;
            this.CommandTimeout = CommandTimeout;
            this.connectionString = connectionstring;
        }

        /// <summary>
        /// Start listening process
        /// </summary>
        public void start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(listen, cancellationTokenSource.Token);
        }

        /// <summary>
        /// Create session with DB
        /// </summary>
        /// <remarks>
        /// Set value to: <see cref="SessionID"/>
        /// </remarks>
        private void Connect()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand("ConnectUserToTable", conn))
            {
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@tableName", tableName));

                SqlParameter sessionIDparam = new SqlParameter("@sessionGUID", SqlDbType.NVarChar, 36);
                sessionIDparam.Direction = ParameterDirection.Output;
                command.Parameters.Add(sessionIDparam);

                command.ExecuteNonQuery();

                SessionID = sessionIDparam.Value.ToString();
            }
        }

        /// <summary>
        /// Drop session, delete <see cref="SessionID"/> value
        /// </summary>
        public void Disconnect()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand("DeconnectUserFromTable", conn))
            {
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@tableName", tableName));
                command.Parameters.Add(new SqlParameter("@sessionGUID", SessionID));

                command.ExecuteNonQuery();
                SessionID = string.Empty;
            }
        }

        /// <summary>
        /// Listening process
        /// </summary>
        /// <param name="input">input values from <see cref="ThreadPool.QueueUserWorkItem(WaitCallback, object)"/></param>
        private void listen(object input)
        {
            try
            {
                Connect();
                while (true)
                {
                    var message = ReceiveEvent();
                    if (!string.IsNullOrWhiteSpace(message))
                        Changed.Invoke(this, new DbChangeEventArgs(message));
                }
            }
            catch { }
            finally
            {
                NotificationProcessStopped?.BeginInvoke(this, EventArgs.Empty, null, null);
            }
        }

        /// <summary>
        /// Call to <paramref name="ServiceBroker"/> queue
        /// </summary>
        /// <returns>message value from queue, with XAML format for <see cref="DbChangeEventArgs"/></returns>
        private string ReceiveEvent()
        {
            var commandText = $@"
                DECLARE @ConvHandle UNIQUEIDENTIFIER
                DECLARE @message VARBINARY(MAX)
                USE [{databaseName}]
                WAITFOR (RECEIVE TOP(1) @ConvHandle=Conversation_Handle
                            , @message=message_body FROM {shemaName}.[{tableName}MessageQueue{SessionID}]), TIMEOUT {CommandTimeout / 2};
	            BEGIN TRY END CONVERSATION @ConvHandle; END TRY BEGIN CATCH END CATCH

                SELECT CAST(@message AS NVARCHAR(MAX)) 
            ";

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand(commandText, conn))
            {
                conn.Open();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read() || reader.IsDBNull(0)) return string.Empty;

                    return reader.GetString(0);
                }
            }
        }

        /// <summary>
        /// Stop Listening 
        /// </summary>
        public void Stop()
        {
            if (cancellationTokenSource.Token.IsCancellationRequested || !cancellationTokenSource.Token.CanBeCanceled)
                return;

            Disconnect();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose() => Stop();
    }
}