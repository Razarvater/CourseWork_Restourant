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

UPDATE [dbo].[UserRoles] SET [IsCan] = 1 WHERE [User_ID] = 1