-- Create BitUser table
CREATE TABLE [BitUser] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [ASPNetIdentityID] NVARCHAR(50) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL,
    [Tag] NVARCHAR(50) NOT NULL,
    [Bio] NVARCHAR(500) NOT NULL,
    [ProfilePicture] VARBINARY(MAX) NULL,
    [EmailConfirmedStatus] BIT NULL,
    [OptInConfirmation] BIT NOT NULL DEFAULT 1
);

-- Create Tournaments table
CREATE TABLE [Tournaments] (
    [ID] INT PRIMARY KEY IDENTITY(1, 1),
    [Name] NVARCHAR(50) NOT NULL,
    [Location] NVARCHAR(255) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [Created] DATETIME NOT NULL,
    [Owner] INT FOREIGN KEY REFERENCES [BitUser]([ID])
);

-- Create UserAnnouncements table
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

-- Create Announcements table
CREATE TABLE [Announcements] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(50) NOT NULL,
    [CreationDate] DATETIME NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [IsActive] BIT NOT NULL,
    [Author] NVARCHAR(50) NOT NULL,
);

-- Create Brackets table
CREATE TABLE [Brackets] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [BracketData] NVARCHAR(4000) NOT NULL,
    [TournamentID] INT FOREIGN KEY REFERENCES [Tournaments]([ID])
);

-- Create SentFriendRequests table
CREATE TABLE [SentFriendRequests] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [SenderID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [ReceiverID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [Status] NVARCHAR(50) NOT NULL
);

-- Create ReceivedFriendRequests table
CREATE TABLE [ReceivedFriendRequests] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [SenderID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [Status] NVARCHAR(50) NOT NULL
);

-- Create Friends table
CREATE TABLE [Friends] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT FOREIGN KEY REFERENCES [BitUser]([ID]),
    [FriendID] INT FOREIGN KEY REFERENCES [BitUser]([ID])
);
