CREATE TABLE [dbo].[TipstaffRecordSolicitors] (
    [tipstaffRecordID] INT NOT NULL,
    [solicitorID]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([tipstaffRecordID] ASC, [solicitorID] ASC),
    CONSTRAINT [TipstaffRecord_LinkedSolicitors] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE,
    CONSTRAINT [TipstaffRecordSolicitor_solicitor] FOREIGN KEY ([solicitorID]) REFERENCES [dbo].[Solicitors] ([solicitorID]) ON DELETE CASCADE
);

