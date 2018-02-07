using System;
using System.Security.Cryptography;
using System.Text;

namespace Eagle.Common.Security.Cryptography
{
    public static class RandomText
    {
        /// <summary>
        /// Generates an 4 letter random text.
        /// </summary>
        public static string Generate()
        {
            // Generate random text
            string s = "";
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            int lenght = RNG.Next(4, 6);
            for (int i = 0; i < lenght; i++)
            {
                var index = RNG.Next(chars.Length - 1);
                s += chars[index].ToString();
            }
            return s;
        }

        public static string GenerateGuidToShortString(int length = 6)
        {
            var randomData = new byte[length];
            RNGCryptoServiceProvider randomGenerator = new RNGCryptoServiceProvider();
            randomGenerator.GetNonZeroBytes(randomData);
            char[] validCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789".ToCharArray();
            int counter = 0;

            var result = new StringBuilder(length);
            foreach (var value in randomData)
            {
                counter = (counter + value) % (validCharacters.Length - 1);
                result.Append(validCharacters[counter]);
            }
            return result.ToString();
        }
        
        /// <summary>
        /// Generate shorter string from the uniqueness of a GUID : 3c4ebc5f5f2c4edc
        /// </summary>
        /// <returns></returns>
        public static string GuidToShortString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= b + 1;
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static long GuidToInt64()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Guid
        /// </summary>
        /// <returns></returns>
        public static string GuidToString()
        {
            return Guid.NewGuid().ToString();
        }

        public static string Generate(int minLength, int maxLength)
        {
            // Generate random text
            string s = "";
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            int lenght = RNG.Next(minLength, maxLength);
            for (int i = 0; i < lenght; i++)
            {
                var index = RNG.Next(chars.Length - 1);
                s += chars[index].ToString();
            }
            return s;
        }

        /// <summary>
        /// This method gets 4 random numbers, puts them in a 4 byte array and then base64’s the array. 
        ///  </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Base64Hash(int size=4)
        {
            Random random = new Random();
            byte[] buffer = new byte[size];
            random.NextBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// This displays the id in the format xx-xx. 
        /// It’s easy to read out but is limited to 1/255 x 1/255 = 1/65025 before you get a clash.
        /// </summary>
        /// <returns></returns>
        public static string NumberHash()
        {
            Random random = new Random();
            byte[] buffer = new byte[2];
            random.NextBytes(buffer);
            return string.Format("{0:00}-{1:00}", buffer[0], buffer[1]);
        }

        /// <summary>
        /// This generates a 4 alpha-numerical id, each character is based on a random number between 1 and 36 
        /// (10 digits, 26 lowercase letters) which is then used to grab the corresponding index. Example output : v5m0
        /// from the character array. Example output : cEPypg==
        /// </summary>
        /// <returns></returns>
        public static string AlphaNumeric()
        {
            string s = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(1, 36);
                builder.Append(s[index]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// This is targetted specifically at the id generation ala Tinyurl.com. It uses the .NET built in GetHashCode() method on the string version of the url. The resulting number is then displayed in hex format to make it short
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string UriHashCode(Uri uri)
        {
            // e.g. Uri uri = new Uri("http://www.yetanotherchris.info/csharp/example");
            // gives eaafc653
            int hashcode = uri.GetHashCode();
            return $"{hashcode:X}".ToLower();
        }

        /// <summary>
        /// This takes the current seconds and milliseconds, and displays them as two hex values side by side.
        ///  It’s relatively safe for uniqueness except for very large amounts of concurrent ids being generated,
        ///  which obviously these id methods aren’t aimed at solving. It suffers from having a possibility of 
        /// a clash from 2 times on the same day.
        /// </summary>
        /// <returns></returns>
        public static string TimeToHexString()
        {
            long ms = DateTime.Now.Second;
            long ms2 = DateTime.Now.Millisecond;
            return string.Format("{0:X}{1:X}", ms, ms2).ToLower();
        }

        /// <summary>
        ///This uses the DateTime.Now.Ticks property, which is “the number of 100-nanosecond intervals that have 
        /// elapsed since 12:00:00 midnight, January 1, 0001”. 
        /// It will therefore always be unique, unless the id is generated in a threaded scenario. 
        /// Example output : 8cb46e251f0f610
        /// </summary>
        /// <returns></returns>
        public static string TicksToString()
        {
            long ticks = DateTime.Now.Ticks;
            return string.Format("{0:X}", ticks).ToLower();
        }

        public static string RandomMd5()
        {
            Random _random = new Random();
            byte[] buffer = new byte[16];
            _random.NextBytes(buffer);

            MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(buffer);
            StringBuilder builder = new StringBuilder();

            foreach (byte t in output)
                builder.AppendFormat("{0:x2}", t);

            return builder.ToString();
        }

        public static string Base62Random()
        {
            Random random = new Random();
            int randomNext = random.Next();
            return Base62ToString(randomNext);
        }

        private static string Base62ToString(long value)
        {
            // Divides the number by 64, so how many 64s are in
            // 'value'. This number is stored in Y.
            // e.g #1
            // 1) 1000 / 62 = 16, plus 8 remainder (stored in x).
            // 2) 16 / 62 = 0, remainder 16
            // 3) 16, 8 or G8:
            // 4) 65 is A, add 6 to this = 71 or G.
            //
            // e.g #2:
            // 1) 10000 / 62 = 161, remainder 18
            // 2) 161 / 62 = 2, remainder 37
            // 3) 2 / 62 = 0, remainder 2
            // 4) 2, 37, 18, or 2,b,I:
            // 5) 65 is A, add 27 to this (minus 10 from 37 as these are digits) = 92.
            //    Add 6 to 92, as 91-96 are symbols. 98 is b.
            // 6)
            long x = 0;
            long y = Math.DivRem(value, 62, out x);
            if (y > 0)
                return Base62ToString(y) + ValToChar(x).ToString();
            else
                return ValToChar(x).ToString();
        }

        private static char ValToChar(long value)
        {
            if (value > 9)
            {
                int ascii = (65 + ((int)value - 10));
                if (ascii > 90)
                    ascii += 6;
                return (char)ascii;
            }
            else
                return value.ToString()[0];
        }

    }
}
