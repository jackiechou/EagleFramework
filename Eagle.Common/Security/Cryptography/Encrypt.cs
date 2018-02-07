using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;

namespace Eagle.Common.Security.Cryptography
{
    /// <summary>
    /// A simple encryption class that can be used to two-way encode/decode strings and byte buffers
    /// with single method calls.
    /// </summary>
    public class Encrypt
    {
        private readonly SymmetricAlgorithm _mCsp;
        private static string _strKey;

        public Encrypt()
        {
            _mCsp = new TripleDESCryptoServiceProvider();
            _strKey = "5EAGLES";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(_strKey));
            hashmd5.Clear();

            _mCsp.Key = keyArray;
            _mCsp.Mode = CipherMode.ECB;
            _mCsp.Padding = PaddingMode.PKCS7;
        }

        public string EncryptString(string value)
        {
            ICryptoTransform ct = _mCsp.CreateEncryptor();

            byte[] byt = Encoding.UTF8.GetBytes(value);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string DecryptString(string value)
        {
            ICryptoTransform ct = _mCsp.CreateDecryptor();

            byte[] byt = Convert.FromBase64String(value);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Encodes a stream of bytes using DES encryption with a pass key. Lowest level method that 
        /// handles all work.
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static byte[] EncryptBytes(byte[] inputString, string encryptionKey)
        {
            if (encryptionKey == null)
                encryptionKey = _strKey;

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            des.Key = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));
            des.Mode = CipherMode.ECB;

            ICryptoTransform transform = des.CreateEncryptor();

            byte[] buffer = inputString;
            return transform.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Encrypts a string into bytes using DES encryption with a Passkey. 
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static byte[] EncryptBytes(string decryptString, string encryptionKey)
        {
            return EncryptBytes(Encoding.ASCII.GetBytes(decryptString), encryptionKey);
        }

        /// <summary>
        /// Encrypts a string using Triple DES encryption with a two way encryption key.String is returned as Base64 encoded value
        /// rather than binary.
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static string EncryptString(string inputString, string encryptionKey)
        {
            return Convert.ToBase64String(EncryptBytes(Encoding.ASCII.GetBytes(inputString), encryptionKey));
        }


        /// <summary>
        /// Decrypts a Byte array from DES with an Encryption Key.
        /// </summary>
        /// <param name="decryptBuffer"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static byte[] DecryptBytes(byte[] decryptBuffer, string encryptionKey)
        {
            if (decryptBuffer == null || decryptBuffer.Length == 0)
                return null;

            if (encryptionKey == null)
                encryptionKey = _strKey;

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            des.Key = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(encryptionKey));
            des.Mode = CipherMode.ECB;

            ICryptoTransform transform = des.CreateDecryptor();

            return transform.TransformFinalBlock(decryptBuffer, 0, decryptBuffer.Length);
        }

        public static byte[] DecryptBytes(string decryptString, string encryptionKey)
        {
            return DecryptBytes(Convert.FromBase64String(decryptString), encryptionKey);
        }

        /// <summary>
        /// Decrypts a string using DES encryption and a pass key that was used for 
        /// encryption.
        /// <seealso>Class wwEncrypt</seealso>
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>String</returns>
        public static string DecryptString(string decryptString, string encryptionKey)
        {
            try
            {
                return Encoding.ASCII.GetString(DecryptBytes(Convert.FromBase64String(decryptString), encryptionKey));
            }
            catch { return string.Empty; }  // Probably not encoded
        }


        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. 
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        /// <remarks>
        /// ComputeHash code provided as an example by Obviex at
        /// http://www.obviex.com/samples/hash.aspx
        /// As noted by Obviex themselves, code is definitely not optimally efficient.
        /// Should performance requirements necessitate improvement, this should
        /// be improved.
        /// </remarks>
        public static string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            if (plainText == null)
                return null;

            // If salt is not specified, generate it on the fly.
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

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

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
        /// GZip encodes a memory buffer to a compressed memory buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] GZipMemory(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();

            GZipStream gZip = new GZipStream(ms, CompressionMode.Compress);

            gZip.Write(buffer, 0, buffer.Length);
            gZip.Close();

            byte[] result = ms.ToArray();
            ms.Close();

            return result;
        }

        /// <summary>
        /// Encodes a string to a gzip compressed memory buffer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GZipMemory(string input)
        {
            return GZipMemory(Encoding.ASCII.GetBytes(input));
        }

        /// <summary>
        /// Encodes a file to a gzip memory buffer
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="isFile"></param>
        /// <returns></returns>
        public static byte[] GZipMemory(string filename, bool isFile)
        {
            if (!isFile)
            {
                return GZipMemory(Encoding.ASCII.GetBytes(filename));
            }

            byte[] buffer = File.ReadAllBytes(filename);
            return GZipMemory(buffer);
        }

        /// <summary>
        /// Encodes one file to another file that is gzip compressed.
        /// File is overwritten if it exists and not locked.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static bool GZipFile(string filename, string outputFile)
        {
            byte[] buffer = File.ReadAllBytes(filename);
            FileStream fs = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            GZipStream gZip = new GZipStream(fs, CompressionMode.Compress);
            gZip.Write(buffer, 0, buffer.Length);
            gZip.Close();
            fs.Close();

            return true;
        }

    }
}
