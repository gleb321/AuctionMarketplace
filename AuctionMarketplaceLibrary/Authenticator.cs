using System;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AuctionMarketplaceLibrary {
    public static class Authenticator {
        private const string Issuer = "AuthenticationServer";
        private const string Audience = "Client";
        public const int AccessTokenLifetime = 15;
        public const int RefreshTokenLifetime = 120;
        public enum TokenType {
            Access,
            Refresh
        }
        private static SecurityKey GetSecurityKey(string signature) {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signature));
        }
        
        public static TokenValidationParameters GetTokenValidationParameters(TokenType type) {
            return new TokenValidationParameters {
                ValidateIssuer = true, 
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                IssuerSigningKey = GetSecurityKey(type == TokenType.Access ? Config.AccessSignature : Config.RefreshSignature),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        }
        
        /// <summary>
        ///  Метод для создания нового jwt токена
        /// </summary>
        /// <param name="lifetime">Время жизни токена в минутах</param>
        /// <param name="type">Тип токена</param>
        /// <param name="login">Email пользователя</param>
        /// <param name="role">Роль пользователя(admin/user)</param>
        /// <returns>Строковое представление jwt токена</returns>
        public static string CreateToken(double lifetime, TokenType type, string login, string role) {
            var jwt = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: GetIdentity(login, role).Claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.AddMinutes(lifetime).ToUniversalTime(),
                signingCredentials: new SigningCredentials(
                    GetSecurityKey(type == TokenType.Access ? Config.AccessSignature : Config.RefreshSignature), 
                    SecurityAlgorithms.HmacSha256)
                );
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private static ClaimsIdentity GetIdentity(string login, string role) {
            var claims = new List<Claim> {
                new (ClaimsIdentity.DefaultNameClaimType, login),
                new (ClaimsIdentity.DefaultRoleClaimType, role)
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", 
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            return claimsIdentity;
        }
    }
}