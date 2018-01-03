using System;
using System.Runtime.InteropServices;
using System.Security;

namespace liteFTP.Helpers
{
    public static class SecureStringHelper
    {
        public static string ToStandardString(this SecureString secureString)
        {
            if (secureString == null)
            {
                return string.Empty;
            }

            var unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
