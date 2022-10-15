using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    class Hash256
    {
        public static string Compute(string data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder hashString = new StringBuilder();

            foreach (byte _byte in hash)
            {
                hashString.AppendFormat("{0:x2}", _byte);
            }

            return hashString.ToString();
        }
    }
}
