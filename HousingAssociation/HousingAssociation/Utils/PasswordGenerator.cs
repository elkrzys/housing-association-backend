using System;

namespace HousingAssociation.Utils
{
    public static class PasswordGenerator
    {
        private const string Characters = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";

        public static string CreateRandomPassword(int length = 10)  
        {
            var random = new Random();
            var chars = new char[length];  
            for (var i = 0; i < length; i++)  
            {  
                chars[i] = Characters[random.Next(0, Characters.Length)];  
            }  
            return new string(chars);  
        } 
    }
}