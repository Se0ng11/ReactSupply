using Microsoft.IdentityModel.Tokens;
using ReactSupply.Models.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactSupply.Logic
{
    public class JwtTokenLogic:BaseLogic
    {

        public string GenerateJwtToken(string userName, string originalRefreshToken, out string outRefreshToken)
        {
            JwtTokenResponse obj = new JwtTokenResponse();

            try
            {
                var accessToken = GenerateJwtToken(userName, DateTime.UtcNow.AddMinutes(15), Static.Const.ACCESS_KEY);
                JwtSecurityToken refreshToken = null;
                obj.Token = new JwtSecurityTokenHandler().WriteToken(accessToken);

                if (string.IsNullOrEmpty(originalRefreshToken))
                {
                    refreshToken = GenerateJwtToken(userName, DateTime.UtcNow.AddDays(14), Static.Const.REFRESH_KEY);
                    obj.Refresh = new JwtSecurityTokenHandler().WriteToken(refreshToken);
                }
                else
                {
                    obj.Refresh = originalRefreshToken;
                }

                obj.UserId = userName;

            }
            catch(Exception ex)
            {

            }
            outRefreshToken = obj.Refresh;

            return ConvertToJSON(obj);
        }

        //public string GenerateRefreshToken(string userName)
        //{
        //    var refreshToken = GenerateJwtToken(userName, DateTime.UtcNow.AddDays(14), Static.Const.REFRESH_KEY);

        //    return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        //}

        private JwtSecurityToken GenerateJwtToken(string userName, DateTime expires, string key)
        {
            JwtSecurityToken token = null;
            try
            {
                var claims = new[]
                   {
                        new Claim(JwtRegisteredClaimNames.Sub, userName),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Exp, expires.ToString())
                    };


                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                token = new JwtSecurityToken(
                        issuer: Static.Const.COMPANY_WEBADDRESS,
                        audience: Static.Const.COMPANY_WEBADDRESS,
                        expires: expires,
                        claims: claims,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
            }
            catch(Exception ex)
            {

            }
           
            return token;
        }
    }
}
