CREATE TABLE [dbo].[SkinColours] (
    [skinColourID]  INT           NOT NULL,
    [Detail]        NVARCHAR (50) NOT NULL,
    [active]        BIT           NOT NULL,
    [deactivated]   DATETIME      NULL,
    [deactivatedBy] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([skinColourID] ASC)
);

