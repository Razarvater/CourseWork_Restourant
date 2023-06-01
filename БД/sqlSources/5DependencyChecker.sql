CREATE TABLE [dbo].[ActiveQueues]
(
	QueueName nvarchar(255) UNIQUE NOT NULL,
	ServiceName nvarchar(255) UNIQUE NOT NULL,
	TableName nvarchar(255) NOT NULL,
	LinkedUserID int NULL --���� �������� ID ������������� � ������� ������������
)

--�����������|
CREATE PROCEDURE [dbo].[ConnectUserToTable]
	@tableName nvarchar(255),
	@sessionGUID NVARCHAR(36) OUTPUT
AS
BEGIN
		DECLARE @sql NVARCHAR(MAX)

		SET @sessionGUID = CONVERT(NVARCHAR(36), NEWID());

		DECLARE @service nvarchar(255);
		DECLARE @queue nvarchar(255);

		SET @service = @tableName + 'Service' + @sessionGUID + '';
		SET @queue = @tableName + 'MessageQueue' + @sessionGUID + '';
		-- ������� � ������� �������� ������������� ���������� 
		IF NOT EXISTS (SELECT * FROM sys.service_queues WHERE name = @queue)
		BEGIN
            SET @sql = 'CREATE QUEUE [' + @queue + ']';
			EXEC sp_executesql @sql;
			PRINT '��������� ������� ' + @queue + '';
		END;

        -- ������ �� ������� ����� ������������ ������������� ����������
        IF NOT EXISTS(SELECT * FROM sys.services WHERE name = @service)
		BEGIN
            SET @sql = 'CREATE SERVICE [' + @service + '] ON QUEUE [' + @queue + '] ([DEFAULT])';
			EXEC sp_executesql @sql;
			PRINT '�������� ������ ' + @service;
		END;

		INSERT INTO [dbo].[ActiveQueues] ([ServiceName],[QueueName],[TableName],[LinkedUserID]) VALUES (@service,@queue,@tableName,0);
END;

--�����������|
CREATE PROCEDURE [dbo].[DeconnectUserFromTable]
	@tableName nvarchar(255),
	@sessionGUID NVARCHAR(36)
AS
BEGIN
		DECLARE @sql NVARCHAR(MAX)

		DECLARE @service nvarchar(255);
		DECLARE @queue nvarchar(255);

		SET @service = @tableName + 'Service' + @sessionGUID;
		SET @queue = @tableName + 'MessageQueue' + @sessionGUID;
		
		SET @sql = 'DROP SERVICE [' + @service + ']';
		EXEC sp_executesql @sql;
		SET @sql = 'DROP QUEUE [' + @queue + ']';
		EXEC sp_executesql @sql;
		
		DELETE FROM [dbo].[ActiveQueues] WHERE [TableName] = @tableName AND [ServiceName] = @service AND [QueueName] = @queue;
END;

--�����������|
CREATE PROCEDURE [dbo].[DropAllConnects]
AS
BEGIN
	DECLARE @serviceName NVARCHAR(255);
	DECLARE @queueName NVARCHAR(255);
	DECLARE @tableName NVARCHAR(255);
	DECLARE @sql NVARCHAR(MAX);
	DECLARE sessions_curs CURSOR
	FOR SELECT [QueueName],[ServiceName],[TableName] FROM [dbo].[ActiveQueues];

	OPEN sessions_curs
	FETCH NEXT FROM sessions_curs INTO @queueName, @serviceName, @tableName;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF EXISTS(SELECT * FROM sys.services WHERE name = @serviceName)
		BEGIN
			SET @sql = 'DROP SERVICE [' + @serviceName + ']';
			EXEC sp_executesql @sql;
		END;

		IF EXISTS (SELECT * FROM sys.service_queues WHERE name = @queueName)
		BEGIN
			SET @sql = 'DROP QUEUE [' + @queueName + ']';
			EXEC sp_executesql @sql;
		END;

		FETCH NEXT FROM sessions_curs INTO @queueName, @serviceName, @tableName;
	END;

	DELETE FROM [ActiveQueues];
	--��������� ������
	CLOSE sessions_curs
	--����������� ������ ���������� ��� ������
	DEALLOCATE sessions_curs
END;

--�����������|
CREATE PROCEDURE [dbo].[DropListenerNotifications]
AS
BEGIN
	DECLARE @tableName NVARCHAR(255);
	DECLARE @sql NVARCHAR(MAX);
	-- ������ ��� ��������� ���� ������ � ��
    DECLARE tables_cursor CURSOR FOR 
    SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') AND name != 'ActiveQueues'
	
	--��������� ������
    OPEN tables_cursor

	--��������� ������ ��������
    FETCH NEXT FROM tables_cursor INTO @tableName
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF EXISTS(SELECT * FROM sys.triggers WHERE name = @tableName + 'Listener')
		BEGIN
			SET @sql = 'DROP TRIGGER ' + @tableName + 'Listener';
			EXEC sp_executesql @sql;
			PRINT '����� ������� '+ @tableName + 'Listener';
		END;
		FETCH NEXT FROM tables_cursor INTO @tableName;
	END;

	--��������� ������
	CLOSE tables_cursor
	--����������� ������ ���������� ��� ������
	DEALLOCATE tables_cursor
END;
--�����������|
CREATE PROCEDURE [dbo].[InstallListenerNotifications]
AS
BEGIN 
	--��������� Service Broker
	IF EXISTS(SELECT * FROM sys.databases WHERE name = DB_NAME() AND is_broker_enabled = 0)
	BEGIN
		ALTER DATABASE [DB_NAME()] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		ALTER DATABASE [DB_NAME()] SET ENABLE_BROKER;
		ALTER DATABASE [DB_NAME()] SET MULTI_USER WITH ROLLBACK IMMEDIATE;
		PRINT '������� Service Broker';
	END

	--��� �������������� �������, � ���������� SQL ������
	DECLARE @tableName NVARCHAR(255)

    -- ������ ��� ��������� ���� ������ � ��
    DECLARE tables_cursor CURSOR FOR 
    SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') AND name != 'ActiveQueues'
	
	--��������� ������
    OPEN tables_cursor

	--��������� ������ ��������
    FETCH NEXT FROM tables_cursor INTO @tableName

	--������ �� ���� ���������
    WHILE @@FETCH_STATUS = 0
    BEGIN
		--��������� ������� ��������-������� ����������� �� ������ �������
		IF NOT EXISTS(SELECT * FROM sys.triggers WHERE name = @tableName + 'Listener')
		BEGIN
			DECLARE @triggerStatement NVARCHAR(MAX);--�������
			DECLARE @select NVARCHAR(MAX);--������ columns
			DECLARE @sqlInserted NVARCHAR(MAX);--Insert ������� (����� ����������� �������)
			DECLARE @sqlDeleted NVARCHAR(MAX);--Delete ������� (����� �������� �������)

			--������������� ������� ��������
			SET @triggerStatement = N'
			CREATE TRIGGER [' + @tableName + 'Listener] ON [dbo].[' + @tableName + ']
			AFTER INSERT, UPDATE, DELETE
			AS
			SET NOCOUNT ON;
			BEGIN
				DECLARE @message NVARCHAR(MAX);
				SET @message = N''<root/>'';

				IF(EXISTS(SELECT 1))
				BEGIN
					DECLARE @retvalOUT NVARCHAR(MAX);

					%InsertSelectState%

					IF(@retvalOUT IS NOT NULL)
					BEGIN
						SET @message = N''<root>'' + @retvalOUT;
					END;

					%DeleteSelectState%

					IF(@retvalOUT IS NOT NULL)
					BEGIN
						IF(@message = N''<root/>'')
						BEGIN
							SET @message = N''<root>'' + @retvalOUT;
						END
						ELSE
						BEGIN
							SET @message = @message + @retvalOUT;
						END;
					END;

					IF (@message != N''<root/>'') 
					BEGIN 
						SET @message = @message + N''</root>'';
					END;        
				END;

				DECLARE @serviceName nvarchar(255);
				DECLARE services_cursor CURSOR FOR 
				SELECT [ServiceName] FROM [ActiveQueues] WHERE [TableName] = ''' + @tableName + ''';
	
				--��������� ������
				OPEN services_cursor;

				--��������� ������ ��������
				FETCH NEXT FROM services_cursor INTO @serviceName;

				WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @ConvHandle UNIQUEIDENTIFIER

					--��������� ����� ������� � �������������� ������������� SQL
					DECLARE @Sql NVARCHAR(MAX) = N''BEGIN DIALOG @ConvHandle FROM SERVICE ['' + @serviceName + ''] TO SERVICE '''''' + @serviceName + '''''' ON CONTRACT [DEFAULT] WITH ENCRYPTION = OFF, LIFETIME = 60;
					SEND ON CONVERSATION @ConvHandle MESSAGE TYPE [DEFAULT] (@message);
					END CONVERSATION @ConvHandle;''

					--��������� ������ � �������������� sp_executesql
					EXEC sp_executesql @Sql, N''@ConvHandle uniqueidentifier OUTPUT, @serviceName nvarchar(255), @message nvarchar(MAX)'', @ConvHandle OUTPUT, @serviceName, @message
					FETCH NEXT FROM services_cursor INTO @serviceName;
				END;
				--��������� ������
				CLOSE services_cursor
				--����������� ������ ���������� ��� ������
				DEALLOCATE services_cursor

			END;'

			--�������� �������� ������� � ������� � ����: "[Column1],[Column2],[Column3]"
			SET @select = STUFF
			(
				(SELECT ',' + '[' + [COLUMN_NAME] + ']' FROM INFORMATION_SCHEMA.COLUMNS
				WHERE DATA_TYPE NOT IN  ('text','ntext','image','geometry','geography') AND TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @tableName AND TABLE_CATALOG = DB_NAME()
				FOR XML PATH ('')), 1, 1, ''
			);
			--��������� ������ ��� ��������� ���� inserted ��������
			SET @sqlInserted = N'
				SET @retvalOUT = (SELECT '+ @select +' FROM INSERTED
				FOR XML PATH(''row''), ROOT(''inserted''));';

			--��������� ������ ��� ��������� ���� deleted ��������
			SET @sqlDeleted = N'
				SET @retvalOUT = (SELECT '+ @select +' FROM DELETED
				FOR XML PATH(''row''), ROOT(''deleted''));';

			--��������� �������
			SET @triggerStatement = REPLACE(@triggerStatement, '%InsertSelectState%', @sqlInserted);
			SET @triggerStatement = REPLACE(@triggerStatement, '%DeleteSelectState%', @sqlDeleted);

			--������ �������
			EXEC sp_executesql @triggerStatement;
			PRINT '�������� ������� ' + @tableName + 'Listener';
		END;
		--��������� � ��������� �������
		FETCH NEXT FROM tables_cursor INTO @tableName
	END
	--��������� ������
	CLOSE tables_cursor
	--����������� ������ ���������� ��� ������
	DEALLOCATE tables_cursor
END;