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
CREATE TABLE [GuidBracket] (
    [ID] INT PRIMARY KEY IDENTITY(1, 1),
    [Guid] UNIQUEIDENTIFIER NOT NULL,
    [BracketData] VARCHAR(4000) NOT NULL

CREATE TABLE [FriendRequests] (
	[ID] INT IDENTITY(1,1) PRIMARY KEY,
	[SenderID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[ReceiverID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[Status] NVARCHAR(50) NOT NULL
);

CREATE TABLE [UserAnnouncements] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(50) NOT NULL,
    [CreationDate] DATETIME NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [IsDraft] BIT NOT NULL DEFAULT 0,
    [Author] NVARCHAR(50) NOT NULL,
    [Owner] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [TournamentID] INT NULL FOREIGN KEY REFERENCES [Tournaments]([ID])
);

CREATE TABLE [SentFriendRequests] (
	[ID] INT IDENTITY(1,1) PRIMARY KEY,
	[SenderID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[ReceiverID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[Status] NVARCHAR(50) NOT NULL
);

CREATE TABLE [RecievedFriendRequests] (
	[ID] INT IDENTITY(1,1) PRIMARY KEY,
	[SenderID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[Status] NVARCHAR(50) NOT NULL
);

CREATE TABLE [Friends] (
	[ID] INT IDENTITY(1,1) PRIMARY KEY,
	[UserID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
	[FriendID] INT FOREIGN KEY REFERENCES [BitUser]([ID])
);

-- Create JoinedPlayers table
CREATE TABLE [JoinedPlayers] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [PlayerId] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [TournamentId] INT FOREIGN KEY REFERENCES [Tournaments]([ID])
);

-- Create SentJoinRequests table
CREATE TABLE [SentJoinRequests] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [SenderId] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [TournamentId] INT FOREIGN KEY REFERENCES [Tournaments]([ID]),
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending'
);

-- Create ReceivedPlayerRequests table
CREATE TABLE [ReceivedPlayerRequests] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [ReceiverId] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [TournamentId] INT FOREIGN KEY REFERENCES [Tournaments]([ID]),
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending'
);