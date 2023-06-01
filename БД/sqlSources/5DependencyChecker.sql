CREATE TABLE [dbo].[ActiveQueues]
(
	QueueName nvarchar(255) UNIQUE NOT NULL,
	ServiceName nvarchar(255) UNIQUE NOT NULL,
	TableName nvarchar(255) NOT NULL,
	LinkedUserID int NULL --Сюда добавить ID подключенного к очереди пользователя
)

--разделитель|
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
		-- Очередь в которой хранится отслеживаемая информация 
		IF NOT EXISTS (SELECT * FROM sys.service_queues WHERE name = @queue)
		BEGIN
            SET @sql = 'CREATE QUEUE [' + @queue + ']';
			EXEC sp_executesql @sql;
			PRINT 'Добавлена очередь ' + @queue + '';
		END;

        -- Сервис на который будет отправляться отслеживаемая информация
        IF NOT EXISTS(SELECT * FROM sys.services WHERE name = @service)
		BEGIN
            SET @sql = 'CREATE SERVICE [' + @service + '] ON QUEUE [' + @queue + '] ([DEFAULT])';
			EXEC sp_executesql @sql;
			PRINT 'Добавлен сервис ' + @service;
		END;

		INSERT INTO [dbo].[ActiveQueues] ([ServiceName],[QueueName],[TableName],[LinkedUserID]) VALUES (@service,@queue,@tableName,0);
END;

--разделитель|
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

--разделитель|
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
	--Закрываем курсор
	CLOSE sessions_curs
	--Освобождаем память выделенную под курсор
	DEALLOCATE sessions_curs
END;

--разделитель|
CREATE PROCEDURE [dbo].[DropListenerNotifications]
AS
BEGIN
	DECLARE @tableName NVARCHAR(255);
	DECLARE @sql NVARCHAR(MAX);
	-- Курсор для обработки всех таблиц в БД
    DECLARE tables_cursor CURSOR FOR 
    SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') AND name != 'ActiveQueues'
	
	--Открываем курсор
    OPEN tables_cursor

	--Извлекаем первое значение
    FETCH NEXT FROM tables_cursor INTO @tableName
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF EXISTS(SELECT * FROM sys.triggers WHERE name = @tableName + 'Listener')
		BEGIN
			SET @sql = 'DROP TRIGGER ' + @tableName + 'Listener';
			EXEC sp_executesql @sql;
			PRINT 'Удалён триггер '+ @tableName + 'Listener';
		END;
		FETCH NEXT FROM tables_cursor INTO @tableName;
	END;

	--Закрываем курсор
	CLOSE tables_cursor
	--Освобождаем память выделенную под курсор
	DEALLOCATE tables_cursor
END;
--разделитель|
CREATE PROCEDURE [dbo].[InstallListenerNotifications]
AS
BEGIN 
	--Установка Service Broker
	IF EXISTS(SELECT * FROM sys.databases WHERE name = DB_NAME() AND is_broker_enabled = 0)
	BEGIN
		ALTER DATABASE [DB_NAME()] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		ALTER DATABASE [DB_NAME()] SET ENABLE_BROKER;
		ALTER DATABASE [DB_NAME()] SET MULTI_USER WITH ROLLBACK IMMEDIATE;
		PRINT 'Включен Service Broker';
	END

	--Имя обрабатываемой таблицы, и динамичный SQL запрос
	DECLARE @tableName NVARCHAR(255)

    -- Курсор для обработки всех таблиц в БД
    DECLARE tables_cursor CURSOR FOR 
    SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') AND name != 'ActiveQueues'
	
	--Открываем курсор
    OPEN tables_cursor

	--Извлекаем первое значение
    FETCH NEXT FROM tables_cursor INTO @tableName

	--Проход по всем значениям
    WHILE @@FETCH_STATUS = 0
    BEGIN
		--Проверяем наличие триггера-очереди повешенного на данную таблицу
		IF NOT EXISTS(SELECT * FROM sys.triggers WHERE name = @tableName + 'Listener')
		BEGIN
			DECLARE @triggerStatement NVARCHAR(MAX);--Триггер
			DECLARE @select NVARCHAR(MAX);--Список columns
			DECLARE @sqlInserted NVARCHAR(MAX);--Insert команда (поиск вставленных записей)
			DECLARE @sqlDeleted NVARCHAR(MAX);--Delete команда (поиск удалённых записей)

			--Инициализация шаблона триггера
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
	
				--Открываем курсор
				OPEN services_cursor;

				--Извлекаем первое значение
				FETCH NEXT FROM services_cursor INTO @serviceName;

				WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @ConvHandle UNIQUEIDENTIFIER

					--Формируем текст запроса с использованием динамического SQL
					DECLARE @Sql NVARCHAR(MAX) = N''BEGIN DIALOG @ConvHandle FROM SERVICE ['' + @serviceName + ''] TO SERVICE '''''' + @serviceName + '''''' ON CONTRACT [DEFAULT] WITH ENCRYPTION = OFF, LIFETIME = 60;
					SEND ON CONVERSATION @ConvHandle MESSAGE TYPE [DEFAULT] (@message);
					END CONVERSATION @ConvHandle;''

					--Выполняем запрос с использованием sp_executesql
					EXEC sp_executesql @Sql, N''@ConvHandle uniqueidentifier OUTPUT, @serviceName nvarchar(255), @message nvarchar(MAX)'', @ConvHandle OUTPUT, @serviceName, @message
					FETCH NEXT FROM services_cursor INTO @serviceName;
				END;
				--Закрываем курсор
				CLOSE services_cursor
				--Освобождаем память выделенную под курсор
				DEALLOCATE services_cursor

			END;'

			--Получаем перечень колонок в таблице в виде: "[Column1],[Column2],[Column3]"
			SET @select = STUFF
			(
				(SELECT ',' + '[' + [COLUMN_NAME] + ']' FROM INFORMATION_SCHEMA.COLUMNS
				WHERE DATA_TYPE NOT IN  ('text','ntext','image','geometry','geography') AND TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @tableName AND TABLE_CATALOG = DB_NAME()
				FOR XML PATH ('')), 1, 1, ''
			);
			--Формируем запрос для получения всех inserted значений
			SET @sqlInserted = N'
				SET @retvalOUT = (SELECT '+ @select +' FROM INSERTED
				FOR XML PATH(''row''), ROOT(''inserted''));';

			--Формируем запрос для получения всех deleted значений
			SET @sqlDeleted = N'
				SET @retvalOUT = (SELECT '+ @select +' FROM DELETED
				FOR XML PATH(''row''), ROOT(''deleted''));';

			--Вставляем шаблоны
			SET @triggerStatement = REPLACE(@triggerStatement, '%InsertSelectState%', @sqlInserted);
			SET @triggerStatement = REPLACE(@triggerStatement, '%DeleteSelectState%', @sqlDeleted);

			--Создаём триггер
			EXEC sp_executesql @triggerStatement;
			PRINT 'Добавлен триггер ' + @tableName + 'Listener';
		END;
		--Переходим к следующей таблице
		FETCH NEXT FROM tables_cursor INTO @tableName
	END
	--Закрываем курсор
	CLOSE tables_cursor
	--Освобождаем память выделенную под курсор
	DEALLOCATE tables_cursor
END;