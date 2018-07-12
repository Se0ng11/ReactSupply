using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using ReactSupply.Logic;
using ReactSupply.Models.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactSupply.Utils
{
    public static class Tools
    {
        private static Logger _Logger { get; set; } = LogManager.GetCurrentClassLogger();

        public static string ConvertToJSON(object result)
        {
            return JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string GenerateJwtToken(SettingLogic setting, string userName, string role, string originalRefreshToken, bool isSuper, out string outRefreshToken)
        {
            JwtTokenResponse obj = new JwtTokenResponse();

            try
            {
                int accessExpire = Convert.ToInt32(setting.GetSettingValueInString("AccessExpire"));
                int refreshExpire = Convert.ToInt32(setting.GetSettingValueInString("RefreshExpire"));

                var accessToken = GenerateJwtToken(userName, DateTime.UtcNow.AddMinutes(accessExpire), Utils.Messages.ACCESS_KEY);
                JwtSecurityToken refreshToken = null;
                obj.Token = new JwtSecurityTokenHandler().WriteToken(accessToken);

                if (string.IsNullOrEmpty(originalRefreshToken))
                {
                    refreshToken = GenerateJwtToken(userName, DateTime.UtcNow.AddMinutes(refreshExpire), Utils.Messages.REFRESH_KEY);
                    obj.Refresh = new JwtSecurityTokenHandler().WriteToken(refreshToken);
                }
                else
                {
                    obj.Refresh = originalRefreshToken;
                }

                obj.UserId = userName;

                obj.Role = (isSuper ? "SuperAdmin" : role);

            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            outRefreshToken = obj.Refresh;

            return ConvertToJSON(obj);
        }

        public static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }


        private static JwtSecurityToken GenerateJwtToken(string userName, DateTime expires, string key)
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
                        issuer: Messages.COMPANY_WEBADDRESS,
                        audience: Messages.COMPANY_WEBADDRESS,
                        expires: expires,
                        claims: claims,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return token;
        }


    }
}
