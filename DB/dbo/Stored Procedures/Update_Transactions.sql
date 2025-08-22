-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Update_Transactions
	-- Add the parameters for the stored procedure here
	@Id INT,
	@TransactionDate DATETIME,
	@Amount DECIMAL(18,2),
	@PreviousAmount DECIMAL(18,2),
	@AccountId INT,
	@PreviousAccountId INT,
	@CategoryId INT,
	@Note NVARCHAR(1000) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Revert Previous Transaction
	UPDATE Accounts
	SET Balance -= @PreviousAmount
	WHERE Id = @PreviousAccountId

	-- Do New Transaction
	UPDATE Accounts
	SET Balance += @Amount
	WHERE Id = @AccountId

	-- Update Transaction
	UPDATE Transactions
	SET TransactionDate = @TransactionDate, Amount = ABS(@Amount), Note = @Note, 
	AccountId = @AccountId, CategoryId = @CategoryId
	WHERE Id = @Id
			
END
