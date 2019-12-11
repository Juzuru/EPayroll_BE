using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Utilities
{
    public class StringGenerationUtility
    {
        private static readonly Random random = new Random();

        public static string GenerateCode()
        {
            string result = "";
            for (int i = 0; i < 6; i++)
            {
                result += alphabet[random.Next() % 36];
            }
            return result;
        }
