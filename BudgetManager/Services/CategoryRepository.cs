using BudgetManager.Models.Entities;
using BudgetManager.Models.Enums;
using BudgetManager.Models.Filters;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManager.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string? connectionString;
        public CategoryRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Count(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>
                                    (@"SELECT COUNT(*)
                                    FROM Categories
                                    WHERE [UserId] = @UserId;", new { userId });
        }

        public async Task Create(Category category)
        {
           using var connection = new SqlConnection(connectionString);
           var id = await connection.QuerySingleAsync<int>
                 (@"INSERT INTO Categories ([Name], TransactionTypeId, [UserId])
                    VALUES (@Name, @TransactionTypeId, @UserId);
                    SELECT SCOPE_IDENTITY()", category);
           category.Id = id;
        }

        public async Task Delete(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Categories
                                            WHERE Id = @Id
                                            AND [UserId] = @UserId;", new { id, userId });
        }

        public async Task<IEnumerable<Category>> GetAll(int userId, PaginationFilter paginationFilter)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>(@$"SELECT *
                                                    FROM Categories
                                                    WHERE [UserId] = @UserId
                                                    ORDER BY Id
                                                    OFFSET {paginationFilter.Skip} ROWS
                                                    FETCH NEXT {paginationFilter.RowsPerPage} ROWS ONLY", new { userId });
        }

        public async Task<IEnumerable<Category>> GetAllByTransactionType(int userId, TransactionType transactionType)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>
                                    (@"SELECT *
                                    FROM Categories
                                    WHERE [UserId] = @UserId
                                    AND TransactionTypeId = @TransactionType;", new { userId, transactionType });
        }

        public async Task<Category?> GetById(int id, int userId)
        {
            using var connection = new SqlConnection (connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>
                                            (@"SELECT *
                                            FROM Categories
                                            WHERE Id = @Id AND [UserId] = @UserId;", new { id, userId });

        }

        public async Task Update(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                        (@"UPDATE Categories 
                        SET [Name] = @Name, TransactionTypeId = @TransactionTypeId
                        WHERE Id = @Id", category);
        }
    }
}
