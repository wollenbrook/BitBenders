CREATE TABLE [BitUser] (
	[ID] INT PRIMARY KEY IDENTITY(1,1),
    [ASPNetIdentityID] NVARCHAR(50) NOT NULL,
	[Username] NVARCHAR(50) NOT NULL,
    [Tag] NVARCHAR(50) NOT NULL,
    [Bio] NVARCHAR(500) NOT NULL,
    [ProfilePicture] VARBINARY(MAX) NULL,
    [EmailConfirmedStatus] BIT NULL
);
CREATE TABLE [Tournaments] (
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [Name] nvarchar(50) NOT NULL,
    [Location] nvarchar(255) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Created] datetime NOT NULL,
    [Owner] int FOREIGN KEY REFERENCES [BitUser]([ID])
);
CREATE TABLE [Brackets] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [BracketData] NVARCHAR(4000) NOT NULL,
    [TournamentID] INT FOREIGN KEY REFERENCES [Tournaments]([ID])
);
CREATE TABLE [Announcements] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(50) NOT NULL, 
    [CreationDate] DATETIME NOT NULL,
    [Description] NVARCHAR(500) NOT NULL, 
    [IsActive] BIT NOT NULL,
    [Author] NVARCHAR(50) NOT NULL 
);