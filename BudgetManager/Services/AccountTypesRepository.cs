using BudgetManager.Models.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManager.Services
{
    public class AccountTypesRepository: IAccountTypesRepository
    {
        private readonly string? connectionString;
        public AccountTypesRepository(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Save(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                             ("Insert_AccountTypes", 
                                             new {userId = accountType.UserId, 
                                                 name = accountType.Name},
                                             commandType: System.Data.CommandType.StoredProcedure);
            accountType.Id = id;
        }

        public async Task<bool> ExistByNameAndUser(string name, int userId, int id = 0)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>
                                                (@"SELECT 1
                                                FROM AccountTypes [AT]
                                                WHERE [AT].[Name] = @Name AND
                                                [AT].UserId = @UserId AND
                                                [AT].Id <> @Id;",
                                                new {name, userId, id});
            return exist == 1;
        }

        public async Task<IEnumerable<AccountType>> GetAll(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountType>
                                                    (@"SELECT Id, Name, [Order]
                                                    FROM AccountTypes [AT]
                                                    WHERE [AT].UserId = @UserId
                                                    ORDER BY [Order];", new {userId});
        }

        public async Task Update(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes 
                                            SET [Name] = @Name
                                            WHERE Id = @Id;", accountType);
        }

        public async Task<AccountType?> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>
                                                    (@"SELECT Id, [Name], [Order]
                                                        FROM AccountTypes
                                                        WHERE Id = @Id AND
                                                        UserId = @UserId;", new {id, userId});
        }

        public async Task DeleteById(int id, int userId)
        {
           using var connection = new SqlConnection(connectionString);
           await connection.ExecuteAsync(@"DELETE AccountTypes
                                        WHERE Id = @Id 
                                        AND [UserId] = @UserId", new {id, userId});
        }

        public async Task Sort(IEnumerable<AccountType> accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes
                                            SET [Order] = @Order
                                            WHERE Id = @Id;", accountTypes);

        }
    }
}
