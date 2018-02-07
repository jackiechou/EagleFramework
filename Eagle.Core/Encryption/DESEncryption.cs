using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Eagle.Core.Encryption
{
    /// <summary>
    /// Encrypts and decrypts string and byte arrays using DES symmetric encryption
    /// </summary>
    public class DesEncryption : IEncryption
    {
        private static readonly TripleDESCryptoServiceProvider Des = new TripleDESCryptoServiceProvider();
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();

        /// <summary>
        /// Pre-defined keys - byte array
        /// </summary>
        public byte[] Iv = { 2, 3, 6, 9, 1, 3, 3, 1 },
            Key = {5, 23, 1, 9, 1, 5, 9, 1, 2, 12, 15, 1, 7, 4, 
                  15, 10, 16, 11, 21, 12, 1, 15, 23, 11};

        public byte[] Encrypt(byte[] input)
        {
            return transform(input,
                   Des.CreateEncryptor(Key, Iv));
        }

        public byte[] Decrypt(byte[] input)
        {
            return transform(input,
                   Des.CreateDecryptor(Key, Iv));
        }

        public string Encrypt(string text)
        {
            //  Return empty strings rather then break it.
            if (string.IsNullOrEmpty(text)) return text;

            byte[] input = Encoding.GetBytes(text);
            byte[] output = transform(input,
                            Des.CreateEncryptor(Key, Iv));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            //  Return empty strings rather then break it.
            if (string.IsNullOrEmpty(text)) return text;

            byte[] input = Convert.FromBase64String(text);
            byte[] output = transform(input,
                            Des.CreateDecryptor(Key, Iv));
            return Encoding.GetString(output);
        }

        public static string EncryptAndEncode(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return HttpUtility.UrlEncode(new DesEncryption().Encrypt(input));
        }

        public string AsciiToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += $"{Convert.ToUInt32(tmp.ToString()):x2}";
            }
            return hex;
        }

        public string HexToAscii(string hexString)
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        private Byte[] transform(Byte[] input, ICryptoTransform cryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream,
                         cryptoTransform, CryptoStreamMode.Write);

            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();

            // Read the memory stream and convert it back into a byte array
            memStream.Position = 0;
            byte[] result = memStream.ToArray();

            // close and release the streams
            memStream.Close();
            cryptStream.Close();

            // hand back the encrypted buffer
            return result;
        }
    }
}
