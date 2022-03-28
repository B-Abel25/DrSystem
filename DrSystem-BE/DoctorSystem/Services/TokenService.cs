using DoctorSystem.Entities;
using DoctorSystem.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoctorSystem.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(Doctor doctor)
        {
            List<Claim> claim = new List<Claim>
            {
                new Claim("userNumber",doctor.SealNumber) //userNumber = sealNumber, csak a kiolvasás miatt kell azonos név
            };

            SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string CreateToken(Client client)
        {
            List<Claim> claim = new List<Claim>
            {
                new Claim("userNumber",client.MedNumber) //userNumber = medNumber, csak a kiolvasás miatt kell azonos név
            };

            SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string ReadToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokenBody =  tokenHandler.ReadJwtToken(token.Replace("Bearer ",""));
            return tokenBody.Payload["userNumber"].ToString();
        }
        /*
        public string SetExpireDateToken(string token, int minutes = 60)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.TokenLifetimeInMinutes = minutes;

            tokenHandler.WriteToken(tokenHandler.CreateToken());
        }
        */
    }
}
