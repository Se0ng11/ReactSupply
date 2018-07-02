//using Microsoft.IdentityModel.Tokens;
//using ReactSupply.Bundles;
//using ReactSupply.Models.Entity;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace ReactSupply.Logic
//{
//    public class JwtTokenLogic:BaseLogic
//    {

//        public string GenerateJwtToken(SettingLogic setting, string userName, string originalRefreshToken, bool isSuper, out string outRefreshToken)
//        {
//            JwtTokenResponse obj = new JwtTokenResponse();

//            try
//            {
//                int accessExpire = Convert.ToInt32(setting.GetAccessExpireInMinute());
//                int refreshExpire = Convert.ToInt32(setting.GetRefreshExpireInMinute());

//                var accessToken = GenerateJwtToken(userName, DateTime.UtcNow.AddMinutes(accessExpire), Bundles.Messages.ACCESS_KEY);
//                JwtSecurityToken refreshToken = null;
//                obj.Token = new JwtSecurityTokenHandler().WriteToken(accessToken);

//                if (string.IsNullOrEmpty(originalRefreshToken))
//                {
//                    refreshToken = GenerateJwtToken(userName, DateTime.UtcNow.AddMinutes(refreshExpire), Bundles.Messages.REFRESH_KEY);
//                    obj.Refresh = new JwtSecurityTokenHandler().WriteToken(refreshToken);
//                }
//                else
//                {
//                    obj.Refresh = originalRefreshToken;
//                }

//                obj.UserId = userName;

//                obj.Role = (isSuper? "SuperAdmin" : "Normal");

//            }
//            catch(Exception ex)
//            {
//                _Logger.Error(ex);
//            }
//            outRefreshToken = obj.Refresh;

//            return Tools.ConvertToJSON(obj);
//        }

      
//        //public string GenerateRefreshToken(string userName)
//        //{
//        //    var refreshToken = GenerateJwtToken(userName, DateTime.UtcNow.AddDays(14), Static.Const.REFRESH_KEY);

//        //    return new JwtSecurityTokenHandler().WriteToken(refreshToken);
//        //}

//        private JwtSecurityToken GenerateJwtToken(string userName, DateTime expires, string key)
//        {
//            JwtSecurityToken token = null;
//            try
//            {
//                var claims = new[]
//                   {
//                        new Claim(JwtRegisteredClaimNames.Sub, userName),
//                        new Claim(JwtRegisteredClaimNames.UniqueName, userName),
//                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                        new Claim(JwtRegisteredClaimNames.Exp, expires.ToString())
//                    };


//                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
//                token = new JwtSecurityToken(
//                        issuer: Messages.COMPANY_WEBADDRESS,
//                        audience: Messages.COMPANY_WEBADDRESS,
//                        expires: expires,
//                        claims: claims,
//                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
//                    );
//            }
//            catch(Exception ex)
//            {
//                _Logger.Error(ex);
//            }
           
//            return token;
//        }
//    }
//}
