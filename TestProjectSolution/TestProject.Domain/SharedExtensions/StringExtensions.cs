using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace TestProject.Domain.SharedExtensions
{
    public static class StringExtensions
    {
        public static Uri ToUri(this string text)
        {
            return new Uri(text);
        }

        public static string ToSha256Hash(this string text)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                    
                return builder.ToString();
            }
        }

        public static string ToHmac(this string text, string key)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var keyBytes = HexDecode(key);

            byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }

    }
}
