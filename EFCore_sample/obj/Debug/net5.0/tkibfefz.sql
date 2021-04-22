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
GO

CREATE TABLE [Guild] (
    [GuildId] int NOT NULL IDENTITY,
    [GuildName] nvarchar(max) NULL,
    CONSTRAINT [PK_Guild] PRIMARY KEY ([GuildId])
);
GO

CREATE TABLE [Player] (
    [PlayerId] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    [GuildId] int NULL,
    CONSTRAINT [PK_Player] PRIMARY KEY ([PlayerId]),
    CONSTRAINT [FK_Player_Guild_GuildId] FOREIGN KEY ([GuildId]) REFERENCES [Guild] ([GuildId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Item] (
    [ItemId] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [SoftDelete] bit NOT NULL,
    [TemplateId] int NOT NULL,
    [CreateDate] datetime2 NOT NULL DEFAULT (GETDATE()),
    [TestOwnerId] int NOT NULL,
    [TestCreatorId] int NULL,
    [RecoveredDate] datetime2 NOT NULL,
    [DestroyDate] datetime2 NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY ([ItemId]),
    CONSTRAINT [FK_Item_Player_TestCreatorId] FOREIGN KEY ([TestCreatorId]) REFERENCES [Player] ([PlayerId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Item_Player_TestOwnerId] FOREIGN KEY ([TestOwnerId]) REFERENCES [Player] ([PlayerId]) ON DELETE CASCADE
);
GO

CREATE TABLE [ItemOption] (
    [ItemId] int NOT NULL,
    [Str] int NOT NULL,
    [Dex] int NOT NULL,
    [Hp] int NOT NULL,
    CONSTRAINT [PK_ItemOption] PRIMARY KEY ([ItemId]),
    CONSTRAINT [FK_ItemOption_Item_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Item] ([ItemId]) ON DELETE CASCADE
);
GO

CREATE TABLE [ItemReview] (
    [ItemReviewId] int NOT NULL IDENTITY,
    [Score] int NOT NULL,
    [ItemId] int NULL,
    CONSTRAINT [PK_ItemReview] PRIMARY KEY ([ItemReviewId]),
    CONSTRAINT [FK_ItemReview_Item_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [Item] ([ItemId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Item_TestCreatorId] ON [Item] ([TestCreatorId]);
GO

CREATE UNIQUE INDEX [IX_Item_TestOwnerId] ON [Item] ([TestOwnerId]);
GO

CREATE INDEX [IX_ItemReview_ItemId] ON [ItemReview] ([ItemId]);
GO

CREATE UNIQUE INDEX [Index_Person_Name] ON [Player] ([Name]);
GO

CREATE INDEX [IX_Player_GuildId] ON [Player] ([GuildId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210422004733_Hello-Migration', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Item] ADD [ItemGrade] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210422005922_ItemGrade', N'5.0.5');
GO

COMMIT;
GO

