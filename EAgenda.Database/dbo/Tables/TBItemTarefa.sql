CREATE TABLE [dbo].[TBItemTarefa] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Titulo]   NVARCHAR (100)   NOT NULL,
    [Status]   NVARCHAR (20)    NOT NULL,
    [TarefaId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

ALTER TABLE [dbo].[TBItemTarefa]
    ADD CONSTRAINT [FK_TBItemTarefa_TBTarefa] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[TBTarefa] ([Id]) ON DELETE CASCADE;
GO

