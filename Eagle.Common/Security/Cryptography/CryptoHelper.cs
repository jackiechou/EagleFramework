using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security;

namespace Eagle.Common.Security.Cryptography
{
    /// <summary>
    /// The algorithm types available for hashing.
    /// </summary>
    public enum HashType
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    /// <summary>
    /// Contains extensions to Stream, byte[], string that helps to encrypt/decrypt using symmetric and asymetric algorithms and to compute hashes.
    /// </summary>
    public static class CryptoHelper
    {

        #region Aes

        static AesCryptoServiceProvider CreateAesCryptoServiceProvider(byte[] key, byte[] iv = null)
        {
            if (key.Length != 32)
            {
                throw new Exception("The key must have 32 bytes!");
            }
            
            AesCryptoServiceProvider aesService = new AesCryptoServiceProvider();
            aesService.KeySize = 256;
            aesService.Key = key;

            //Array.Copy(key, aesService.IV, 16); //doesn't work
            if (iv == null)
            {
                iv = new byte[16];
                Array.Copy(key, iv, 16);
            }
            aesService.IV = iv;
            return aesService;
        }

        /// <summary>
        /// A symmetric service provider used to encrypt/decrypt by default when another service provider isn't provided. (by default it is initialized with a new AesCryptoServiceProvider that will accept keys of 256 bytes size).
        /// </summary>
        public static SymmetricAlgorithm DefaultSymmetricCryptoServiceProvider { get; set; }

        #region Stream
        

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        public static void Encrypt(this Stream source, Stream destination, SymmetricAlgorithm serviceProvider = null)
        {
            SymmetricAlgorithm service = serviceProvider == null ? DefaultSymmetricCryptoServiceProvider : serviceProvider;
            
            CryptoStream csEncrypt = null;
            try
            {
                ICryptoTransform encryptor = service.CreateEncryptor();
                csEncrypt = new CryptoStream(destination, encryptor, CryptoStreamMode.Write);
                source.CopyTo(csEncrypt);
            }
            finally
            {
                if (csEncrypt != null)
                    csEncrypt.Close();
            }

        }

        /// <summary>
        /// Decrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting decrypted stream.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        public static void Decrypt(this Stream source, Stream destination, SymmetricAlgorithm serviceProvider = null)
        {
            SymmetricAlgorithm service = serviceProvider == null ? DefaultSymmetricCryptoServiceProvider : serviceProvider;
            CryptoStream csDecrypt = null;
            try
            {
                ICryptoTransform decryptor = service.CreateDecryptor();
                csDecrypt = new CryptoStream(source, decryptor, CryptoStreamMode.Read);
                csDecrypt.CopyTo(destination);
            }
            finally
            {
                if (csDecrypt != null)
                    csDecrypt.Close();
            }
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void Encrypt(this Stream source, Stream destination, byte[] key, byte[] iv = null)
        {
            Encrypt(source, destination, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Decrypts an encrypted stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting decrypted stream.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void Decrypt(this Stream source, Stream destination, byte[] key, byte[] iv = null)
        {
            Decrypt(source, destination, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void Encrypt(this Stream source, Stream destination, string key, string iv = null)
        {
            Encrypt(source, destination, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        /// <summary>
        /// Decrypts an encrypted stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting decrypted stream.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static void Decrypt(this Stream source, Stream destination, string key, string iv = null)
        {
            Decrypt(source, destination, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        public static MemoryStream Encrypt(this Stream source, SymmetricAlgorithm serviceProvider = null)
        {
            MemoryStream encrypted = new MemoryStream();
            Encrypt(source, encrypted, serviceProvider);
            return encrypted;
        }

        /// <summary>
        /// Decrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The resulting decrypted stream in memory.</returns>
        public static MemoryStream Decrypt(this Stream source, SymmetricAlgorithm serviceProvider = null)
        {
            MemoryStream clear = new MemoryStream();
            Decrypt(source, clear, serviceProvider);
            return clear;
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        public static MemoryStream Encrypt(this Stream source, byte[] key, byte[] iv = null)
        {
            return Encrypt(source, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Decrypts an encrypted stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The decrypted stream in memory.</returns>
        public static MemoryStream Decrypt(this Stream source, byte[] key, byte[] iv = null)
        {
            return Decrypt(source, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        public static MemoryStream Encrypt(this Stream source, string key, string iv = null)
        {
            return Encrypt(source, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        /// <summary>
        /// Decrypts an encrypted stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The decrypted stream in memory.</returns>
        public static MemoryStream Decrypt(this Stream source, string key, string iv = null)
        {
            return Decrypt(source, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        #endregion

        #region Byte Array

        /// <summary>
        /// Encrypts a byte array using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The resulting encrypted byte array.</returns>
        public static byte[] Encrypt(this byte[] data, SymmetricAlgorithm serviceProvider = null)
        {
            MemoryStream msClear = new MemoryStream(data);
            MemoryStream msEncrypted = new MemoryStream();
            Encrypt(msClear, msEncrypted, serviceProvider);
            return msEncrypted.ToArray();
        }

        /// <summary>
        /// Decrypts a byte array using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The resulting decrypted byte array.</returns>
        public static byte[] Decrypt(this byte[] data, SymmetricAlgorithm serviceProvider = null)
        {
            MemoryStream msEncrypted = new MemoryStream(data);
            MemoryStream msClear = new MemoryStream();
            Decrypt(msEncrypted, msClear, serviceProvider);
            return msClear.ToArray();
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The resulting encrypted byte array.</returns>
        public static byte[] Encrypt(this byte[] data, byte[] key, byte[] iv = null)
        {
            return Encrypt(data, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Decrypts an encrypted byte array using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static byte[] Decrypt(this byte[] data, byte[] key, byte[] iv = null)
        {
            return Decrypt(data, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The resulting encrypted byte array.</returns>        
        public static byte[] Encrypt(this byte[] data, string key, string iv = null)
        {
            return Encrypt(data, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        /// <summary>
        /// Decrypts an encrypted byte array using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static byte[] Decrypt(this byte[] data, string key, string iv = null)
        {
            return Decrypt(data, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        #endregion

        #region String

        /// <summary>
        /// Encrypts a string using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The string to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The encrypted string (base64).</returns>
        public static string Encrypt(this string data, SymmetricAlgorithm serviceProvider = null)
        {
            byte[] b = Encoding.UTF8.GetBytes(data);
            b = Encrypt(b, serviceProvider);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Decrypts an encrypted string using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The string to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultSymmetricCryptoServiceProvider will be used instead.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(this string data, SymmetricAlgorithm serviceProvider = null)
        {
            byte[] b = Convert.FromBase64String(data);
            b = Decrypt(b, serviceProvider);
            return Encoding.UTF8.GetString(b);
        }

        /// <summary>
        /// Encrypts a stream using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The string to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The encrypted string (base64).</returns>
        public static string Encrypt(this string data, byte[] key, byte[] iv = null)
        {
            return Encrypt(data, CreateAesCryptoServiceProvider(key, iv));
        }

        /// <summary>
        /// Decrypts an encrypted string using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static string Decrypt(this string data, byte[] key, byte[] iv = null)
        {
            return Decrypt(data, CreateAesCryptoServiceProvider(key, iv));

        }

        /// <summary>
        /// Encrypts a string using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The string to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>The encrypted string (base64).</returns>
        public static string Encrypt(this string data, string key, string iv = null)
        {
            return Encrypt(data, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        /// <summary>
        /// Decrypts an encrypted string using a symmetric algorithm (by default AES).
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="key">The key used by a new AesCryptoServiceProvider.</param>
        /// <param name="iv">The initialization vector.</param>
        public static string Decrypt(this string data, string key, string iv = null)
        {
            return Decrypt(data, Encoding.UTF8.GetBytes(key), iv != null ? Encoding.UTF8.GetBytes(iv) : null);
        }

        #endregion

        #endregion

        #region Rsa

        static RSACryptoServiceProvider CreateRsaCryptoServiceProvider(string xmlRsaKey)
        {
            RSACryptoServiceProvider rsaService = new RSACryptoServiceProvider();
            rsaService.FromXmlString(xmlRsaKey);
            return rsaService;
        }

        /// <summary>
        /// RSACryptoServiceProvider used to encrypt/decrypt by default when another service provider isn't provided.
        /// </summary>
        public static RSACryptoServiceProvider DefaultRsaCryptoServiceProvider { get; set; }

        #region Byte Array

        /// <summary>
        /// Encryptes a byte array using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] RSAEncrypt(this byte[] data, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            RSACryptoServiceProvider service = serviceProvider == null ? DefaultRsaCryptoServiceProvider : serviceProvider;
            byte[] encryptedData;
            encryptedData = service.Encrypt(data, doOAEPPadding);
            return encryptedData;
        }

        /// <summary>
        /// Decryptes a RSA encrypted byte array.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] RSADecrypt(this byte[] data, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            RSACryptoServiceProvider service = serviceProvider == null ? DefaultRsaCryptoServiceProvider : serviceProvider;
            byte[] decryptedData;
            decryptedData = service.Decrypt(data, doOAEPPadding);
            return decryptedData;
        }

        /// <summary>
        /// Encryptes a byte array using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] RSAEncrypt(this byte[] data, bool doOAEPPadding = true)
        {
            return RSAEncrypt(data, DefaultRsaCryptoServiceProvider, doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a RSA encrypted byte array.
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] RSADecrypt(this byte[] data, bool doOAEPPadding = true)
        {
            return RSADecrypt(data, DefaultRsaCryptoServiceProvider, doOAEPPadding);
        }

        /// <summary>
        /// Encryptes a byte array using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] RSAEncrypt(this byte[] data, string xmlKey, bool doOAEPPadding = true)
        {
            return RSAEncrypt(data, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a byte array using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] RSADecrypt(this byte[] data, string xmlKey, bool doOAEPPadding = true)
        {
            return RSADecrypt(data, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        #endregion

        #region String
        

        /// <summary>
        /// Encryptes a string using the RSA algorithm.
        /// </summary>
        /// <param name="data">The string to be encrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted string (base64).</returns>
        public static string RSAEncrypt(this string data, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(RSAEncrypt(byteData, serviceProvider, doOAEPPadding));
        }

        /// <summary>
        /// Decryptes a RSA encrypted string.
        /// </summary>
        /// <param name="data">The string to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted string.</returns>
        public static string RSADecrypt(this string data, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            byte[] byteData = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(RSADecrypt(byteData, serviceProvider, true));
        }

        /// <summary>
        /// Encryptes a string using the RSA algorithm.
        /// </summary>
        /// <param name="data">The string to be encrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The encrypted string (base64).</returns>
        public static string RSAEncrypt(this string data, bool doOAEPPadding = true)
        {
            return RSAEncrypt(data, DefaultRsaCryptoServiceProvider, doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a RSA encrypted string.
        /// </summary>
        /// <param name="data">The string to be decrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted string.</returns>
        public static string RSADecrypt(this string data, bool doOAEPPadding = true)
        {
            return RSADecrypt(data, DefaultRsaCryptoServiceProvider, doOAEPPadding);
        }

        /// <summary>
        /// Encryptes a string using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The encrypted string (base64).</returns>
        public static string RSAEncrypt(this string data, string xmlKey, bool doOAEPPadding = true)
        {
            return RSAEncrypt(data, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a string using the RSA algorithm.
        /// </summary>
        /// <param name="data">The byte array to be decrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted string.</returns>
        public static string RSADecrypt(this string data, string xmlKey, bool doOAEPPadding = true)
        {
            return RSADecrypt(data, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        #endregion

        #region Stream
        
        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="serviceProvider">The service provider used to do the encryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static void RSAEncrypt(this Stream source, Stream destination, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            MemoryStream ms = new MemoryStream();
            source.CopyTo(ms);
            byte[] enc = RSAEncrypt(ms.ToArray(), serviceProvider, doOAEPPadding);
            destination.Write(enc, 0, enc.Length);
        }

        /// <summary>
        /// Decryptes a RSA encrypted stream.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        public static void RSADecrypt(this Stream source, Stream destination, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            MemoryStream ms = new MemoryStream();
            source.CopyTo(ms);
            byte[] enc = RSADecrypt(ms.ToArray(), doOAEPPadding);
            destination.Write(enc, 0, enc.Length);
        }

        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static void RSAEncrypt(this Stream source, Stream destination, bool doOAEPPadding = true)
        {
            RSAEncrypt(source, destination, DefaultRsaCryptoServiceProvider, true);
        }

        /// <summary>
        /// Decryptes a RSA encrypted stream.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        public static void RSADecrypt(this Stream source, Stream destination, bool doOAEPPadding = true)
        {
            RSADecrypt(source, destination, DefaultRsaCryptoServiceProvider, doOAEPPadding);
        }

        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="destination">The resulting encrypted stream.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static void RSAEncrypt(this Stream source, Stream destination, string xmlKey, bool doOAEPPadding = true)
        {
            RSAEncrypt(source, destination, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="destination">The resulting decrypted stream.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        public static void RSADecrypt(this Stream source, Stream destination, string xmlKey, bool doOAEPPadding = true)
        {
            RSADecrypt(source, destination, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static MemoryStream RSAEncrypt(this Stream source, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            MemoryStream destination = new MemoryStream();
            RSAEncrypt(source, destination, doOAEPPadding);
            return destination;
        }

        /// <summary>
        /// Decryptes a RSA encrypted stream.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="serviceProvider">The service provider used to do the decryption. If not provided, the DefaultRsaCryptoServiceProvider will be used instead.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The resulting decrypted stream in memory.</returns>
        public static MemoryStream RSADecrypt(this Stream source, RSACryptoServiceProvider serviceProvider, bool doOAEPPadding = true)
        {
            MemoryStream destination = new MemoryStream();
            RSADecrypt(source, destination, doOAEPPadding);
            return destination;
        }

        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static MemoryStream RSAEncrypt(this Stream source, bool doOAEPPadding = true)
        {
            return RSAEncrypt(source, (RSACryptoServiceProvider)null, doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a RSA encrypted stream.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The resulting decrypted stream in memory.</returns>
        public static MemoryStream RSADecrypt(this Stream source, bool doOAEPPadding = true)
        {
            return RSADecrypt(source, (RSACryptoServiceProvider)null, doOAEPPadding);
        }

        /// <summary>
        /// Encryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be encrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The resulting encrypted stream in memory.</returns>
        /// <remarks>The length of the source stream must be extremely low - For example, with the 2048 bit encryption a maximum of 256 bytes can be encrypted.</remarks>
        public static MemoryStream RSAEncrypt(this Stream source, string xmlKey, bool doOAEPPadding = true)
        {
            return RSAEncrypt(source, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        /// <summary>
        /// Decryptes a stream using the RSA algorithm.
        /// </summary>
        /// <param name="source">The stream to be decrypted.</param>
        /// <param name="xmlKey">The key used by a new RSACryptoServiceProvider.</param>
        /// <param name="doOAEPPadding">True for OAEP padding, false for PKCS#1 v1.5 padding.</param>
        /// <returns>The decrypted stream in memory.</returns>
        public static MemoryStream RSADecrypt(this Stream source, string xmlKey, bool doOAEPPadding = true)
        {
            return RSADecrypt(source, CreateRsaCryptoServiceProvider(xmlKey), doOAEPPadding);
        }

        #endregion

        #endregion

        #region Protect
        static byte[] protectSalt = null;

        /// <summary>
        /// The salt used to protect (encrypt) data. Used by the Protect methods if their salt parameter is not provided.
        /// </summary>
        public static string DefaultProtectSalt
        {
            get
            {
                return Encoding.UTF8.GetString(protectSalt);
            }
            set
            {
                protectSalt = Encoding.UTF8.GetBytes(value);
            }
        }

        static byte[] Protect(byte[] data, byte[] salt)
        {
            byte[] useSalt = (salt == null) ? protectSalt : salt;
            byte[] encryptedData = ProtectedData.Protect(data, useSalt, DataProtectionScope.CurrentUser);
            return encryptedData;
        }

        static byte[] Unprotect(byte[] data, byte[] salt)
        {
            byte[] useSalt = (salt == null) ? protectSalt : salt;
            byte[] decryptedData = ProtectedData.Unprotect(data, useSalt, DataProtectionScope.CurrentUser);
            return decryptedData;
        }

        /// <summary>
        ///  It provides protection using the user credentials to encrypt data.
        /// </summary>
        /// <param name="data">Data to be protected.</param>
        /// <param name="salt">An additional key used to encrypt data. If not provided the DefaultProtectSalt will be used instead.</param>
        /// <returns>The encrypted data.</returns>
        public static byte[] Protect(this byte[] data, string salt = null)
        {
            return Protect(data, salt != null ? Encoding.UTF8.GetBytes(salt) : null);
        }

        /// <summary>
        /// It provides protection using the user credentials to encrypt or decrypt data.
        /// </summary>
        /// <param name="data">String to be encrypted.</param>
        /// <param name="salt">An additional key used to encrypt data. If not provided the DefaultProtectSalt will be used instead.</param>
        /// <returns>The encrypted string.</returns>
        public static string Protect(this  string data, string salt = null)
        {
            byte[] encryptedData = Protect(Encoding.UTF8.GetBytes(data), salt);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Unprotects (decrypts) using the user credentials.
        /// </summary>
        /// <param name="data">Data to be decrypted.</param>
        /// <param name="salt">An additional key used to decrypt data. If not provided the DefaultProtectSalt will be used instead.</param>
        /// <returns>The decrypted data.</returns>
        public static byte[] Unprotect(this byte[] data, string salt = null)
        {
            return Unprotect(data, salt != null ? Encoding.UTF8.GetBytes(salt) : null);
        }

        /// <summary>
        /// Unprotects (decrypts) using the user credentials.
        /// </summary>
        /// <param name="data">String to be decrypted.</param>
        /// <param name="salt">An additional key used to decrypt data. If not provided the DefaultProtectSalt will be used instead.</param>
        /// <returns>The decrypted string.</returns>
        public static string Unprotect(this string data, string salt = null)
        {
            byte[] decryptedData = Unprotect(Convert.FromBase64String(data), salt);
            return Encoding.UTF8.GetString(decryptedData);
        }

        #endregion

        #region Hash

        static byte[] hashSalt = new byte[0];
        /// <summary>
        /// Used by the ComputeHash methods to salt the hash.
        /// </summary>
        public static string DefaultHashSalt
        {
            get
            {
                return Encoding.UTF8.GetString(hashSalt);
            }
            set
            {
                hashSalt = Encoding.UTF8.GetBytes(value);
            }
        }

        /// <summary>
        /// The size of the buffer used to make a transformation.
        /// </summary>
        public static int HashBuferSize { get; set; }



        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The stream for which to calculate the hash.</param>
        /// <param name="hash">The algorith to use for the hash.</param>
        /// <param name="salt">The salt to add to the stream. If not provided the DefaultHashSalt will be used instead.</param>
        /// <returns>The hash.</returns>
        public static byte[] ComputeHash(Stream data, HashAlgorithm hash, string salt = null)
        {
            byte[] buffer = new byte[HashBuferSize];
            int readBytes;

            while ((readBytes = data.Read(buffer, 0, HashBuferSize)) > 0)
            {
                hash.TransformBlock(buffer, 0, readBytes, buffer, 0);
            }

            byte[] useSalt = salt == null ? hashSalt : Encoding.UTF8.GetBytes(salt);
            hash.TransformFinalBlock(useSalt, 0, useSalt.Length);
            return hash.Hash;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The stream for which to calculate the hash.</param>
        /// <param name="hashType">The algorith to use for the hash.</param>
        /// <param name="salt">The salt to add to the stream. If not provided the DefaultHashSalt will be used instead.</param>
        /// <returns>The hash (base64).</returns>
        public static string ComputeHash(this Stream data, HashType hashType = HashType.MD5, string salt = null)
        {
            HashAlgorithm hash = null;
            switch (hashType)
            {
                case HashType.SHA1:
                    hash = new SHA1CryptoServiceProvider();
                    break;

                case HashType.SHA256:
                    hash = new SHA256CryptoServiceProvider();
                    break;

                case HashType.SHA384:
                    hash = new SHA384CryptoServiceProvider();
                    break;

                case HashType.SHA512:
                    hash = new SHA512CryptoServiceProvider();
                    break;

                case HashType.MD5:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            return Convert.ToBase64String(ComputeHash(data, hash, salt));
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data for which to calculate the hash.</param>
        /// <param name="hashType">The algorithm type to use for the hash.</param>
        /// <param name="salt">The salt to add to the data. If not provided the DefaultHashSalt will be used instead.</param>
        /// <returns>The hash (base64).</returns>
        public static string ComputeHash(this byte[] data, HashType hashType = HashType.MD5, string salt = null)
        {
            MemoryStream ms = new MemoryStream(data);
            return ComputeHash(ms, hashType, salt);
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="data">The data for which to calculate the hash.</param>
        /// <param name="hashType">The algorithm type to use for the hash.</param>
        /// <param name="salt">The salt to add to the data. If not provided the DefaultHashSalt will be used instead.</param>
        /// <returns>The hash (base64).</returns>
        public static string ComputeHash(this string data, HashType hashType = HashType.MD5, string salt = null)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            return ComputeHash(ms, hashType, salt);
        }


        #endregion

        static CryptoHelper()
        {
            DefaultSymmetricCryptoServiceProvider = new AesCryptoServiceProvider();
            DefaultRsaCryptoServiceProvider = new RSACryptoServiceProvider();

            DefaultHashSalt = "";
            HashBuferSize = 64 * 1024;
        }
    }
}
