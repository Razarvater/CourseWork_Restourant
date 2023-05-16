using System.Threading.Tasks;

namespace RestourantDesktop.UserController
{
    internal static class UserController
    {
        public static async Task<(string HashedPassword, string salt)> CalculateNewPassword(string password)
        {
            (string HashedPassword, string salt) result = (string.Empty, string.Empty);

            result.salt = await UserKrypt.GenerateSalt();
            result.HashedPassword = await UserKrypt.ComputeHash(await UserKrypt.GetSaltedPassword(password, result.salt));

            return result;
        }
    }
}