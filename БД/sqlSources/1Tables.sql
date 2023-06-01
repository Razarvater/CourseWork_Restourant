--Роли
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
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[Product_ID] int REFERENCES [dbo].[ProductsInStock]([ID]),
	[Dish_ID] int REFERENCES [dbo].[Dishes]([ID]),
	[Count] float NOT NULL DEFAULT 0
)

--Раздел заказов
CREATE TABLE [dbo].[OpenedOrder]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[EmployeeUserID] int REFERENCES [dbo].[EmployeeUsers]([ID]),
	[CreateDateTime] datetime NULL,
	[TableInfo] nvarchar(250),
	[CookingTime] int NOT NULL DEFAULT 0,
	[Sum] money NOT NULL DEFAULT 0
)

CREATE TABLE [dbo].[OpenedOrderLists]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[OrderID] int REFERENCES [dbo].[OpenedOrder]([ID]),
	[ProductID] int REFERENCES [dbo].[ProductsInStock]([ID])
)

CREATE TABLE [dbo].[ClosedOrders]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[EmployeeUserID] int REFERENCES [dbo].[EmployeeUsers]([ID]),
	[CreateDateTime] datetime NULL,
	[CloseDateTime] datetime NULL,
	[TableInfo] nvarchar(250),
	[CookingTime] int NOT NULL DEFAULT 0,
	[Sum] money NOT NULL DEFAULT 0
)

CREATE TABLE [dbo].[ClosedOrderLists]
(
	[ID] int IDENTITY(1,1) PRIMARY KEY,
	[OrderID] int REFERENCES [dbo].[OpenedOrder]([ID]),
	[ProductID] int REFERENCES [dbo].[ProductsInStock]([ID])
)
