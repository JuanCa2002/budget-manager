CREATE TABLE [dbo].[Users] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    [LastName]      NVARCHAR (100) NOT NULL,
    [Email]         NVARCHAR (256) NOT NULL,
    [StandardEmail] NVARCHAR (256) NOT NULL,
    [PasswordHash]  NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

