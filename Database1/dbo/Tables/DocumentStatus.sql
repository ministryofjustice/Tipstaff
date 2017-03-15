CREATE TABLE [dbo].[DocumentStatus] (
    [DocumentStatusID] INT           IDENTITY (1, 1) NOT NULL,
    [Detail]           NVARCHAR (40) NOT NULL,
    [active]           BIT           NOT NULL,
    [deactivated]      DATETIME      NULL,
    [deactivatedBy]    NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([DocumentStatusID] ASC)
);

