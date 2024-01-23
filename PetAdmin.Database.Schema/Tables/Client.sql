CREATE TABLE [dbo].[Client]
(
	[ClientId]	BIGINT			NOT NULL	IDENTITY(1,1),
	[ClientUid] UNIQUEIDENTIFIER NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [ProfileTypeId] INT NOT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(),

	[PhoneNumber] VARCHAR(20) NULL, 
    [Address] VARCHAR(300) NULL, 
    [Complement] VARCHAR(200) NULL, 
    [Name] VARCHAR(500) NOT NULL, 
    [Email] VARCHAR(200) NULL, 
    [IsActive] BIT NOT NULL, 
    [Neighborhood] VARCHAR(100) NULL, 
    [DocumentInformation] VARCHAR(20) NULL, 
	[DocumentTypeId] INT NULL, 

	CONSTRAINT [PK_Client] PRIMARY KEY ([ClientId]),
    CONSTRAINT [FK_Client_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
)
GO

CREATE INDEX 
	x_Client_UserId ON dbo.Client( [UserId] )
GO