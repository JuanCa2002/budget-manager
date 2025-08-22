CREATE TABLE [dbo].[Categories] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [TransactionTypeId] INT           NOT NULL,
    [UserId]            INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Categories_TransactionTypes] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionTypes] ([Id]),
    CONSTRAINT [FK_Categories_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

