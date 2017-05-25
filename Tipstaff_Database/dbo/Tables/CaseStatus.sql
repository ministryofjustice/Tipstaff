CREATE TABLE [dbo].[CaseStatus] (
    [caseStatusID]  INT           IDENTITY (1, 1) NOT NULL,
    [Detail]        NVARCHAR (30) NOT NULL,
    [active]        BIT           NOT NULL,
    [deactivated]   DATETIME      NULL,
    [deactivatedBy] NVARCHAR (50) NULL,
    [sequence]      INT           NULL,
    PRIMARY KEY CLUSTERED ([caseStatusID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Sequence]
    ON [dbo].[CaseStatus]([sequence] ASC);

