CREATE TABLE [dbo].[DeletedTipstaffRecords] (
    [TipstaffRecordID] INT           NOT NULL,
    [deletedReasonID]  INT           NOT NULL,
    [discriminator]    NVARCHAR (50) NOT NULL,
    [UniqueRecordID]   NVARCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([TipstaffRecordID] ASC, [deletedReasonID] ASC),
    CONSTRAINT [DeletedTipstaffRecord_DeletedReason] FOREIGN KEY ([deletedReasonID]) REFERENCES [dbo].[DeletedReasons] ([deletedReasonID])
);

