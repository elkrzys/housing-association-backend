using System;
using System.Security.Cryptography;

namespace HousingAssociation.Utils
{
    public static class HashGenerator
    {
        public static string CalculateMd5StringFromBytes(byte[] file)
        {
            using var md5Hash = MD5.Create();
            var hashBytes = md5Hash.ComputeHash(file);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }
}