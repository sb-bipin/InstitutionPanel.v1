--Start : Initiated db migration script for EF Core version 9.0.12 --Bipin Poudel-- 2026 Jan 21

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260121170035_init', N'9.0.12');

COMMIT;
GO

--End : Initiated db migration script for EF Core version 9.0.12 --Bipin Poudel-- 2026 Jan 21


--Start : New fields added --Bipin Poudel-- 2026 Jan 23

ALTER TABLE [AspNetUsers] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [AspNetUsers] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [AspNetUsers] ADD [FirstName] nvarchar(max) NULL;

ALTER TABLE [AspNetUsers] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [AspNetUsers] ADD [LastName] nvarchar(max) NULL;

--End : New fields added --Bipin Poudel-- 2026 Jan 23

--Start : New fields added --Bipin Poudel-- 2026 Jan 24

ALTER TABLE [AspNetUsers] ADD [InstitutionId] uniqueidentifier NULL;

ALTER TABLE [AspNetUsers] ADD [UpdatedAt] nvarchar(max) NULL DEFAULT N'';

ALTER TABLE [AspNetUsers] ADD [UpdatedBy] nvarchar(max) NULL DEFAULT N'';

CREATE TABLE [Institutions] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NULL,
    [Description] nvarchar(500) NOT NULL,
    [Address] nvarchar(100) NOT NULL,
    [ContactNo] nvarchar(9) NULL,
    [PhoneNo] nvarchar(10) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] nvarchar(max) NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Institutions] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_AspNetUsers_InstitutionId] ON [AspNetUsers] ([InstitutionId]);

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Institutions_InstitutionId] FOREIGN KEY ([InstitutionId]) REFERENCES [Institutions] ([Id]);


DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
UPDATE [AspNetUsers] SET [Email] = N'' WHERE [Email] IS NULL;
ALTER TABLE [AspNetUsers] ALTER COLUMN [Email] nvarchar(256) NOT NULL;
ALTER TABLE [AspNetUsers] ADD DEFAULT N'' FOR [Email];


DROP INDEX [EmailIndex] ON [AspNetUsers];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'NormalizedEmail');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
UPDATE [AspNetUsers] SET [NormalizedEmail] = N'' WHERE [NormalizedEmail] IS NULL;
ALTER TABLE [AspNetUsers] ALTER COLUMN [NormalizedEmail] nvarchar(256) NOT NULL;
ALTER TABLE [AspNetUsers] ADD DEFAULT N'' FOR [NormalizedEmail];
CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

--End : New fields added --Bipin Poudel-- 2026 Jan 24


--Start : UserType is added --Bipin Poudel-- 2026 Jan 26

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'UpdatedBy');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [UpdatedBy] nvarchar(max) NULL;

ALTER TABLE [AspNetUsers] ADD [UserType] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);

--End : UserType is added --Bipin Poudel-- 2026 Jan 26


--Start :Make Modified date and by flag of common entites as nullable --Bipin Poudel-- 2026 Jan 27

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Institutions]') AND [c].[name] = N'UpdatedBy');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Institutions] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Institutions] ALTER COLUMN [UpdatedBy] nvarchar(max) NULL;

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Institutions]') AND [c].[name] = N'UpdatedAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Institutions] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Institutions] ALTER COLUMN [UpdatedAt] nvarchar(max) NULL;

--End :Make Modified date and by flag of common entites as nullable --Bipin Poudel-- 2026 Jan 27

--Start :Add image gallery entity --Bipin Poudel-- 2026 Feb 1


CREATE TABLE [ImageGalleries] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(30) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [InstitutionId] uniqueidentifier NOT NULL,
    [CreatedAt] nvarchar(max) NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_ImageGalleries] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ImageGalleries_Institutions_InstitutionId] FOREIGN KEY ([InstitutionId]) REFERENCES [Institutions] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_ImageGalleries_InstitutionId] ON [ImageGalleries] ([InstitutionId]);

--Start :Add image gallery entity --Bipin Poudel-- 2026 Feb 1
