CREATE TABLE [dbo].[Pet]
(
	[PetId] BIGINT			NOT NULL	IDENTITY(1,1),
	[PetUid] UNIQUEIDENTIFIER NOT NULL, 
    [PetLoverId] BIGINT NOT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [PetTypeId] INT NULL, 
    [Breed] VARCHAR(100) NULL, 
    [BirthDate] DATETIME NULL, 
    [Gender] INT NULL, 
    [Size] INT NULL, 
    [Coat] INT NULL, 
    [Name] VARCHAR(200) NOT NULL,
	[Note] VARCHAR(600) NULL, 
    [IsActive] BIT NOT NULL,
	[IsClub] BIT NOT NULL default(0),

	[PhotoUrl] VARCHAR(300) NULL, 
    CONSTRAINT [PK_Pet] PRIMARY KEY ([PetId]),
    CONSTRAINT [FK_Pet_PetLover] FOREIGN KEY ([PetLoverId]) REFERENCES [dbo].[PetLover] ([PetLoverId])
)
GO

CREATE INDEX 
	x_Pet_PetLoverId ON dbo.Pet( [PetLoverId] )
GO
