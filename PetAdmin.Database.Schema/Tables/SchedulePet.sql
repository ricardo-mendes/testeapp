CREATE TABLE [dbo].[SchedulePet]
(
	[SchedulePetId] BIGINT NOT NULL	IDENTITY(1,1), 
	[SchedulePetUid] UNIQUEIDENTIFIER NOT NULL,
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Note] VARCHAR(500) NULL, 
    [Status] INT NOT NULL, 
    [PetLoverId] BIGINT NULL, 
    [PetId] BIGINT NULL, 
    [ScheduleId] BIGINT NULL,
	[ScheduleItemEmployeeId] BIGINT NULL,

	CONSTRAINT [PK_SchedulePet] PRIMARY KEY ([SchedulePetId]),
    CONSTRAINT [FK_SchedulePet_Schedule] FOREIGN KEY ([ScheduleId]) REFERENCES [dbo].[Schedule] ([ScheduleId]),
	CONSTRAINT [FK_SchedulePet_PetLover] FOREIGN KEY ([PetLoverId]) REFERENCES [dbo].[PetLover] ([PetLoverId]),
	CONSTRAINT [FK_SchedulePet_Pet] FOREIGN KEY ([PetId]) REFERENCES [dbo].[Pet] ([PetId]),
	CONSTRAINT [FK_SchedulePet_ScheduleItemEmployee] FOREIGN KEY ([ScheduleItemEmployeeId]) REFERENCES [dbo].[ScheduleItemEmployee] ([ScheduleItemEmployeeId])
)
GO

CREATE INDEX 
	x_SchedulePet_ScheduleId ON dbo.SchedulePet( [ScheduleId] )
GO

CREATE INDEX 
	x_SchedulePet_PetLoverId ON dbo.SchedulePet( [PetLoverId] )
GO

CREATE INDEX 
	x_SchedulePet_PetId ON dbo.SchedulePet( [PetId] )
GO

CREATE INDEX 
	x_SchedulePet_ScheduleItemEmployeeId ON dbo.SchedulePet( [ScheduleItemEmployeeId] )
GO
