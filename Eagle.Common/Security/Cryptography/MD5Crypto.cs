using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Eagle.Common.Security.Cryptography
{
    public class Md5Crypto
    {
        private static string _saltKey = "5EAGLES_HIGH_TECHNOLOGY_COMPANY";

        #region enum, constants and fields
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
                    return _saltKey;
            }
            set
            {
                _saltKey = value;
            }
        }
        #endregion

        #region MD5 Crypto ==============================================================

        // Hash an input string and return the hash as a 32 character hexadecimal string.
        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Encrypt(string strToEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(strToEncrypt);

            //If hashing use get hashcode regards to your key           
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(SaltKey));
            //Always release the resources and flush data of the Cryptographic service provide
            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Encrypt the given string using the specified key.
        /// </summary>
        /// <param name="strToEncrypt">The string to be encrypted.</param>
        /// <param name="strKey">The encryption key.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encrypt(string strToEncrypt, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDesCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMd5 = new MD5CryptoServiceProvider();
                string strTempKey = strKey;
                var byteHash = objHashMd5.ComputeHash(Encoding.ASCII.GetBytes(strTempKey));
                objDesCrypto.Key = byteHash;
                objDesCrypto.Mode = CipherMode.ECB; //CBC, CFB
                var byteBuff = Encoding.ASCII.GetBytes(strToEncrypt);
                return Convert.ToBase64String(objDesCrypto.CreateEncryptor().
                    TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        /// <summary>
        /// Decrypt the given string using the default key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string strEncrypted)
        {
            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(strEncrypted);

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(SaltKey));
            //release any resource held by the MD5CryptoServiceProvider
            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Decrypt the given string using the specified key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <param name="strKey">The decryption key.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string strEncrypted, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDesCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMd5 = new MD5CryptoServiceProvider();
                string strTempKey = strKey;
                var byteHash = objHashMd5.ComputeHash(Encoding.ASCII.GetBytes(strTempKey));
                objDesCrypto.Key = byteHash;
                objDesCrypto.Mode = CipherMode.ECB; //CBC, CFB
                var byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = Encoding.ASCII.GetString
                (objDesCrypto.CreateDecryptor().TransformFinalBlock
                (byteBuff, 0, byteBuff.Length));
                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }


      

       
        #endregion ======================================================================
    }
}
