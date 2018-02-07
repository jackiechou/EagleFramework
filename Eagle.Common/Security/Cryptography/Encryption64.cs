using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace Eagle.Common.Security.Cryptography
{
    public class Encryption64
    {
        private static string _EncryptionKey = "nWTpHwBpYeQ9MeLJ29a0hg==";
        private static byte[] key = { };
        private static byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 }; // it can be any byte value

        public static string EncryptionKey
        {
            get
            {
                string key = ConfigurationManager.AppSettings["SaltKey"];
                if (key != string.Empty)                
                    return key;              
                else
                    return _EncryptionKey;
            }
            set
            {
                    _EncryptionKey = value;
            }
        }

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length];
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }       

        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        { 
            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length];
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static string Encrypt(string stringToEncrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {          
            byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length)

            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    } 
}
