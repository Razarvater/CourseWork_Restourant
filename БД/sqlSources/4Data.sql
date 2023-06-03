INSERT INTO [dbo].[PagesList] VALUES ('Авторизация');
INSERT INTO [dbo].[PagesList] VALUES ('Роли');
INSERT INTO [dbo].[PagesList] VALUES ('Пользователи');
INSERT INTO [dbo].[PagesList] VALUES ('Кухня');
INSERT INTO [dbo].[PagesList] VALUES ('Заказы');

INSERT INTO [dbo].[Roles] VALUES ('Владелец');

UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = 1;


INSERT INTO [dbo].[Positions] VALUES ('Ген. директор', 100000);
--пароль test
INSERT INTO [dbo].[EmployeeUsers] VALUES ('Owner','7A53579F2329D52F01E0B821B7525D874E1A324167A200A040FBC73F6EADD42D2402967C1359CD5C55D3DF7B4D7DE07A03C50DD4B4F279B0A3CA608FA6CE3361', 'bq9gExJ+8qN','name','pass','phoneNum',1)

UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner')

INSERT INTO [ProductsInStock] VALUES('Test1', '', 100);
INSERT INTO [ProductsInStock] VALUES('Test2', '', 100);
INSERT INTO [ProductsInStock] VALUES('Test3', '', 100);

INSERT INTO [Dishes] VALUES ('Test_Dish1', 2500, 1000, 'Test_Description', '');
INSERT INTO [Dishes] VALUES ('Test_Dish2', 3500, 500, 'Test_Description', '');
INSERT INTO [Dishes] VALUES ('Test_Dish2', 4500, 3600, 'Test_Description', '');

INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test1'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish1'), 1);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test2'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish1'), 10);

INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test1'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish2'), 0.7);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test2'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish2'), 15);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test3'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish2'), 0.5);

INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Test1'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish3'), 0.8);

INSERT INTO [OpenedOrder] VALUES ((SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner'),'','столик 1',1000, 2500)

INSERT INTO [OpenedOrderLists] VALUES ((SELECT TOP 1 [ID] FROM [OpenedOrder] WHERE EmployeeUserID = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner')),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Test_Dish1'), 10)