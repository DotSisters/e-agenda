CREATE TABLE [dbo].[TBCompromisso] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Assunto]        NVARCHAR (100)   NOT NULL,
    [DataOcorrencia] DATE             NOT NULL,
    [HoraInicio]     TIME (0)         NOT NULL,
    [HoraTermino]    TIME (0)         NOT NULL,
    [Tipo]           NVARCHAR (20)    NOT NULL,
    [Local]          NVARCHAR (100)   NULL,
    [Link]           NVARCHAR (200)   NULL,
    [ContatoId]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [FK_TBCompromisso_TBContato] FOREIGN KEY ([ContatoId]) REFERENCES [dbo].[TBContato] ([Id]);
GO

