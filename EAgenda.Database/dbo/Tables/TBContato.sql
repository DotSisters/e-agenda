CREATE TABLE [dbo].[TBContato] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Nome]     NVARCHAR (100)   NOT NULL,
    [Email]    NVARCHAR (100)   NOT NULL,
    [Telefone] NVARCHAR (18)    NOT NULL,
    [Cargo]    NVARCHAR (100)   NULL,
    [Empresa]  NVARCHAR (100)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

