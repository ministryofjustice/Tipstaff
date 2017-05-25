CREATE TABLE [dbo].[Genders] (
    [genderID]      INT           IDENTITY (1, 1) NOT NULL,
    [detail]        NVARCHAR (50) NOT NULL,
    [active]        BIT           NOT NULL,
    [deactivated]   DATETIME      NULL,
    [deactivatedBy] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([genderID] ASC)
);

