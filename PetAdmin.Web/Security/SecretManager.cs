using System.Security.Cryptography;
using System.Text;

namespace PetAdmin.Web.Security
{
    public static class SecretManager
    {
        private static RNGCryptoServiceProvider _cryptoServiceProvider = null;
        private const int SALT_SIZE = 24;

        static SecretManager()
        {
            _cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        // utilty function to convert string to byte[]        
        public static byte[] GetBytes(string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        // utilty function to convert byte[] to string        
        public static string GetString(byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes);
        }

        public static string GetSaltString()
        {
            byte[] saltBytes = new byte[SALT_SIZE];

            _cryptoServiceProvider.GetNonZeroBytes(saltBytes);
            string saltString = GetString(saltBytes);
            return saltString;
        }

        public static string GetPasswordHashAndSalt(string message)
        {
            // Use SHA256 algorithm to hash salted password
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = GetBytes(message);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            return GetString(resultBytes);
        }

        public static string GeneratePasswordHash(string plainTextPassword, out string salt)
        {
            salt = GetSaltString();
            string finalString = plainTextPassword + salt;
            return GetPasswordHashAndSalt(finalString);
        }

        public static bool IsPasswordMatch(string password, string salt, string hash)
        {
            string finalString = password + salt;
            return hash == GetPasswordHashAndSalt(finalString);
        }
    }
}
