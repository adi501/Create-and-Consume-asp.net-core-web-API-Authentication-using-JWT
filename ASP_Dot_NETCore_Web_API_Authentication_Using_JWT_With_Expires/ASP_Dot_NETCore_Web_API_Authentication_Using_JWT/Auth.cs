using ASP_Dot_NETCore_Web_API_Authentication_Using_JWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASP_Dot_NETCore_Web_API_Authentication_Using_JWT
{
    public class Auth : IJwtAuth
    {
        private readonly string userName = "adi";
        private readonly string userPassword = "password";
        private readonly string key;
        public Auth(string key)
        {
            this.key = key;
        }
        public object Authentication(string username, string password)
        {
            if (!(userName.Equals(username) || userPassword.Equals(password)))
            {
                return null;
            }

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            TokenResponce obj = new TokenResponce();
            obj.Issuer = token.Issuer;
            obj.Token = tokenHandler.WriteToken(token);
            obj.ValidFrom = token.ValidFrom;
            obj.ValidTo = token.ValidTo;

            // 5. Return Token from method
            // return tokenHandler.WriteToken(token);
            return JsonConvert.SerializeObject(obj);
        }
    }
}
