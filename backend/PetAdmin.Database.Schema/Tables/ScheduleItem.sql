CREATE TABLE [dbo].[ScheduleItem]
(
	[ScheduleItemId] BIGINT NOT NULL	IDENTITY(1,1), 
	[ScheduleItemUid] UNIQUEIDENTIFIER NOT NULL,
    [Name] VARCHAR(300) NOT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [IsActive] BIT NOT NULL,

	CONSTRAINT [PK_ScheduleItem] PRIMARY KEY ([ScheduleItemId])
)
