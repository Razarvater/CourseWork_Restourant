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
                AuthorizedUser = new User() { UserSessionToken = UserToken };

            return AuthorizedUser != null;
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