CREATE TABLE [dbo].[TBTarefa] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Titulo]              NVARCHAR (100)   NOT NULL,
    [Prioridade]          NVARCHAR (20)    NOT NULL,
    [DataCriacao]         DATE             NOT NULL,
    [DataConclusao]       DATE             NULL,
    [Status]              NVARCHAR (20)    NOT NULL,
    [PercentualConcluido] DECIMAL (5, 2)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

