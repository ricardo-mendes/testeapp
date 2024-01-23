CREATE TABLE [dbo].[ScheduleItemEmployee]
(
	[ScheduleItemEmployeeId] BIGINT NOT NULL	IDENTITY(1,1), 
	[ScheduleItemEmployeeUid] UNIQUEIDENTIFIER NOT NULL, 
    [ScheduleItemId] BIGINT NOT NULL, 
    [EmployeeId] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL, 
    [Price] NUMERIC(18, 2) NULL, 
    [Hour] INT NULL, 
    [Minutes] INT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Name] VARCHAR(300) NOT NULL, 

	CONSTRAINT [PK_ScheduleItemEmployee] PRIMARY KEY ([ScheduleItemEmployeeId]),
    CONSTRAINT [FK_ScheduleItemEmployee_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
	CONSTRAINT [FK_ScheduleItemEmployee_ScheduleItem] FOREIGN KEY ([ScheduleItemId]) REFERENCES [dbo].[ScheduleItem] ([ScheduleItemId])
)
GO

CREATE INDEX 
	x_ScheduleItemEmployee_EmployeeId ON dbo.ScheduleItemEmployee( [EmployeeId] )
GO

CREATE INDEX 
	x_ScheduleItemEmployee_ScheduleItemId ON dbo.ScheduleItemEmployee( [ScheduleItemId] )
GO

