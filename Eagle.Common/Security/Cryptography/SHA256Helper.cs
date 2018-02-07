using System;
using System.Security.Cryptography;
using System.Text;

namespace Eagle.Common.Security.Cryptography
{
    public class Sha256Helper
    {
        private const int HashSize = 256 / 8;
        private static readonly HashAlgorithm Algorithm = new SHA256Managed();

        /// <summary>
        /// Generate SHA256 hash salted.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>return: hash(salt + $password) + salt</returns>
        public static string HashPassword(string password)
        {
            var saltBytes = GenerateSalt();
            byte[] hashBytes = HashSha256Salted(password, saltBytes);

            // hash(salt + $password) + salt
            var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            }

            return Convert.ToBase64String(hashWithSaltBytes);
        }

        /// <summary>
        /// Compares a hash of password to a given hash value.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string hashValue)
        {
            var hashWithSaltBytes = Convert.FromBase64String(hashValue);
            byte[] hashBytes = new byte[HashSize];
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - HashSize];

            for (int i = 0; i < HashSize; i++)
            {
                hashBytes[i] = hashWithSaltBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                saltBytes[i] = hashWithSaltBytes[HashSize + i];
            }

            var expectedHashBytes = HashSha256Salted(password, saltBytes);

            return CompareByteArrays(hashBytes, expectedHashBytes);
        }

        /// <summary>
        /// Computes the value for the password and salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltBytes"></param>
        /// <returns></returns>
        private static byte[] HashSha256Salted(string password, byte[] saltBytes)
        {
            // hash(salt + $password)
            var passwordBytes = Encoding.UTF8.GetBytes(password.Trim());
            byte[] result = new byte[saltBytes.Length + passwordBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                result[i] = saltBytes[i];
            }
            for (int i = 0; i < passwordBytes.Length; i++)
            {
                result[saltBytes.Length + i] = passwordBytes[i];
            }

            return Algorithm.ComputeHash(result);
        }

        /// <summary>
        /// Generate Salt 
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateSalt()
        {
            int minSaltSize = 8;
            int maxSaltSize = 16;
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] salt = new byte[saltSize];
            rng.GetNonZeroBytes(salt);
            return salt;
        }

        /// <summary>
        /// Compare two byte array
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
