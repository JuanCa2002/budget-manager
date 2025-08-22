CREATE TABLE [dbo].[AccountTypes] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [UserId] INT           NOT NULL,
    [Order]  INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountTypes_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

