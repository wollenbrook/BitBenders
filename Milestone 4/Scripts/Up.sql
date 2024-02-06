CREATE TABLE [Tournament] (
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [Name] nvarchar(50) NOT NULL,
    [Location] nvarchar(255) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Brackets] nvarchar(50) NOT NULL,
    [Created] datetime NOT NULL
);

CREATE TABLE [Announcements] (
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [Time] datetime NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Author] nvarchar(50) NOT NULL
);