using System.Security.Cryptography;

namespace WithSecure.Interview.Api.Helper
{
    public static class SecurityHelper
    {
        public static byte[] CalculateSHA1(byte[] fileInByteArray)
        {
            if (fileInByteArray != null && fileInByteArray.Length > 0)
            {
                SHA1 sha1Hash = SHA1.Create();
                var hashedVaule = sha1Hash.ComputeHash(fileInByteArray);
                return hashedVaule;
            }
            else
            {
                throw new Exception("ByteArray for calculate SHA1 can not be null.");
            }
        }
    }
}
