CREATE TABLE [dbo].[AttendanceNotes] (
    [AttendanceNoteID]     INT             IDENTITY (1, 1) NOT NULL,
    [callDated]            DATETIME        NOT NULL,
    [callStarted]          DATETIME        NULL,
    [callEnded]            DATETIME        NULL,
    [callDetails]          NVARCHAR (1000) NOT NULL,
    [AttendanceNoteCodeID] INT             NOT NULL,
    [tipstaffRecordID]     INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([AttendanceNoteID] ASC),
    CONSTRAINT [AttendanceNote_tipstaffRecord] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE,
    CONSTRAINT [AttendanceNoteCode_AttendanceNotes] FOREIGN KEY ([AttendanceNoteCodeID]) REFERENCES [dbo].[AttendanceNoteCodes] ([AttendanceNoteCodeID]) ON DELETE CASCADE
);

