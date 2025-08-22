CREATE TABLE [dbo].[TransactionTypes] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

