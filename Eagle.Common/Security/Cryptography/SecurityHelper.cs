using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Eagle.Common.Security.Cryptography
{
    /// <summary>
    /// Container for extension methods. 
    /// </summary>
    public static class SecurityHelper
    {
        #region SecureString
        public static SecureString ToSecureString(this string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        ////#ref http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx
        public static string ToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        #endregion
    }
}
