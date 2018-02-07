using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Eagle.Common.Security.Cryptography
{
    public class HashAlgorithmUtility
    {
        #region enum, constants and fields
        public static string _SaltKey = "5EAGLES_HIGH_TECHNOLOGY_COMPANY";
        public static string SaltKey
        {
            get
            {
                //System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
                //_SaltKey = (string)settingsReader.GetValue("SaltKey", typeof(String));             

                string key = ConfigurationManager.AppSettings["SaltKey"];
                if (key != string.Empty)
                    return key;
                else
                    return _SaltKey;
            }
            set
            {
                _SaltKey = value;
            }
        }

        //types of hashing available
        public enum HashingTypes
        {
            /// <summary>
            /// Cyclic Redundancy Check provider, 32-bit
            /// </summary>
            CRC32,
            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-1 variant, 160-bit
            /// </summary>
            SHA1,
            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 256-bit
            /// </summary>
            SHA256,
            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 384-bit
            /// </summary>
            SHA384,
            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 512-bit
            /// </summary>
            SHA512,
            /// <summary>
            /// Message Digest algorithm 5, 128-bit
            /// </summary>
            MD5
        }
        #endregion


        #region Hashing Engine
        /// <summary>
        ///     returns the specific hashing alorithm
        /// </summary>
        /// <param name="hashingType">type of hashing to use</param>
        /// <returns>HashAlgorithm</returns>
        private static HashAlgorithm getHashAlgorithm(HashingTypes hashingType)
        {
            switch (hashingType)
            {
                case HashingTypes.MD5:
                    return new MD5CryptoServiceProvider();
                case HashingTypes.SHA1:
                    return new SHA1CryptoServiceProvider();
                case HashingTypes.SHA256:
                    return new SHA256Managed();
                case HashingTypes.SHA384:
                    return new SHA384Managed();
                case HashingTypes.SHA512:
                    return new SHA512Managed();
                default:
                    return new MD5CryptoServiceProvider();
            }
        }

        public static string Hash(String inputText)
        {
            return ComputeHash(inputText, HashingTypes.MD5);
        }

        public static string Hash(String inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText, hashingType);
        }

        public static string Hash(string plainText, string saltKey, HashingTypes hashingType)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(saltKey); 
            // If salt is not specified, generate it.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = getHashAlgorithm(hashingType);

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public static string Decrypt(string encryptedText, string saltKey, string VIKey)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(encryptedText, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }



        /// <summary>
        ///     computes the hash code and converts it to string
        /// </summary>
        /// <param name="inputText">input text to be hashed</param>
        /// <param name="hashingType">type of hashing to use</param>
        /// <returns>hashed string</returns>
        private static string ComputeHash(string inputText, HashingTypes hashingType)
        {
            HashAlgorithm HA = getHashAlgorithm(hashingType);

            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of input text
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);


            //hash the input byte array
            byte[] output = HA.ComputeHash(inputBytes);

            //convert output byte array to a string
            return Convert.ToBase64String(output);
        }

        private static string ComputeHash(string plainText, byte[] saltBytes, HashingTypes hashingType)
        {
            // If salt is not specified, generate it.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = getHashAlgorithm(hashingType);

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }
          
        /// <summary>
        /// Encrypts a clear text using specified password and salt.
        /// </summary>
        /// <param name="clearText">The text to encrypt.</param>
        /// <param name="password">The password to create key for.</param>
        /// <param name="salt">The salt to add to encrypted text to make it more secure.</param>
        public static string Encrypt(string inputText, string password, byte[] salt)
        {
            // Turn text to bytes
            byte[] clearBytes = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt);

            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();

            byte[] EncryptedData = ms.ToArray();

            return Convert.ToBase64String(EncryptedData);
        }
        /// <summary>
        /// Decrypts an encrypted text using specified password and salt.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="password">The password used to encrypt text.</param>
        /// <param name="salt">The salt added to encrypted text.</param>
        public static string Decrypt(string cipherText, string password, byte[] salt)
        {
            // Convert text to byte
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt);

            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);

            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();

            byte[] DecryptedData = ms.ToArray();

            return Encoding.Unicode.GetString(DecryptedData);
        }

        /// <summary>
        ///     returns true if the input text is equal to hashed text
        /// </summary>
        /// <param name="inputText">unhashed text to test</param>
        /// <param name="hashText">already hashed text</param>
        /// <returns>boolean true or false</returns>
        public static bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }

        public static bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText, hashingType) == hashText);
        }

        //public static bool VerifyHash(string plainText, string hashValue, HashingTypes hashingType)
        //{

        //    // Convert base64-encoded hash value into a byte array.
        //    byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        //    // We must know size of hash (without salt).
        //    int hashSizeInBits, hashSizeInBytes;
          
        //    // Size of hash is based on the specified algorithm.
        //    //HashAlgorithm hash = getHashAlgorithm(hashingType);
        //    switch (hashingType)
        //    {
        //        case HashingTypes.CRC32:
        //            hashSizeInBits = 160;
        //            break;
        //        case HashingTypes.MD5:
        //            hashSizeInBits = 128;
        //            break;
        //        case HashingTypes.SHA1:
        //            hashSizeInBits = 256;
        //            break;
        //        case HashingTypes.SHA384:
        //            hashSizeInBits = 384;
        //            break;
        //        case HashingTypes.SHA512:
        //            hashSizeInBits = 512;
        //            break;
        //        default: // Must be MD5
        //            hashSizeInBits = 128;
        //            break;
        //    }

        //    // Convert size of hash from bits to bytes.
        //    hashSizeInBytes = hashSizeInBits / 8;

        //    // Make sure that the specified hash value is long enough.
        //    if (hashWithSaltBytes.Length < hashSizeInBytes)
        //        return false;

        //    // Allocate array to hold original salt bytes retrieved from hash.
        //    byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

        //    // Copy salt from the end of the hash to the new array.
        //    for (int i = 0; i < saltBytes.Length; i++)
        //        saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

        //    // Compute a new hash string.
        //    string expectedHashString = ComputeHash(plainText, saltBytes,hashingType);

        //    // If the computed hash matches the specified hash,
        //    // the plain text value must be correct.
        //    return (hashValue == expectedHashString);
        //}
        #endregion
    }
}
