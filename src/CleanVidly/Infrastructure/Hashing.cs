using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CleanVidly.Infrastructure
{
    public static class Hashing
    {
        public static byte[] CreateHash(byte[] salt, string valueToHash)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
            }
        }

        public static byte[] GenerateSalt()
        {
            using (var hmac = new HMACSHA512())
            {
                return hmac.Key;
            }
        }

        public static bool VerifyHash(string password, byte[] salt, byte[] actualPassword)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(actualPassword);
            }
        }
    }
}