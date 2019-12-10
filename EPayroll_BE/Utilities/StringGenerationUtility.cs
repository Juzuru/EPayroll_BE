using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Utilities
{
    public class StringGenerationUtility
    {
        private static readonly Random random = new Random();
        private static readonly string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        
        public static string GenerateCode()
        {
            return alphabet[random.Next() % 36] + alphabet[random.Next() % 36] + alphabet[random.Next() % 36];
        }
    }
}
