CREATE TABLE [dbo].[ScheduleItemClient]
(
	[ScheduleItemClientId] BIGINT NOT NULL	IDENTITY(1,1), 
	[ScheduleItemClientUid] UNIQUEIDENTIFIER NOT NULL, 
    [ScheduleItemId] BIGINT NOT NULL, 
    [ClientId] BIGINT NOT NULL,
	[IsActive] BIT NOT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 

	CONSTRAINT [PK_ScheduleItemClient] PRIMARY KEY ([ScheduleItemClientId]),
    CONSTRAINT [FK_ScheduleItemClient_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client] ([ClientId]),
	CONSTRAINT [FK_ScheduleItemClient_ScheduleItem] FOREIGN KEY ([ScheduleItemId]) REFERENCES [dbo].[ScheduleItem] ([ScheduleItemId])
)
GO

CREATE INDEX 
	x_ScheduleItemClient_ClientId ON dbo.ScheduleItemClient( [ClientId] )
GO

CREATE INDEX 
	x_ScheduleItemClient_ScheduleItemId ON dbo.ScheduleItemClient( [ScheduleItemId] )
GO

