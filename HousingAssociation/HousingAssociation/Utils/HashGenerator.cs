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

        public static string GetHashIfNotEqual(this byte[] file, string hash)
        {
            var fileHash = CalculateMd5StringFromBytes(file);
            return fileHash != hash ? fileHash : null;
        }
    }
}