using BudgetManager.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace BudgetManager.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string? connectionString;
        public AccountRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Accounts
                                         WHERE Id = @Id;", new { id });
        }

        public async Task<IEnumerable<Account>> GetAll(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Account>
                                    (@"SELECT A.Id, A.[Name], A.Balance, [AT].[Name] AS AccountType
                                    FROM Accounts A
                                    INNER JOIN AccountTypes [AT] ON [AT].Id = A.AccountTypeId
                                    WHERE [AT].UserId = @UserId
                                    ORDER BY [AT].[Order]", new {userId});
        }

        public async Task<Account?> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>
                                   (@"SELECT A.Id, A.[Name], A.Balance, A.AccountTypeId, A.Description
                                    FROM Accounts A
                                    INNER JOIN AccountTypes [AT] ON [AT].Id = A.AccountTypeId
                                    WHERE [AT].UserId = @UserId 
                                    AND A.Id = @Id", new { userId, id });
        }

        public async Task Save(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                            (@"INSERT INTO Accounts ([Name], AccountTypeId, [Description], Balance)
                            VALUES (@Name, @AccountTypeId, @Description, @Balance);
                            SELECT SCOPE_IDENTITY();", account);
            account.Id = id;
        }

        public async Task Update(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                                            SET [Name] = @Name, [Description] = @Description, 
                                            [AccountTypeId] = @AccountTypeId, [Balance] = @Balance
                                            WHERE Id = @Id", account);
        }
    }
}
