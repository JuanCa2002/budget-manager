using BudgetManager.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManager.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly string? connectionString;
        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Create(User user)
        {
            using var connection = new SqlConnection(connectionString);
            int userId = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Users ([Name], LastName, Email, StandardEmail, PasswordHash)
                VALUES (@Name, @LastName, @Email, @StandardEmail, @PasswordHash);
                SELECT SCOPE_IDENTITY();", user);

            await connection.ExecuteAsync("AddDefaultDataNewUser", new { userId },
                commandType: System.Data.CommandType.StoredProcedure);

            return userId;
        }

        public async Task<User?> GetByEmail(string standardEmail)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>
                            (@"SELECT *
                            FROM Users
                            WHERE StandardEmail = @StandardEmail;", new { standardEmail });
        }

        public async Task Update(User user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                SET PasswordHash = @PasswordHash
                                WHERE Id = @Id;", user);
        }
    }
}
