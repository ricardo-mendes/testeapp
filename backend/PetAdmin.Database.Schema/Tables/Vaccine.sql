CREATE TABLE [dbo].[Vaccine]
(
	[VaccineId]	BIGINT			NOT NULL	IDENTITY(1,1),
	[VaccineUid] UNIQUEIDENTIFIER NOT NULL, 
	[PetId] BIGINT NOT NULL, 
    [Name] VARCHAR(200) NOT NULL, 
    [Date] DATETIME NOT NULL, 
	[RevaccineDate] DATETIME NULL, 
	[ClinicName] VARCHAR(200) NULL,
	[Note] VARCHAR(500) NULL, 
    [CreationTime] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [IsActive] BIT NOT NULL default 1, 
	[PhotoUrl] VARCHAR(300) NULL,

	CONSTRAINT [PK_Vaccine] PRIMARY KEY ([VaccineId]),
    CONSTRAINT [FK_Vaccine_Pet] FOREIGN KEY ([PetId]) REFERENCES [dbo].[Pet] ([PetId])
)
GO

CREATE INDEX 
	x_Vaccine_PetId ON dbo.Vaccine( [PetId] )
GO