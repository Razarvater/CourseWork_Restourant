--Разделитель|
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
--Разделитель|
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
--Разделитель|
CREATE TRIGGER AddNewUserTrigger ON [dbo].[EmployeeUsers]
AFTER INSERT
AS
BEGIN
	DECLARE @userID int;

	SELECT @userID=[ID] FROM inserted;

	INSERT INTO [dbo].[UserRoles] ([Role_ID],[User_ID])
	SELECT [ID],@userID FROM [dbo].[Roles]
END;