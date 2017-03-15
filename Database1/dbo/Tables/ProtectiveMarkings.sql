CREATE TABLE [dbo].[ProtectiveMarkings] (
    [protectiveMarkingID] INT           IDENTITY (1, 1) NOT NULL,
    [Detail]              NVARCHAR (15) NOT NULL,
    [active]              BIT           NOT NULL,
    [deactivated]         DATETIME      NULL,
    [deactivatedBy]       NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([protectiveMarkingID] ASC)
);

