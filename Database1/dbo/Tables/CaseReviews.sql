CREATE TABLE [dbo].[CaseReviews] (
    [caseReviewID]       INT            IDENTITY (1, 1) NOT NULL,
    [reviewDate]         DATETIME       NULL,
    [actionTaken]        NVARCHAR (800) NULL,
    [caseReviewStatusID] INT            NOT NULL,
    [nextReviewDate]     DATETIME       NOT NULL,
    [tipstaffRecordID]   INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([caseReviewID] ASC),
    CONSTRAINT [CaseReview_caseReviewStatus] FOREIGN KEY ([caseReviewStatusID]) REFERENCES [dbo].[CaseReviewStatus] ([caseReviewStatusID]) ON DELETE CASCADE,
    CONSTRAINT [CaseReview_tipstaffRecord] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE
);

