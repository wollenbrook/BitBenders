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