using System.Collections.Generic;
using Dapper;
using AdvancePharmacyPOS.Data;
using AdvancePharmacyPOS.Models;

namespace AdvancePharmacyPOS.Data.Repositories
{
    public class UsersRepository
    {
        public IEnumerable<User> GetAll()
        {
            using (var conn = Database.GetOpenConnection())
            {
                return conn.Query<User>("SELECT UserId, Username, FullName, Role, IsActive FROM Users");
            }
        }

        public int Create(User user)
        {
            const string sql = @"INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Role, IsActive)
VALUES (@Username, @PasswordHash, @PasswordSalt, @FullName, @Role, @IsActive);
SELECT CAST(SCOPE_IDENTITY() as int);";
            using (var conn = Database.GetOpenConnection())
            {
                return conn.ExecuteScalar<int>(sql, user);
            }
        }
    }
}