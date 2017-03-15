CREATE TABLE [dbo].[SolicitorFirms] (
    [solicitorFirmID] INT            IDENTITY (1, 1) NOT NULL,
    [firmName]        NVARCHAR (50)  NOT NULL,
    [addressLine1]    NVARCHAR (100) NOT NULL,
    [addressLine2]    NVARCHAR (100) NULL,
    [addressLine3]    NVARCHAR (100) NULL,
    [town]            NVARCHAR (100) NULL,
    [county]          NVARCHAR (100) NULL,
    [postcode]        NVARCHAR (10)  NULL,
    [DX]              NVARCHAR (50)  NULL,
    [phoneDayTime]    NVARCHAR (20)  NULL,
    [phoneOutofHours] NVARCHAR (20)  NULL,
    [email]           NVARCHAR (60)  NULL,
    [active]          BIT            DEFAULT (1) NOT NULL,
    [deactivated]     DATETIME       NULL,
    [deactivatedBy]   NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([solicitorFirmID] ASC)
);

