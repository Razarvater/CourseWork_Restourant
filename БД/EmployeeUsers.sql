CREATE DATABASE Restourant;
--DROP DATABASE Restourant;

CREATE TABLE [dbo].[PagesList]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[PageName] nvarchar(50) UNIQUE NOT NULL DEFAULT 'Главная страница'
)

CREATE TABLE [dbo].[Roles]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[RoleName] nvarchar(50) UNIQUE NOT NULL DEFAULT 'Бухгалтер'
)

CREATE TABLE [dbo].[RightRole]
(
	[Page_ID] int REFERENCES [dbo].[PagesList]([ID]) ON DELETE CASCADE,
	[Role_ID] int REFERENCES [dbo].[Roles]([ID]) ON DELETE CASCADE,
	[IsCan] bit NOT NULL DEFAULT 0,
	PRIMARY KEY CLUSTERED ([Page_ID], [Role_ID])
)

CREATE TABLE [dbo].[Positions]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(50) UNIQUE,
	[Salary] money NOT NULL DEFAULT 0
)

CREATE TABLE [dbo].[EmployeeUsers]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[Login] nvarchar(100) UNIQUE NOT NULL,
	[Password] varchar(128),
	[Salt] varchar(16) DEFAULT '11111111',
	[FullName] nvarchar(50) NOT NULL,
	[Passport] varchar(11) NOT NULL,
	[PhoneNum] varchar(17) NOT NULL,
	[Position_ID] int REFERENCES [dbo].[Positions]([ID])
)

CREATE TABLE [dbo].[UserRoles]
(
	[User_ID] int REFERENCES [dbo].[EmployeeUsers]([ID]) ON DELETE CASCADE,
	[Role_ID] int REFERENCES [dbo].[Roles]([ID]) ON DELETE CASCADE,
	[IsCan] bit NOT NULL DEFAULT 0,
	PRIMARY KEY CLUSTERED ([User_ID], [Role_ID])
)


--Раздел кухня
CREATE TABLE [dbo].[ProductsInStock]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] nvarchar(100),
	[ProductPicture] nvarchar(max),
	[CountInStock] float NOT NULL DEFAULT 0,
)

CREATE TABLE [dbo].[Dishes]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] nvarchar(100),
	[Cost] money NOT NULL DEFAULT 0,
	[TimeCooking] int NOT NULL DEFAULT 0,
	[Description] nvarchar(1024),
	[Pictures] nvarchar(max)
)

CREATE TABLE [dbo].[ProductForDishes]
(
	[Product_ID] int REFERENCES [dbo].[ProductsInStock]([ID]),
	[Dish_ID] int REFERENCES [dbo].[Dishes]([ID]),
	[Count] float NOT NULL DEFAULT 0
)

--Триггеры
CREATE TRIGGER AddNewPageTrigger ON [dbo].[PagesList]
AFTER INSERT
AS
BEGIN
	DECLARE @id int;
	DECLARE @pageID int;

	SELECT @pageID=[ID] FROM inserted;

	INSERT INTO [dbo].[RightRole] ([Role_ID], [Page_ID])
    SELECT [ID], @pageID FROM [dbo].[Roles];
END;

CREATE TRIGGER AddNewRoleTrigger ON [dbo].[Roles]
AFTER INSERT
AS
BEGIN
	DECLARE @roleID int;

	SELECT @roleID=[ID] FROM inserted;

	INSERT INTO [dbo].[RightRole] ([Role_ID], [Page_ID])
	SELECT @roleID,[ID] FROM [dbo].[PagesList]

	INSERT INTO [dbo].[UserRoles] ([Role_ID],[User_ID])
	SELECT @roleID,[ID] FROM [dbo].[EmployeeUsers]
END;

CREATE TRIGGER AddNewUserTrigger ON [dbo].[EmployeeUsers]
AFTER INSERT
AS
BEGIN
	DECLARE @userID int;

	SELECT @userID=[ID] FROM inserted;

	INSERT INTO [dbo].[UserRoles] ([Role_ID],[User_ID])
	SELECT [ID],@userID FROM [dbo].[Roles]
END;

--Процедуры
CREATE PROCEDURE AddNewPage
AS
BEGIN
	INSERT INTO [dbo].[PagesList]([PageName]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPageId;
END

CREATE PROCEDURE UpdatePage
	@id int,
	@NewName nvarchar(50)
AS
BEGIN 
	UPDATE [dbo].[PagesList] SET [PageName]=@NewName WHERE [ID] = @id;
END

CREATE PROCEDURE GetPagesList
AS
BEGIN
	SELECT * FROM PagesList ORDER BY [ID] ASC
END

CREATE PROCEDURE GetRolesList
AS
BEGIN
	SELECT * FROM [dbo].[Roles] ORDER BY [ID] ASC
END

CREATE PROCEDURE GetRolesPagesList
AS
BEGIN
	SELECT * FROM [dbo].[RightRole]
END

CREATE PROCEDURE DeletePage
	@id int
AS
BEGIN
	DELETE FROM [dbo].[PagesList] WHERE [ID] = @id
END

CREATE PROCEDURE ChangeRight
	@rid int,
	@pid int,
	@can bit
AS
BEGIN
	UPDATE [dbo].[RightRole] SET [IsCan] = @can WHERE [Role_ID] = @rid AND [Page_ID] = @pid
END 

CREATE PROCEDURE ChangeRoleName
	@id int,
	@roleName nvarchar(50)
AS
BEGIN
	UPDATE [dbo].[Roles] SET [RoleName] = @roleName WHERE [ID] = @id;
END

CREATE PROCEDURE AddNewRole
AS
BEGIN
	INSERT INTO [dbo].[Roles]([RoleName]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPageId;
END

CREATE PROCEDURE DeleteRole
	@id int
AS
BEGIN
	DELETE FROM [dbo].[Roles] WHERE [ID]=@id;
END


CREATE PROCEDURE GetPositionList
AS
BEGIN
	SELECT * FROM [dbo].[Positions]
END

CREATE PROCEDURE AddNewPosition
AS
BEGIN
	INSERT INTO [dbo].[Positions]([Name]) VALUES ('');
	SELECT SCOPE_IDENTITY() AS NewPosId;
END

CREATE PROCEDURE ChangePositionStats
	@id int,
	@PositionName nvarchar(50),
	@Salary money
AS
BEGIN
	UPDATE [dbo].[Positions] SET [Name] = @PositionName, [Salary]=@Salary WHERE [ID] = @id;
END

CREATE PROCEDURE DeletePosition
	@id int
AS
BEGIN
	DELETE FROM [dbo].[Positions] WHERE [ID]=@id;
END

CREATE PROCEDURE GetEmployeeUsersList
AS
BEGIN
	SELECT [ID],[Login],[FullName],[Passport],[PhoneNum],[Position_ID] FROM [dbo].[EmployeeUsers];
END

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

CREATE PROCEDURE DeleteUser
	@userid int
AS
BEGIN
	DELETE FROM [dbo].[EmployeeUsers] WHERE [ID]=@userid;
END

CREATE PROCEDURE GetUserRolesList
AS
BEGIN
	SELECT * FROM [dbo].[UserRoles];
END

CREATE PROCEDURE ChangeUserRole
	@userid int,
	@roleid int,
	@isCan bit
AS 
BEGIN
	UPDATE [dbo].[UserRoles] SET [IsCan] = @isCan WHERE [User_ID]=@userid AND [Role_ID]=@roleid
END

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

CREATE Procedure ChangeUserPassword
	@userID int,
	@password nvarchar(128),
	@salt nvarchar(16)
AS
BEGIN
	SET @password = CONVERT(NVARCHAR(128), HASHBYTES('SHA2_512', @password), 2);

	UPDATE [dbo].[EmployeeUsers] SET [Password] = @password, [Salt] = @salt WHERE [ID] = @userID;
END;

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

CREATE PROC TryAuthorizeUser
	@login nvarchar(100),
	@password nvarchar(128)
AS
BEGIN
	SET @password = CONVERT(NVARCHAR(128), HASHBYTES('SHA2_512', @password), 2);
	SELECT [ID] FROM [EmployeeUsers] WHERE [Login] = @login AND [Password] = @password;
	SELECT @password;
END;

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


CREATE PROC GetProductList
AS 
BEGIN
	SELECT * FROM [dbo].[ProductsInStock];
END;

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

CREATE PROC CreateProduct
AS
BEGIN
	INSERT INTO [ProductsInStock] VALUES ('', '', 0);
END;

CREATE PROC DeleteProduct
	@id int
AS
BEGIN
	DELETE FROM [dbo].[ProductsInStock] WHERE [ID] = @id;
END;