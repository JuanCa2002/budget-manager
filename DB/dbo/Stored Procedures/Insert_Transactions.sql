-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Insert_Transactions
	-- Add the parameters for the stored procedure here
	@UserId INT,
	@TransactionDate DATETIME,
	@Amount DECIMAL(18,2),
	@CategoryId INT,
	@AccountId INT,
	@Note NVARCHAR(1000) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Transactions ([UserId], TransactionDate, Amount, Note, AccountId, CategoryId)
	VALUES (@UserId, @TransactionDate, ABS(@Amount), @Note, @AccountId, @CategoryId);

	UPDATE Accounts
	SET Balance += @Amount
	WHERE Id = @AccountId;

	SELECT SCOPE_IDENTITY();
END
