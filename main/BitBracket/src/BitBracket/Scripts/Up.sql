CREATE TABLE [Tournament] (
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [Name] nvarchar(50) NOT NULL,
    [Location] nvarchar(255) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Brackets] nvarchar(50) NOT NULL,
    [Created] datetime NOT NULL
);

CREATE TABLE [Announcements] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Title] NVARCHAR(255) NOT NULL,
    [CreationDate] DATETIME NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT NOT NULL,
    [Author] NVARCHAR(50) NOT NULL
);

CREATE TABLE [BitUser] (
	[ID] INT PRIMARY KEY IDENTITY(1,1),
    [ASPNetIdentityID] NVARCHAR(50) NOT NULL,
	[Username] NVARCHAR(50) NOT NULL,
    [Tag] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,

);