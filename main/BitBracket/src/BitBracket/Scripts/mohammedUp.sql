
-- Create BitUser Table
CREATE TABLE [BitUser] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [ASPNetIdentityID] NVARCHAR(450) NOT NULL,
    [Username] NVARCHAR(256) NOT NULL,
    [Tag] NVARCHAR(256) NULL,
    [Bio] NVARCHAR(MAX) NULL,
    [ProfilePicture] VARBINARY(MAX) NULL,
    [EmailConfirmedStatus] BIT NOT NULL DEFAULT 0,
    [OptInConfirmation] BIT NOT NULL DEFAULT 1
);
GO

-- Create Tournament Table (if applicable)
CREATE TABLE [Tournaments] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(256) NOT NULL,
    [Location] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Owner] INT NOT NULL,
    CONSTRAINT [FK_Tournaments_BitUser] FOREIGN KEY ([Owner]) REFERENCES [BitUser]([ID])
);
GO

-- Create Announcement Table
CREATE TABLE [Announcements] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(256) NOT NULL,
    [CreationDate] DATETIME2 NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsDraft] BIT NOT NULL DEFAULT 0,
    [Author] NVARCHAR(256) NOT NULL,
    [TournamentId] INT NULL, -- Nullable for announcements not tied to a specific tournament
    [BitUserId] INT NOT NULL, -- Foreign key to BitUser
    CONSTRAINT [FK_Announcements_Tournaments] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments]([ID]),
    CONSTRAINT [FK_Announcements_BitUser] FOREIGN KEY ([BitUserId]) REFERENCES [BitUser]([ID])
);
GO

CREATE TABLE [Brackets] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [BracketData] NVARCHAR(4000) NOT NULL,
    [TournamentID] INT FOREIGN KEY REFERENCES [Tournaments]([ID])
);
GO
