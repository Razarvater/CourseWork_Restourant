CREATE PROC GetOpenedOrderList
AS
BEGIN
	SELECT * FROM [dbo].[OpenedOrder];
END

CREATE PROC CreateOpenedOrder
	@EmpID int
AS
BEGIN
	INSERT INTO [dbo].[OpenedOrder] VALUES (@EmpID, GETDATE(), '',0,0);
END