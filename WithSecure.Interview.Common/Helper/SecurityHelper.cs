using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WithSecure.Interview.Common.Helper
{
    public static class SecurityHelper
    {
        public static byte[] CalculateSHA1(byte[] fileInByteArray)
        {
            SHA1 sha1Hash = SHA1.Create();
            var hashedVaule = sha1Hash.ComputeHash(fileInByteArray);
            return hashedVaule;
        }
    }
}
