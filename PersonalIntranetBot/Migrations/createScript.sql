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