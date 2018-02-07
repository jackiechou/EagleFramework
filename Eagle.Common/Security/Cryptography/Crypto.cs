using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace Eagle.Common.Security.Cryptography
{
    /// <summary>
    /// basic Encrption/decryption functionaility
    /// </summary>
    public class Crypto
    {
        #region enums, constants & fields
        //types of symmetric encyption
        public enum CryptoTypes
        {
            encTypeDES = 0,
            encTypeRC2,
            encTypeRijndael,
            encTypeTripleDES
        }

        private const string CRYPT_DEFAULT_PASSWORD = "CB06cfE507a1";
        private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeRijndael;

        private static byte[] mKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

        //private static byte[] mKey = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        private static byte[] mIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private static byte[] SaltByteArray = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private static CryptoTypes mCryptoType = CRYPT_DEFAULT_METHOD;
        private static string mPassword = CRYPT_DEFAULT_PASSWORD;
        #endregion

        #region Constructors

        public Crypto()
        {
            calculateNewKeyAndIV();
        }

        public Crypto(CryptoTypes cryptoType)
        {
            CryptoType = cryptoType;
        }
        
        #endregion

        #region Props

        /// <summary>
        ///     type of encryption / decryption used
        /// </summary>
        public static CryptoTypes CryptoType
        {
            get
            {
                return mCryptoType;
            }
            set
            {
                if (mCryptoType != value)
                {
                    mCryptoType = value;
                    calculateNewKeyAndIV();
                }
            }
        }

        /// <summary>
        ///     Passsword Key Property.
        ///     The password key used when encrypting / decrypting
        /// </summary>
        public static string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (mPassword != value)
                {
                    mPassword = value;
                    calculateNewKeyAndIV();
                }
            }
        }
        #endregion

        #region Encryption

        /// <summary>
        ///     Encrypt a string
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <returns>an encrypted string</returns>
        public static string Encrypt(string inputText)
        {
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of string
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

            //convert back to a string
            return Convert.ToBase64String(EncryptDecrypt(inputBytes, true));
        }

        /// <summary>
        ///     Encrypt string with user defined password
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <param name="password">password to use when encrypting</param>
        /// <returns>an encrypted string</returns>
        public static string Encrypt(string inputText, string password)
        {
            Password = password;
            return Encrypt(inputText);
        }

        /// <summary>
        ///     Encrypt string acc. to cryptoType and with user defined password
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <param name="password">password to use when encrypting</param>
        /// <param name="cryptoType">type of encryption</param>
        /// <returns>an encrypted string</returns>
        public static string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return Encrypt(inputText, password);
        }

        /// <summary>
        ///     Encrypt string acc. to cryptoType
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <param name="cryptoType">type of encryption</param>
        /// <returns>an encrypted string</returns>
        public static string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            CryptoType = cryptoType;
            return Encrypt(inputText);
        }

        #endregion

        #region Decryption

        /// <summary>
        ///     decrypts a string
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <returns>a decrypted string</returns>
        public static string Decrypt(string inputText)
        {
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of string
            byte[] inputBytes = Convert.FromBase64String(inputText);

            //convert back to a string
            return UTF8Encoder.GetString(EncryptDecrypt(inputBytes, false));
        }

        /// <summary>
        ///     decrypts a string using a user defined password key
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <param name="password">password to use when decrypting</param>
        /// <returns>a decrypted string</returns>
        public string Decrypt(string inputText, string password)
        {
            Password = password;
            return Decrypt(inputText);
        }

        /// <summary>
        ///     decrypts a string acc. to decryption type and user defined password key
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <param name="password">password key used to decrypt</param>
        /// <param name="cryptoType">type of decryption</param>
        /// <returns>a decrypted string</returns>
        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return Decrypt(inputText, password);
        }

        /// <summary>
        ///     decrypts a string acc. to the decryption type
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <param name="cryptoType">type of decryption</param>
        /// <returns>a decrypted string</returns>
        public static string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            CryptoType = cryptoType;
            return Decrypt(inputText);
        }
        #endregion

        #region Symmetric Engine

        /// <summary>
        ///     performs the actual enc/dec.
        /// </summary>
        /// <param name="inputBytes">input byte array</param>
        /// <param name="Encrpyt">wheather or not to perform enc/dec</param>
        /// <returns>byte array output</returns>
        private static byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
        {
            //get the correct transform
            ICryptoTransform transform = getCryptoTransform(Encrpyt);

            //memory stream for output
            MemoryStream memStream = new MemoryStream();

            try
            {
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

                //write data to cryption engine
                cryptStream.Write(inputBytes, 0, inputBytes.Length);

                //we are finished
                cryptStream.FlushFinalBlock();

                //get result
                byte[] output = memStream.ToArray();

                //finished with engine, so close the stream
                cryptStream.Close();

                return output;
            }
            catch (Exception e)
            {
                //throw an error
                throw new Exception("Error in symmetric engine. Error : " + e.Message, e);
            }
        }

        /// <summary>
        ///     returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private static ICryptoTransform getCryptoTransform(bool encrypt)
        {
            SymmetricAlgorithm SA = selectAlgorithm();
            var helper = new Rfc2898DeriveBytes(mPassword, SaltByteArray);
            SA.Key = helper.GetBytes(SA.KeySize / 8);
            SA.IV = helper.GetBytes(SA.BlockSize / 8);

            //SA.Key = mKey;
            //SA.IV = mIV;
            if (encrypt)
            {
                return SA.CreateEncryptor();
            }
            else
            {
                return SA.CreateDecryptor();
            }
        }
        /// <summary>
        ///     returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private static SymmetricAlgorithm selectAlgorithm()
        {
            SymmetricAlgorithm SA;
            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    SA = DES.Create();
                    break;
                case CryptoTypes.encTypeRC2:
                    SA = RC2.Create();
                    break;
                case CryptoTypes.encTypeRijndael:
                    SA = Rijndael.Create();
                    break;
                case CryptoTypes.encTypeTripleDES:
                    SA = TripleDES.Create();
                    break;
                default:
                    SA = TripleDES.Create();
                    break;
            }
            return SA;
        }

        /// <summary>
        ///     calculates the key and IV acc. to the symmetric method from the password
        ///     key and IV size dependant on symmetric method
        /// </summary>
        private static void calculateNewKeyAndIV()
        {
            //use salt so that key cannot be found with dictionary attack
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(mPassword, SaltByteArray);
            SymmetricAlgorithm algo = selectAlgorithm();
            mKey = pdb.GetBytes(algo.KeySize / 8);
            mIV = pdb.GetBytes(algo.BlockSize / 8);            
        }

        #endregion
    }

    /// <summary>
    /// Hashing class. Only static members so no need to create an instance
    /// </summary>
    public class Hashing
    {
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

        #region enum, constants and fields
       
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

        #region static members
        public static string Hash(String inputText)
        {
            return ComputeHash(inputText, HashingTypes.MD5);
        }

        public static string Hash(String inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText, hashingType);
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
        #endregion

     
        #region Encrypt - Decrypt with TripleDESCryptoServiceProvider======================

        public static string Encrypt(string strToEncrypt)
        {
            try
            {
                return Encrypt(strToEncrypt, SaltKey);
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
            try
            {
                return Decrypt(strEncrypted, SaltKey);
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
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
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().
                    TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
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
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = ASCIIEncoding.ASCII.GetString
                (objDESCrypto.CreateDecryptor().TransformFinalBlock
                (byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;
                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }


        public static byte[] GetLegalKey(SymmetricAlgorithm mCryptoService,string mSalt)
        {
            // Adjust key if necessary, and return a valid key
            string mKey = string.Empty;
            if (mCryptoService.LegalKeySizes.Length > 0)
            {
                // Key sizes in bits
                int keySize = mKey.Length * 8;
                int minSize = mCryptoService.LegalKeySizes[0].MinSize;
                int maxSize = mCryptoService.LegalKeySizes[0].MaxSize;
                int skipSize = mCryptoService.LegalKeySizes[0].SkipSize;
                
                if (keySize > maxSize)
                {
                    // Extract maximum size allowed
                    mKey = mKey.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    // Set valid size
                    int validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                    if (keySize < validSize)
                    {
                        // Pad the key with asterisk to make up the size
                        mKey = mKey.PadRight(validSize / 8, '*');
                    }
                }
            }
            PasswordDeriveBytes key = new PasswordDeriveBytes(mKey, ASCIIEncoding.ASCII.GetBytes(mSalt));
            return key.GetBytes(mKey.Length);
        }

        public static string EncryptByTripleDESCrypto(string toEncrypt)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
           
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);           
            byte[] keyArray = GetLegalKey(tdes, SaltKey);
            
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
        public static string DecryptByTripleDESCrypto(string cipherString)
        {            
            //get the byte code of the string
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] keyArray = GetLegalKey(tdes, SaltKey);
            
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
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        public static void EncryptTextToFile(String Data, String FileName, byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file.
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

                // Create a CryptoStream using the FileStream  
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Create a StreamWriter using the CryptoStream.
                StreamWriter sWriter = new StreamWriter(cStream);

                // Write the data to the stream  
                // to encrypt it.
                sWriter.WriteLine(Data);

                // Close the streams and 
                // close the file.
                sWriter.Close();
                cStream.Close();
                fStream.Close();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
            }

        }

        public static string DecryptTextFromFile(String FileName, byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file. 
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

                // Create a CryptoStream using the FileStream  
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create a StreamReader using the CryptoStream.
                StreamReader sReader = new StreamReader(cStream);

                // Read the data from the stream  
                // to decrypt it. 
                string val = sReader.ReadLine();

                // Close the streams and 
                // close the file.
                sReader.Close();
                cStream.Close();
                fStream.Close();

                // Return the string.  
                return val;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
                return null;
            }
        }

        public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                // Create a CryptoStream using the MemoryStream  
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Convert the passed string to a byte array. 
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the  
                // MemoryStream that holds the  
                // encrypted data. 
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer. 
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }

        }

        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a new MemoryStream using the passed  
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(Data);

                // Create a CryptoStream using the MemoryStream  
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create buffer to hold the decrypted data. 
                byte[] fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream 
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it. 
                return new ASCIIEncoding().GetString(fromEncrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion ====================================================================


        #region Hashing Engine

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
        #endregion

    }

    //public class testCrypt
    //{
    //    public void testEncryption()
    //    {
    //        string input = "Thi$ is @ str!&n to tEst encrypti0n!";
    //        Crypto c = new Crypto(Utils.Crypto.CryptoTypes.encTypeTripleDES);
    //        string s1 = c.Encrypt(input);
    //        string s2 = c.Decrypt(s1);
    //        Assert.IsTrue(s2 == input);

    //        s1 = Hashing.Hash(input);
    //        s2 = Hashing.Hash(input,Utils.Hashing.HashingTypes.MD5);
    //        Assert.IsTrue(s1 == s2);
    //        Assert.IsTrue( Hashing.isHashEqual(input,s1));

    //        s1 = Hashing.Hash(input,Utils.Hashing.HashingTypes.SHA512);
    //    }
    //}
}
