using System;
using System.Text;
using System.Security.Cryptography;

namespace AuctionMarketplaceLibrary {
    public static class Cryptographer {
        /// <summary>
        /// Метод для хэширования пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Хэшированный пароль</returns>
        public static string HashPassword(string password) {
            using (var sha256 = SHA256.Create()) {
                password = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            return password;
        }
    }
}