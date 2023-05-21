using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Threading.Tasks;

namespace RestourantDesktop.UserController
{
    internal static class UserController
    {
        public static User AuthorizedUser;
        public static event EventHandler AuthorizedUserStatsChangedEvent;
        public static void AuthorizedUserStatsChanged() => 
            AuthorizedUserStatsChangedEvent?.Invoke(new object(), new EventArgs());
        public static async Task<bool> TryAuthorizeUser(string password, string login)
        {
            string UserToken = string.Empty;
            try
            {
                string salt = string.Empty;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("GetSalt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@login", login));

                        salt = (await command.ExecuteScalarAsync()).ToString();
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("TryAuthorizeUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@login", login));
                        command.Parameters.Add(new SqlParameter("@password", await UserKrypt.ComputeHash(await UserKrypt.GetSaltedPassword(password, salt))));

                        UserToken = (await command.ExecuteScalarAsync()).ToString();
                    }
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }

            if (UserToken != string.Empty)
            {
                AuthorizedUser = new User() { UserID = UserToken };
                AuthorizedUser.GetUserPagesList();
            }

            return AuthorizedUser != null;
        }

        public static DataTable GetPagesListAsync(string UserID)
        {
            DataTable returnDT = new DataTable();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("EXEC GetUserPagesList @UserID", ConfigurationManager.ConnectionStrings["AdminConnectionString"].ConnectionString))
                {
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", UserID));
                    adapter.Fill(returnDT);
                }
            }
            catch (Exception) { /*TODO Сообщение об ошибке*/ }

            return returnDT;
        }

        public static async Task<(string HashedPassword, string salt)> CalculateNewPassword(string password)
        {
            (string HashedPassword, string salt) result = (string.Empty, string.Empty);

            result.salt = await UserKrypt.GenerateSalt();
            result.HashedPassword = await UserKrypt.ComputeHash(await UserKrypt.GetSaltedPassword(password, result.salt));

            return result;
        }
    }
}