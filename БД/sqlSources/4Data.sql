--Список страниц
INSERT INTO [dbo].[PagesList] VALUES ('Авторизация');
INSERT INTO [dbo].[PagesList] VALUES ('Роли');
INSERT INTO [dbo].[PagesList] VALUES ('Пользователи');
INSERT INTO [dbo].[PagesList] VALUES ('Кухня');
INSERT INTO [dbo].[PagesList] VALUES ('Заказы');

--Список ролей
INSERT INTO [dbo].[Roles] VALUES ('Владелец');
INSERT INTO [dbo].[Roles] VALUES ('Официант');
INSERT INTO [dbo].[Roles] VALUES ('Кассир');
INSERT INTO [dbo].[Roles] VALUES ('Повар');

--Права для ролей
UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Владелец');

UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Официант') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Авторизация');
UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Официант') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Заказы');

UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Кассир') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Авторизация');
UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Кассир') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Заказы');
UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Кассир') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Кухня');

UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Повар') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Авторизация');
UPDATE [dbo].[RightRole] SET [IsCan] = 1 WHERE [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Повар') AND [Page_ID] = (SELECT TOP 1 [ID] FROM [PagesList] WHERE [PageName] = 'Кухня');

--Должности
INSERT INTO [dbo].[Positions] VALUES ('Ген. директор', 100000);
INSERT INTO [dbo].[Positions] VALUES ('Официант', 25000);
INSERT INTO [dbo].[Positions] VALUES ('Бармен-продавец', 45000);
INSERT INTO [dbo].[Positions] VALUES ('Повар', 45000);

--Пользователи пароль test
INSERT INTO [dbo].[EmployeeUsers] VALUES ('Owner','7A53579F2329D52F01E0B821B7525D874E1A324167A200A040FBC73F6EADD42D2402967C1359CD5C55D3DF7B4D7DE07A03C50DD4B4F279B0A3CA608FA6CE3361', 'bq9gExJ+8qN','Алексей Валерьевич','passport','phoneNum', (SELECT TOP 1 [ID] FROM [Positions] WHERE [Name] = 'Ген. директор'))
INSERT INTO [dbo].[EmployeeUsers] VALUES ('Ofic','7A53579F2329D52F01E0B821B7525D874E1A324167A200A040FBC73F6EADD42D2402967C1359CD5C55D3DF7B4D7DE07A03C50DD4B4F279B0A3CA608FA6CE3361', 'bq9gExJ+8qN','Сырцов Луи','passport','phoneNum',  (SELECT TOP 1 [ID] FROM [Positions] WHERE [Name] = 'Официант'))
INSERT INTO [dbo].[EmployeeUsers] VALUES ('Cass','7A53579F2329D52F01E0B821B7525D874E1A324167A200A040FBC73F6EADD42D2402967C1359CD5C55D3DF7B4D7DE07A03C50DD4B4F279B0A3CA608FA6CE3361', 'bq9gExJ+8qN','Павлов Максим','passport','phoneNum',  (SELECT TOP 1 [ID] FROM [Positions] WHERE [Name] = 'Бармен-продавец'))
INSERT INTO [dbo].[EmployeeUsers] VALUES ('Pov','7A53579F2329D52F01E0B821B7525D874E1A324167A200A040FBC73F6EADD42D2402967C1359CD5C55D3DF7B4D7DE07A03C50DD4B4F279B0A3CA608FA6CE3361', 'bq9gExJ+8qN','Митя','passport','phoneNum',  (SELECT TOP 1 [ID] FROM [Positions] WHERE [Name] = 'Повар'))

--Роли для пользователей
UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner') AND [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Владелец')
UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Ofic') AND [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Официант')
UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Cass') AND [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Кассир')
UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Pov') AND [Role_ID] = (SELECT TOP 1 [ID] FROM [Roles] WHERE [RoleName] = 'Повар')

--Продукты
INSERT INTO [ProductsInStock] VALUES('Молоко', '/Images/milk.png', 100);
INSERT INTO [ProductsInStock] VALUES('Огурцы', '/Images/cucumber.png', 100);
INSERT INTO [ProductsInStock] VALUES('Селёдка', '/Images/herring.png', 10);

--Блюда
INSERT INTO [Dishes] VALUES ('Коктейль "Взрывной вечер"', 4500, 300, 'Вас ожидает убойный вечер', '/Images/Cocktail-1.png');
INSERT INTO [Dishes] VALUES ('Селёдка под шубой', 10000, 3600, 'Прекрасное дополнение к "Убойному вечеру"', '/Images/herring-1.png');

--Продукты для блюд
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Молоко'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Коктейль "Взрывной вечер"'), 1);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Селёдка'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Коктейль "Взрывной вечер"'), 0.3);

INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Молоко'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Селёдка под шубой'), 0.4);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Огурцы'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Селёдка под шубой'), 1);
INSERT INTO [ProductForDishes] VALUES ((SELECT TOP 1 [ID] FROM [ProductsInStock] WHERE [Name]='Селёдка'),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Селёдка под шубой'), 1);

--Заказы
INSERT INTO [OpenedOrder] VALUES ((SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner'),'2023-06-05 20:59:05.263','Столик 1', 3900, 43500)

INSERT INTO [OpenedOrderLists] VALUES ((SELECT TOP 1 [ID] FROM [OpenedOrder] WHERE EmployeeUserID = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner')),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Коктейль "Взрывной вечер"'), 3)
INSERT INTO [OpenedOrderLists] VALUES ((SELECT TOP 1 [ID] FROM [OpenedOrder] WHERE EmployeeUserID = (SELECT TOP 1 [ID] FROM [EmployeeUsers] WHERE [Login] = 'Owner')),(SELECT TOP 1 [ID] FROM [Dishes] WHERE [Name]='Селёдка под шубой'), 3)