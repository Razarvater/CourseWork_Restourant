--Процедуры
CREATE PROCEDURE AddNewPage
AS
BEGIN
	INSERT INTO [dbo].[PagesList]([PageName]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPageId;
END
--разделитель|
CREATE PROCEDURE UpdatePage
	@id int,
	@NewName nvarchar(50)
AS
BEGIN 
	UPDATE [dbo].[PagesList] SET [PageName]=@NewName WHERE [ID] = @id;
END
--разделитель|
CREATE PROCEDURE GetPagesList
AS
BEGIN
	SELECT * FROM PagesList ORDER BY [ID] ASC
END
--разделитель|
CREATE PROCEDURE GetRolesList
AS
BEGIN
	SELECT * FROM [dbo].[Roles] ORDER BY [ID] ASC
END
--разделитель|
CREATE PROCEDURE GetRolesPagesList
AS
BEGIN
	SELECT * FROM [dbo].[RightRole]
END
--разделитель|
CREATE PROCEDURE DeletePage
	@id int
AS
BEGIN
	DELETE FROM [dbo].[PagesList] WHERE [ID] = @id
END
--разделитель|
CREATE PROCEDURE ChangeRight
	@rid int,
	@pid int,
	@can bit
AS
BEGIN
	UPDATE [dbo].[RightRole] SET [IsCan] = @can WHERE [Role_ID] = @rid AND [Page_ID] = @pid
END 
--разделитель|
CREATE PROCEDURE ChangeRoleName
	@id int,
	@roleName nvarchar(50)
AS
BEGIN
	UPDATE [dbo].[Roles] SET [RoleName] = @roleName WHERE [ID] = @id;
END
--разделитель|
CREATE PROCEDURE AddNewRole
AS
BEGIN
	INSERT INTO [dbo].[Roles]([RoleName]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPageId;
END
--разделитель|
CREATE PROCEDURE DeleteRole
	@id int
AS
BEGIN
	DELETE FROM [dbo].[Roles] WHERE [ID]=@id;
END
--разделитель|
CREATE PROCEDURE GetPositionList
AS
BEGIN
	SELECT * FROM [dbo].[Positions]
END
--разделитель|
CREATE PROCEDURE AddNewPosition
AS
BEGIN
	INSERT INTO [dbo].[Positions]([Name]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPosId;
END
--разделитель|
CREATE PROCEDURE ChangePositionStats
	@id int,
	@PositionName nvarchar(50),
	@Salary money
AS
BEGIN
	UPDATE [dbo].[Positions] SET [Name] = @PositionName, [Salary]=@Salary WHERE [ID] = @id;
END
--разделитель|
CREATE PROCEDURE DeletePosition
	@id int
AS
BEGIN
	DELETE FROM [dbo].[Positions] WHERE [ID]=@id;
END
--разделитель|
CREATE PROCEDURE GetEmployeeUsersList
AS
BEGIN
	SELECT [ID],[Login],[FullName],[Passport],[PhoneNum],[Position_ID] FROM [dbo].[EmployeeUsers];
END
--разделитель|
CREATE PROCEDURE ChangeUserStats
	@userid int,
	@login nvarchar(100),
	@fullName nvarchar(50),
	@passport nvarchar(11),
	@phoneNum nvarchar(17),
	@posID int
AS
BEGIN
	UPDATE [dbo].[EmployeeUsers] SET [Login]=@login,[FullName]=@fullName,[Passport]=@passport,[PhoneNum]=@phoneNum,[Position_ID]=@posID WHERE [ID]=@userid;
END
--разделитель|
CREATE PROCEDURE DeleteUser
	@userid int
AS
BEGIN
	DELETE FROM [dbo].[EmployeeUsers] WHERE [ID]=@userid;
END
--разделитель|
CREATE PROCEDURE GetUserRolesList
AS
BEGIN
	SELECT * FROM [dbo].[UserRoles];
END
--разделитель|
CREATE PROCEDURE ChangeUserRole
	@userid int,
	@roleid int,
	@isCan bit
AS 
BEGIN
	UPDATE [dbo].[UserRoles] SET [IsCan] = @isCan WHERE [User_ID]=@userid AND [Role_ID]=@roleid
END
--разделитель|
CREATE PROCEDURE CreateEmployeeUser
	@login nvarchar(100),
	@fullName nvarchar(50),
	@passport nvarchar(11),
	@phoneNum nvarchar(17),
	@posID int
AS
BEGIN
	INSERT INTO [dbo].[EmployeeUsers] ([Login],[FullName],[Passport],[PhoneNum],[Position_ID])
	VALUES (@login,@fullName,@passport,@phoneNum,@posID);
END;
--разделитель|
CREATE Procedure ChangeUserPassword
	@userID int,
	@password nvarchar(128),
	@salt nvarchar(16)
AS
BEGIN
	SET @password = CONVERT(NVARCHAR(128), HASHBYTES('SHA2_512', @password), 2);

	UPDATE [dbo].[EmployeeUsers] SET [Password] = @password, [Salt] = @salt WHERE [ID] = @userID;
END;
--разделитель|
CREATE PROCEDURE GetSalt
    @login nvarchar(100)
AS
BEGIN
    DECLARE @salt nvarchar(max);
    SELECT @salt = [Salt] FROM [EmployeeUsers] WHERE [Login] = @login;

    IF @salt IS NULL OR @salt = ''
        SET @salt = CONVERT(nvarchar(36), NEWID());

    SET @salt = LEFT(@salt, 16);

    SELECT @salt;
END;
--разделитель|
CREATE PROC TryAuthorizeUser
	@login nvarchar(100),
	@password nvarchar(128)
AS
BEGIN
	SET @password = CONVERT(NVARCHAR(128), HASHBYTES('SHA2_512', @password), 2);
	SELECT [ID] FROM [EmployeeUsers] WHERE [Login] = @login AND [Password] = @password;
	SELECT @password;
END;
--разделитель|
CREATE PROC GetUserPagesList
	@UserID int
AS
BEGIN
	SELECT DISTINCT [PagesList].[PageName] FROM [EmployeeUsers]
	JOIN [UserRoles] ON [EmployeeUsers].[ID] = [UserRoles].[User_ID]
	JOIN [Roles] ON [Roles].[ID] = [UserRoles].[Role_ID]
	JOIN [RightRole] ON [Roles].[ID] = [RightRole].[Role_ID]
	JOIN [PagesList] ON [PagesList].[ID] = [RightRole].[Page_ID]
	WHERE [EmployeeUsers].[ID] = @UserID AND [UserRoles].[IsCan] = 1 AND [RightRole].[IsCan] = 1
END;
--разделитель|
CREATE PROC GetProductList
AS 
BEGIN
	SELECT * FROM [dbo].[ProductsInStock];
END;
--разделитель|
CREATE PROC ChangeProduct
	@id int,
	@productName nvarchar(100),
	@picture nvarchar(max),
	@count int
AS
BEGIN
	UPDATE [dbo].[ProductsInStock] SET [Name] = @productName, [ProductPicture] = @picture, [CountInStock] = @count
	WHERE [ID] = @id;
END;
--разделитель|
CREATE PROC CreateProduct
AS
BEGIN
	INSERT INTO [ProductsInStock] VALUES ('', '', 0);
END;
--разделитель|
CREATE PROC DeleteProduct
	@id int
AS
BEGIN
	DELETE FROM [dbo].[ProductsInStock] WHERE [ID] = @id;
END;
--разделитель|
CREATE PROC GetDishesList
AS 
BEGIN
	SELECT * FROM [dbo].[Dishes];
END;
--разделитель|
CREATE PROC ChangeDish
	@id int,
	@Name nvarchar(100),
	@cost money,
	@time int,
	@description nvarchar(1024),
	@pictures nvarchar(max)
AS
BEGIN
	UPDATE [dbo].[Dishes] SET [Name] = @Name, [Cost] = @cost, [TimeCooking] = @time, [Description] = @description, [Pictures] = @pictures
	WHERE [ID] = @id;
END;
--разделитель|
CREATE PROC CreateDish
AS
BEGIN
	INSERT INTO [dbo].[Dishes] VALUES ('', 0, 0, '', '');
END;
--разделитель|
CREATE PROC DeleteDish
	@id int
AS
BEGIN
	DELETE FROM [dbo].[ProductForDishes] WHERE [Dish_ID] = @id;
	DELETE FROM [dbo].[Dishes] WHERE [ID] = @id;
END;
--разделитель|
CREATE PROC GetDishesProductList
AS 
BEGIN
	SELECT * FROM [dbo].[ProductForDishes];
END;
--разделитель|
CREATE PROC ChangeDishesProduct
	@ID int,
	@count float,
	@Product_ID int
AS
BEGIN
	UPDATE [dbo].[ProductForDishes] SET [Count] = @count, [Product_ID] = @Product_ID WHERE [ID] = @ID
END;
--разделитель|
CREATE PROC CreateDishesProduct
	@Dish_ID int
AS
BEGIN
	INSERT INTO [dbo].[ProductForDishes] ([Dish_ID],[Count]) VALUES (@Dish_ID, 0);
END;
--разделитель|
CREATE PROC DeleteDishProduct
	@ID int
AS
BEGIN
	DELETE FROM [dbo].[ProductForDishes] WHERE [ID] = @ID;
END;
--разделитель|
CREATE PROC GetOpenedOrderList
AS
BEGIN
	SELECT * FROM [dbo].[OpenedOrder];
END
--разделитель|
CREATE PROC GetOpenedDishOrderList
AS
BEGIN
	SELECT * FROM [dbo].[OpenedOrderLists];
END;
--разделитель|
CREATE PROC CloseOrder
	@orderID int
AS
BEGIN
	INSERT INTO [ClosedOrders] SELECT [ID],[EmployeeUserID],[CreateDateTime],GETDATE(),[TableInfo],[CookingTime],[Sum] FROM [OpenedOrder] WHERE [ID] = @orderID;
	INSERT INTO [ClosedOrderLists] SELECT * FROM [OpenedOrderLists] WHERE [OrderID] = @orderID;

	DELETE FROM [OpenedOrderLists] WHERE [OrderID] = @orderID;
	DELETE FROM [OpenedOrder] WHERE [ID] = @orderID;
END;
--разделитель|
CREATE PROC CreateOpenedOrder
	@EmpID int,
	@TableInfo nvarchar(250),
	@cookingTime int,
	@sum money,
	@guid nvarchar(250)
AS
BEGIN
	INSERT INTO [dbo].[OpenedOrder] VALUES (@EmpID, GETDATE(), @TableInfo, @cookingTime, @sum);
	INSERT INTO [dbo].[OpenedOrderLists] SELECT SCOPE_IDENTITY(),[DishID],[ProductCount] FROM [dbo].[OpenedOrderListsTemp] WHERE [GUID] = @guid;
	DELETE FROM [dbo].[OpenedOrderListsTemp] WHERE [GUID] = @guid;
END;
--разделитель|
CREATE PROC CreateOpenedOrderList
	@guid nvarchar(250),
	@dishID int,
	@count int
AS
BEGIN
	INSERT INTO [dbo].[OpenedOrderListsTemp] VALUES (@guid, @dishID, @count);
END;