using AuthAppDemoDB.Models;
using AuthAppDemoService.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    internal class Authorize : IAuthorize
    {
        private readonly JwtInfo jwtInfo;

        public Authorize(JwtInfo jwtInfo)
        {
            this.jwtInfo = jwtInfo;
        }

        public string CreateAccessToken(TokenInfoDto param) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtInfo.Issuer,
                Audience = jwtInfo.Audience,
                Expires = DateTime.UtcNow.AddMinutes(jwtInfo.Lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SigningKey))
                                        , SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.NameIdentifier,param.UserId),
                    new Claim(ClaimTypes.Name,param.UserName)
            })


            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            var encryptedAccessToken = Encryption.AIS_Encryption(accessToken, jwtInfo.SigningKey);

            return encryptedAccessToken;
        }

        public string RefreshAccessToken()
        {
            throw new NotImplementedException();
        }
    }
}
