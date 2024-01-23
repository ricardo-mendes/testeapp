CREATE TABLE [dbo].[Employee]
(
	[EmployeeId] BIGINT			NOT NULL	IDENTITY(1,1), 
	[EmployeeUid] UNIQUEIDENTIFIER NOT NULL, 
    [Name] VARCHAR(250) NOT NULL, 
    [Email] VARCHAR(200) NULL, 
    [PhoneNumber] VARCHAR(20) NULL, 
    [CreationTime] DATETIME NULL DEFAULT GETUTCDATE(), 
    [ClientId] BIGINT NOT NULL,

	[IsActive] BIT NOT NULL, 

	CONSTRAINT [PK_Employee] PRIMARY KEY ([EmployeeId]),
    CONSTRAINT [FK_Employee_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client] ([ClientId])
)
GO

CREATE INDEX 
	x_Employee_ClientId ON dbo.Employee( [ClientId] )
GO
