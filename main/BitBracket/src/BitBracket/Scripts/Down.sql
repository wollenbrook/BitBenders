IF OBJECT_ID('Announcements', 'U') IS NOT NULL
BEGIN
    DROP TABLE [Announcements];
END
IF OBJECT_ID('Brackets', 'U') IS NOT NULL
BEGIN
    ALTER TABLE [Brackets] DROP CONSTRAINT [FK_Brackets_Tournaments];
    DROP TABLE [Brackets];
END

IF OBJECT_ID('Tournaments', 'U') IS NOT NULL
BEGIN
	ALTER TABLE [Tournaments] DROP CONSTRAINT [FK_Tournaments_BitUser_Owner];
    DROP TABLE [Tournaments];

END

IF OBJECT_ID('Friends', 'U') IS NOT NULL
BEGIN
    ALTER TABLE [Friends] DROP CONSTRAINT [FK_Friends_BitUser_UserID];
    ALTER TABLE [Friends] DROP CONSTRAINT [FK_Friends_BitUser_FriendID];
    DROP TABLE [Friends];
END

IF OBJECT_ID('RecievedFriendRequests', 'U') IS NOT NULL
BEGIN
    ALTER TABLE [RecievedFriendRequests] DROP CONSTRAINT [FK_RecievedFriendRequests_BitUser_SenderID];
    DROP TABLE [RecievedFriendRequests];
END

IF OBJECT_ID('SentFriendRequests', 'U') IS NOT NULL
BEGIN
    ALTER TABLE [SentFriendRequests] DROP CONSTRAINT [FK_SentFriendRequests_BitUser_SenderID];
    ALTER TABLE [SentFriendRequests] DROP CONSTRAINT [FK_SentFriendRequests_BitUser_ReceiverID];
    DROP TABLE [SentFriendRequests];
END

IF OBJECT_ID('BitUser', 'U') IS NOT NULL
BEGIN
    DROP TABLE [BitUser];
END

DROP TABLE [GuidBracket];
