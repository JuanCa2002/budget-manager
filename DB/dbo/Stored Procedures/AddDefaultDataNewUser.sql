-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AddDefaultDataNewUser
	-- Add the parameters for the stored procedure here
	@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert Default Account Types

	DECLARE @Cash NVARCHAR(50) = 'Efectivo';
	DECLARE @Credit NVARCHAR(50) = 'Credito';
	DECLARE @Cards NVARCHAR(50) = 'Tarjetas';

	INSERT INTO AccountTypes ([Name], [UserId], [Order])
	VALUES 
	(@Cash, @UserId, 1),
	(@Credit, @UserId, 2),
	(@Cards, @UserId, 3);

	-- Insert Default Accounts

	INSERT INTO Accounts ([Name], Balance, AccountTypeId)
	SELECT [Name], 0, Id
	FROM AccountTypes
	WHERE UserId = @UserId;

    -- Insert Default Categories

    INSERT INTO Categories([Name], TransactionTypeId, [UserId])
	VALUES 
	('Arriendo', 2, @UserId),
	('Salario', 1, @UserId),
	('Comida', 2, @UserId),
	('Ropa', 2, @UserId)

END
