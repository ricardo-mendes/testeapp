CREATE TABLE [dbo].[Schedule]
(
	[ScheduleId] BIGINT NOT NULL	IDENTITY(1,1), 
	[ScheduleUid] UNIQUEIDENTIFIER NOT NULL,
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Period] INT NULL, 
    [Date] DATE NOT NULL, 
    [DateWithHour] DATETIME NOT NULL, 
    [Status] INT NOT NULL, 
    [EmployeeId] BIGINT NOT NULL, 
	[IsActive] BIT NOT NULL, 
	QuantityAllowed INT NOT NULL DEFAULT 0,
	QuantityOccupied INT NOT NULL DEFAULT 0,

	CONSTRAINT [PK_Schedule] PRIMARY KEY ([ScheduleId]),
    CONSTRAINT [FK_Schedule_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]),
	CONSTRAINT [UC_Employee_DateWithHour] UNIQUE ([EmployeeId], [DateWithHour])
)
GO

CREATE INDEX 
	x_Schedule_EmployeeId ON dbo.Schedule( [EmployeeId] )
GO
