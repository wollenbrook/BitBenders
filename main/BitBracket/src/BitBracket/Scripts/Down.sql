IF OBJECT_ID('Announcements', 'U') IS NOT NULL
BEGIN
    DROP TABLE [Announcements];
END


IF OBJECT_ID('Tournaments', 'U') IS NOT NULL
BEGIN
    DROP TABLE [Tournaments];
END

IF OBJECT_ID('BitUser', 'U') IS NOT NULL
BEGIN
    DROP TABLE [BitUser];
END
IF OBJECT_ID('Brackets', 'U') IS NOT NULL
BEGIN
    ALTER TABLE [Brackets] DROP CONSTRAINT [FK_Brackets_Tournaments];
    DROP TABLE [Brackets];
END