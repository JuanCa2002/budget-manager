-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Delete_Transactions] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Declaring Variables
	DECLARE @Amount DECIMAL(18,2);
	DECLARE @AccountId INT;
	DECLARE @TransactionTypeId INT;

	-- Setting Variables From the transaction to delete
	SELECT @Amount = T.Amount, @AccountId = T.AccountId, @TransactionTypeId = C.TransactionTypeId
	FROM Transactions T
	INNER JOIN Categories C ON C.ID = T.CategoryId
	WHERE T.Id = @Id
    AND T.UserId = @UserId;

	-- Declare Multiplicative Factor
	DECLARE @MultiplicativeFactor INT = 1;

	-- Verify Transaction Type
	IF(@TransactionTypeId = 2)
		SET @MultiplicativeFactor = - 1;
    
	-- Calculate Amount
	SET @Amount *= @MultiplicativeFactor;

	-- Reverse Transaction 
	UPDATE Accounts
	SET Balance -= @Amount
	WHERE Id = @AccountId;

	-- Delete Transaction
	DELETE Transactions
	WHERE Id = @Id 
	AND [UserId] = @UserId;
END
