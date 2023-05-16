using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestourantDesktop.UserController
{
    internal static class UserKrypt
    {
        public static async Task<string> GenerateSalt()
        {
            string Salt = string.Empty;
            
            await Task.Run(() =>
            {
                Random rnd = new Random();
                byte[] saltBytes = new byte[rnd.Next(8, 16)];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }

                Salt = BitConverter.ToString(saltBytes).Replace("-", "");
            });

            return Salt;
        }

        public static async Task<string> GetSaltedPassword(string password, string salt)
        {
            string saltedPassword = string.Empty;
            await Task.Run(() =>
            {
                byte[] bytes = new byte[salt.Length + password.Length];

                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                int delimiter = (int)(password[0]) % 3 + 1;
                for (int i = 0, passIndex = 0, saltIndex = 0; i < bytes.Length; i++)
                {
                    if ((i % delimiter == 0 || passIndex >= password.Length) && saltIndex < salt.Length)
                    {
                        bytes[i] = saltBytes[saltIndex];
                        saltIndex++;
                    }
                    else
                    {
                        bytes[i] = passwordBytes[passIndex];
                        passIndex++;
                    }
                }
                saltedPassword = Encoding.UTF8.GetString(bytes);
            });
            return saltedPassword;
        }

        public static async Task<string> ComputeHash(string password)
        { 
            string hash = string.Empty;
            await Task.Run(() => 
            { 
                using (SHA512 hashAlg =  SHA512.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = hashAlg.ComputeHash(inputBytes);
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < hashBytes.Length; i++)
                        sb.Append(hashBytes[i].ToString("x2"));

                    hash = sb.ToString();
                }
            });
            return hash;
        }
    }
}