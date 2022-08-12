using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace AuctionMarketplaceLibrary {
    public static class JwtParser {
        /// <summary>
        /// Метод для получения claims
        /// </summary>
        /// <param name="token">Jwt токен</param>
        /// <param name="type">Тип токена</param>
        /// <returns>Кортеж вида (login, role)</returns>
        public static (string, string) GetClaims(string token, Authenticator.TokenType type) {
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ValidateToken(token, 
                Authenticator.GetTokenValidationParameters(type),
                out SecurityToken validatedToken).Claims.ToList();

            return (claims[0].Value, claims[1].Value);
        }

        /// <summary>
        /// Метод для получения количества минут до истечения срока жизни jwt токена
        /// </summary>
        /// <param name="token">Jwt токен</param>
        /// <param name="type">Тип токена</param>
        /// <returns>Количество минут до истечения срока жизни jwt токена</returns>
        public static double GetMinutesBeforeExpiration(string token, Authenticator.TokenType type) {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, Authenticator.GetTokenValidationParameters(type), out SecurityToken validatedToken);
            return (validatedToken.ValidTo.ToUniversalTime() - DateTime.Now.ToUniversalTime()).TotalMinutes;
        }
    }
}