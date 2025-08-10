using System;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using AdvancePharmacyPOS.Data;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.Services
{
    public class AuthService
    {
        public User ValidateLogin(string username, string password)
        {
            using (var conn = Database.GetOpenConnection())
            {
                var user = conn.QueryFirstOrDefault<User>("SELECT TOP 1 UserId, Username, PasswordHash, PasswordSalt, FullName, Role, IsActive FROM Users WHERE Username=@u AND IsActive=1", new { u = username });
                if (user == null) return null;
                var hash = HashPassword(password, user.PasswordSalt);
                if (!string.Equals(hash, user.PasswordHash, StringComparison.Ordinal)) return null;
                return user;
            }
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        public static string GenerateSalt()
        {
            var rng = new byte[16];
            using (var r = RandomNumberGenerator.Create())
            {
                r.GetBytes(rng);
            }
            return BitConverter.ToString(rng).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}