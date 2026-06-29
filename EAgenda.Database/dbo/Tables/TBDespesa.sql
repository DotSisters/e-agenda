CREATE TABLE [dbo].[TBDespesa] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Descricao]  NVARCHAR (100)   NOT NULL,
    [Ocorrencia] DATE             NOT NULL,
    [Valor]      DECIMAL (18, 2)  NOT NULL,
    [Pagamento]  NVARCHAR (20)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

