CREATE TABLE [dbo].[Users]
(
	[UserId] BIGINT NOT NULL	IDENTITY(1,1),
	[UserUid] UNIQUEIDENTIFIER NOT NULL,
    [Email] VARCHAR(200) NOT NULL, 
    [UserLogin] VARCHAR(200) NOT NULL, 
    [PasswordHash] NVARCHAR(512) NOT NULL, 
    [PasswordSalt] NVARCHAR(512) NOT NULL, 
    [UserTypeId] INT NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [Name] VARCHAR(500) NOT NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(),

	CONSTRAINT [PK_User] PRIMARY KEY ([UserId]),
    CONSTRAINT [UK_UserPassword] UNIQUE ([UserId],[PasswordHash]),
	CONSTRAINT [UK_UserUserName] UNIQUE ([UserId],[UserLogin]),
	CONSTRAINT [UK_UserEmail] UNIQUE ([UserId],[Email])
)
