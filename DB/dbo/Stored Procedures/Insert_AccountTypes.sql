-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Insert_AccountTypes
	-- Add the parameters for the stored procedure here
	@Name NVARCHAR(50),
	@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @Order INT;
	SELECT @Order = COALESCE(MAX([Order]), 0) + 1
	FROM AccountTypes
	WHERE [UserId] = @UserId 

	INSERT INTO AccountTypes ([Name], [UserId], [Order])
	VALUES (@Name, @UserId, @Order);

	SELECT SCOPE_IDENTITY();
END
