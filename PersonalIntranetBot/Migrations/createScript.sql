IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Attendees] (
    [AttendeeId] int NOT NULL IDENTITY,
    [EmailAddress] nvarchar(max) NULL,
    [IsAsPerson] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Attendees] PRIMARY KEY ([AttendeeId])
);

GO

CREATE TABLE [SocialLinks] (
    [SocialLinkId] int NOT NULL IDENTITY,
    [AttendeeId] int NULL,
    [Type] int NOT NULL,
    [URL] nvarchar(max) NULL,
    CONSTRAINT [PK_SocialLinks] PRIMARY KEY ([SocialLinkId]),
    CONSTRAINT [FK_SocialLinks_Attendees_AttendeeId] FOREIGN KEY ([AttendeeId]) REFERENCES [Attendees] ([AttendeeId]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_SocialLinks_AttendeeId] ON [SocialLinks] ([AttendeeId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181118202104_InitialCreate', N'2.0.0-rtm-26452');

GO

ALTER TABLE [SocialLinks] DROP CONSTRAINT [FK_SocialLinks_Attendees_AttendeeId];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Attendees') AND [c].[name] = N'IsAsPerson');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Attendees] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Attendees] DROP COLUMN [IsAsPerson];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Attendees') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Attendees] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Attendees] DROP COLUMN [Name];

GO

DROP INDEX [IX_SocialLinks_AttendeeId] ON [SocialLinks];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'SocialLinks') AND [c].[name] = N'AttendeeId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [SocialLinks] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [SocialLinks] ALTER COLUMN [AttendeeId] int NOT NULL;
CREATE INDEX [IX_SocialLinks_AttendeeId] ON [SocialLinks] ([AttendeeId]);

GO

ALTER TABLE [Attendees] ADD [CurrentJobCompany] nvarchar(max) NULL;

GO

ALTER TABLE [Attendees] ADD [CurrentJobTitle] nvarchar(max) NULL;

GO

ALTER TABLE [Attendees] ADD [DisplayName] nvarchar(max) NULL;

GO

ALTER TABLE [Attendees] ADD [EducationLocation] nvarchar(max) NULL;

GO

ALTER TABLE [Attendees] ADD [ImageURL] nvarchar(max) NULL;

GO

ALTER TABLE [Attendees] ADD [IsPerson] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [SocialLinks] ADD CONSTRAINT [FK_SocialLinks_Attendees_AttendeeId] FOREIGN KEY ([AttendeeId]) REFERENCES [Attendees] ([AttendeeId]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181202145344_AttendeeDetailsAdded', N'2.0.0-rtm-26452');

GO

ALTER TABLE [Attendees] ADD [LastUpdated] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.000';

GO

ALTER TABLE [Attendees] ADD [UpdatedBy] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181207161153_LastUpdatedAndUpdatedByColumnsAdded', N'2.0.0-rtm-26452');

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Attendees') AND [c].[name] = N'UpdatedBy');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Attendees] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Attendees] DROP COLUMN [UpdatedBy];

GO

ALTER TABLE [Attendees] ADD [LastUpdatedBy] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181207162758_LastUpdateByRenamed', N'2.0.0-rtm-26452');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181212123115_MeetingCommentsAdded', N'2.0.0-rtm-26452');

GO

CREATE TABLE [MeetingsCommments] (
    [MeetingCommentId] int NOT NULL IDENTITY,
    [Comment] nvarchar(max) NULL,
    [LastUpdated] datetime2 NOT NULL,
    [LastUpdatedBy] nvarchar(max) NULL,
    [MeetingId] int NOT NULL,
    CONSTRAINT [PK_MeetingsCommments] PRIMARY KEY ([MeetingCommentId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181212123350_MeetingCommentsTableAdded', N'2.0.0-rtm-26452');

GO

ALTER TABLE [MeetingsCommments] DROP CONSTRAINT [PK_MeetingsCommments];

GO

EXEC sp_rename N'MeetingsCommments', N'MeetingsComments';

GO

ALTER TABLE [MeetingsComments] ADD CONSTRAINT [PK_MeetingsComments] PRIMARY KEY ([MeetingCommentId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181212123444_MeetingCommentsTableRenamed', N'2.0.0-rtm-26452');

GO

ALTER TABLE [MeetingsComments] DROP CONSTRAINT [PK_MeetingsComments];

GO

EXEC sp_rename N'MeetingsComments', N'MeetingComments';

GO

ALTER TABLE [MeetingComments] ADD CONSTRAINT [PK_MeetingComments] PRIMARY KEY ([MeetingCommentId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181212123522_MeetingCommentsTableRenamedAgain', N'2.0.0-rtm-26452');

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'MeetingComments') AND [c].[name] = N'MeetingId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MeetingComments] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [MeetingComments] ALTER COLUMN [MeetingId] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181212135256_MeetingCommentsMeetingIdChangeType', N'2.0.0-rtm-26452');

GO

