CREATE TABLE [dbo].[PetLover]
(
	[PetLoverId] BIGINT NOT NULL	IDENTITY(1,1), 
	[PetLoverUid] UNIQUEIDENTIFIER NOT NULL,
    [UserId] BIGINT NULL, 
	[ClientId] BIGINT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(),

	[PhoneNumber] VARCHAR(20) NULL, 
    [Address] VARCHAR(300) NULL, 
    [Complement] VARCHAR(150) NULL, 
    [Name] VARCHAR(500) NOT NULL, 
    [Email] VARCHAR(200) NULL, 
    [IsActive] BIT NOT NULL, 
    [Neighborhood] VARCHAR(100) NULL,
	[Gender] INT NULL,
	[IsClub] BIT NOT NULL default(0),

	CONSTRAINT [PK_PetLover] PRIMARY KEY ([PetLoverId]),
    CONSTRAINT [FK_PetLover_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
	CONSTRAINT [FK_PetLover_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].Client ([ClientId])
)
GO

CREATE INDEX 
	x_PetLover_UserId ON dbo.PetLover( [UserId] )
GO

CREATE INDEX 
	x_PetLover_ClientId ON dbo.PetLover( [ClientId] )
GO
