using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Eagle.Core.Encryption
{
    public class EmailFileContentEncryption : IEncryption
    {
        /// <summary>
        /// Encrypts a string, to be used for encrypting sensitive data such as employee csv file contents
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string Encrypt(
            string inputString)
        {
            // get the encryption key from config
            //var key = ConfigurationManager.AppSettings["FileEncryptionKey"];

            var key = "68BF8D5152C55C23D4E2EC939E3B6";
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            key = KeyConversion(key);
            MemoryStream memoryStream = new MemoryStream();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(key),
                IV = Encoding.ASCII.GetBytes(key)
            };
            ICryptoTransform desencrypt = des.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(memoryStream,
               desencrypt,
               CryptoStreamMode.Write);
            byte[] plainBytes = Encoding.ASCII.GetBytes(inputString);
            cryptostream.Write(plainBytes, 0, plainBytes.Length);

            cryptostream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptostream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;

        }
        public byte[] Encrypt(byte[] input) { throw new NotImplementedException(); }
        public byte[] Decrypt(byte[] input) { throw new NotImplementedException(); }
        public string Decrypt(string text) { throw new NotImplementedException(); }

        //converts our key to des-compatible
        private static string KeyConversion(string str)
        {
            byte[] bytes = new byte[8];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            var key = Encoding.ASCII.GetString(bytes);
            return key;
        }
    }
}
