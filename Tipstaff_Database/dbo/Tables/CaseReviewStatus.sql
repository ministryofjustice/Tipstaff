﻿CREATE TABLE [dbo].[CaseReviewStatus] (
    [caseReviewStatusID] INT           IDENTITY (1, 1) NOT NULL,
    [Detail]             NVARCHAR (20) NOT NULL,
    [active]             BIT           NOT NULL,
    [deactivated]        DATETIME      NULL,
    [deactivatedBy]      NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([caseReviewStatusID] ASC)
);

