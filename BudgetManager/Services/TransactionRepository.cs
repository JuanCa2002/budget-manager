using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManager.Services
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly string? connectionString;
        public TransactionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Delete(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Delete_Transactions", new {id, userId},
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Transaction>> GetByAccount(TransactionsByAccountFilter filter)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>
                       (@"SELECT T.Id, T.Amount, T.TransactionDate, 
                        C.[Name] as Category, A.[Name] as Account, C.TransactionTypeId
                        FROM Transactions T
                        INNER JOIN Categories C ON C.ID = T.CategoryId
                        INNER JOIN Accounts A ON A.Id = T.AccountId
                        WHERE T.[UserId] = @UserId
                        AND T.AccountId = @AccountId
                        AND T.TransactionDate BETWEEN @InitialDate AND @FinalDate;", filter);
        }

        public async Task<Transaction?> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaction>
                                    (@"SELECT T.*, C.TransactionTypeId
                                    FROM Transactions T
                                    INNER JOIN Categories C ON C.ID = T.CategoryId
                                    WHERE T.Id = @Id 
                                    AND T.UserId = @UserId", new { id, userId });
        }

        public async Task<IEnumerable<TransactionByMonth>> GetByMonth(int userId, int year)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TransactionByMonth>
                (@"SELECT MONTH(T.TransactionDate) AS Month, SUM(Amount) as Amount,
                C.TransactionTypeId
                FROM Transactions T
                INNER JOIN Categories C ON C.ID = T.CategoryId
                WHERE T.UserId = @UserId AND YEAR(T.TransactionDate) = @Year
                GROUP BY MONTH(T.TransactionDate), C.TransactionTypeId;", new { userId, year });
        }

        public async Task<IEnumerable<Transaction>> GetByUser(TransactionByUserFilter filter)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>
                        (@"SELECT T.Id, T.Amount, T.TransactionDate, T.Note,
                        C.[Name] as Category, A.[Name] as Account, C.TransactionTypeId
                        FROM Transactions T
                        INNER JOIN Categories C ON C.ID = T.CategoryId
                        INNER JOIN Accounts A ON A.Id = T.AccountId
                        WHERE T.[UserId] = @UserId
                        AND T.TransactionDate BETWEEN @InitialDate AND @FinalDate
                        ORDER BY T.TransactionDate DESC;", filter);
        }

        public async Task<IEnumerable<TransactionByWeek>> GetByWeek(TransactionByUserFilter filter)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TransactionByWeek>
                        (@"SELECT DATEDIFF(d, @initialDate, TransactionDate) / 7 + 1 AS Week,
                        SUM(Amount) as Amount, C.TransactionTypeId
                        FROM Transactions T
                        INNER JOIN Categories C ON C.ID = T.CategoryId
                        WHERE T.UserId = @UserId
                        AND TransactionDate BETWEEN @initialDate and @finalDate
                        GROUP BY DATEDIFF(d, @initialDate, TransactionDate) / 7, C.TransactionTypeId", filter);

        }

        public async Task Save(Transaction transaction)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                ("Insert_Transactions", new 
                { 
                    transaction.UserId, 
                    transaction.TransactionDate,
                    transaction.Amount, 
                    transaction.CategoryId, 
                    transaction.AccountId, 
                    transaction.Note
                },
                commandType: System.Data.CommandType.StoredProcedure);
            transaction.Id = id;
        }

        public async Task Update(Transaction transaction, decimal previousAmount, int previousAccountId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Update_Transactions", new
            {
                transaction.Id,
                transaction.TransactionDate,
                transaction.Amount,
                previousAmount,
                transaction.AccountId,
                previousAccountId,
                transaction.CategoryId,
                transaction.Note
            },
            commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
