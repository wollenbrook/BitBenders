-- Drop foreign key constraints
ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];

-- Drop tables
DROP TABLE [AspNetUserTokens];
DROP TABLE [AspNetUserRoles];
DROP TABLE [AspNetUserLogins];
DROP TABLE [AspNetUserClaims];
DROP TABLE [AspNetRoleClaims];
DROP TABLE [AspNetUsers];
DROP TABLE [AspNetRoles];
DROP TABLE [__EFMigrationsHistory];