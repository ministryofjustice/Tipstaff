﻿CREATE TABLE [dbo].[ContactTypes] (
    [contactTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Detail]        NVARCHAR (50) NOT NULL,
    [active]        BIT           NOT NULL,
    [deactivated]   DATETIME      NULL,
    [deactivatedBy] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([contactTypeID] ASC)
);

