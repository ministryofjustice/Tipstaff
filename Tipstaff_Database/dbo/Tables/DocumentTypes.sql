CREATE TABLE [dbo].[DocumentTypes] (
    [documentTypeID] INT            IDENTITY (1, 1) NOT NULL,
    [Detail]         NVARCHAR (100) NULL,
    [active]         BIT            NOT NULL,
    [deactivated]    DATETIME       NULL,
    [deactivatedBy]  NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([documentTypeID] ASC)
);

