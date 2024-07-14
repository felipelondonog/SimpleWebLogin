using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebLogin.Services
{
    public static class ServiceUtilities
    {
        public static string ConvertSHA256(string text)
        {
            string hash = string.Empty;

            using(SHA256 sha256 = SHA256.Create())
            {
                // Get hash from received text
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convert bytes array to string
                foreach(byte b in hashValue)
                    hash += $"{b:X2}";
            }

            return hash;
        }

        public static string CreateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}