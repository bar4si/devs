PRINT 'Starting database initialization...';

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$(dbname)')
BEGIN
    PRINT 'Creating database $(dbname)...';
    CREATE DATABASE $(dbname);
END
GO

USE $(dbname);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
BEGIN
    PRINT 'Creating table Tasks...';
    CREATE TABLE Tasks (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX),
        DueDate DATETIME2,
        Priority INT DEFAULT 1,
        IsCompleted BIT DEFAULT 0,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SensorReadings')
BEGIN
    PRINT 'Creating table SensorReadings...';
    CREATE TABLE SensorReadings (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SensorName NVARCHAR(100) NOT NULL,
        Value FLOAT NOT NULL,
        Unit NVARCHAR(20) NOT NULL,
        Timestamp DATETIME2 DEFAULT GETUTCDATE()
    );
END
GO

-- Seed industrial data
IF NOT EXISTS (SELECT 1 FROM Tasks)
BEGIN
    PRINT 'Seeding industrial data...';
    INSERT INTO Tasks (Id, Title, Description, DueDate, Priority, IsCompleted, CreatedAt)
    VALUES 
    (NEWID(), 'Inspeção Corretiva - Caldeira 04', 'Verificar vazamento na válvula de segurança e pressão nominal.', DATEADD(day, 2, GETUTCDATE()), 3, 0, GETUTCDATE()),
    (NEWID(), 'Calibração Sensores de Pressão', 'Calibrar sensores da linha de montagem A2 para conformidade ISO.', DATEADD(day, 5, GETUTCDATE()), 2, 0, GETUTCDATE()),
    (NEWID(), 'Manutenção Preventiva - CNC 01', 'Troca de óleo e limpeza dos trilhos do motor principal.', DATEADD(day, -1, GETUTCDATE()), 1, 1, DATEADD(day, -2, GETUTCDATE())),
    (NEWID(), 'Auditoria de Segurança Trimestral', 'Verificar validade de extintores e sinalização de rotas de fuga.', DATEADD(day, 10, GETUTCDATE()), 2, 0, GETUTCDATE()),
    (NEWID(), 'Treinamento de Operação - Robô KUKA', 'Capacitação da nova equipe no turno da noite.', DATEADD(day, 3, GETUTCDATE()), 1, 0, GETUTCDATE());
END
GO

-- Seed temporal sensor data (Generating 20 records per sensor)
IF NOT EXISTS (SELECT 1 FROM SensorReadings)
BEGIN
    PRINT 'Seeding sensor data...';
    DECLARE @Counter INT = 0;
    DECLARE @StartTime DATETIME2 = DATEADD(hour, -2, GETUTCDATE());

    WHILE @Counter < 20
    BEGIN
        -- Boiler Temperature (180 - 195 Range)
        INSERT INTO SensorReadings (SensorName, Value, Unit, Timestamp)
        VALUES ('TEMP_CALDEIRA_04', 180 + (RAND() * 15), '°C', DATEADD(minute, @Counter * 5, @StartTime));

        -- Line Pressure (5.5 - 7.0 Range)
        INSERT INTO SensorReadings (SensorName, Value, Unit, Timestamp)
        VALUES ('PRES_LINHA_A2', 5.5 + (RAND() * 1.5), 'bar', DATEADD(minute, @Counter * 5, @StartTime));

        -- Motor Vibration (0.01 - 0.10 Range, with occasional spike)
        INSERT INTO SensorReadings (SensorName, Value, Unit, Timestamp)
        VALUES ('VIB_MOTOR_CNC01', 
                CASE WHEN @Counter = 18 THEN 0.48 ELSE 0.01 + (RAND() * 0.05) END, 
                'mm/s', DATEADD(minute, @Counter * 5, @StartTime));

        SET @Counter = @Counter + 1;
    END
END
GO

PRINT 'Database initialization completed successfully!';
GO
