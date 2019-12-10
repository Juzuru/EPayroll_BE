using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EPayroll_BE.Utilities
{
    public static class JWTUtility
    {
        private static string jwtKey = "Project_Employee_Payroll_Management_API_SWD";
        private static List<string> validateAudiences;
        public static TokenValidationParameters tokenValidationParameters;

        static JWTUtility()
        {
            validateAudiences = new List<string>();
            tokenValidationParameters = new TokenValidationParameters
            {
                //ValidIssuer = Configuration["JwtIssuer"],
                //ValidAudience = Configuration["JwtIssuer"],
                ValidAudiences = validateAudiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero, // remove delay of token when expire

                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = true
            };
        }

        public static string GenerateJwtToken(string userUID, Claim[] claims)
        {
            DateTime datetime = DateTime.Now;
            validateAudiences.Add(userUID + "_" + datetime);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = userUID + "_" + datetime,
                Expires = datetime.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            }));
        }

        public static string GetClaimValueFromToken(string claim, string token)
        {
            return ReadToken(token).Claims.First(_ => _.Type.Equals(claim)).Value;
        }

        public static void RemoveAudien(string token)
        {
            validateAudiences.Remove(ReadToken(token).Audiences.First());
        }

        private static JwtSecurityToken ReadToken(string token)
        {
            try
            {
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                return new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
