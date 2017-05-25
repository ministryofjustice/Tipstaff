CREATE TABLE [dbo].[Addresses] (
    [addressID]        INT            IDENTITY (1, 1) NOT NULL,
    [addressLine1]     NVARCHAR (100) NOT NULL,
    [addressLine2]     NVARCHAR (100) NULL,
    [addressLine3]     NVARCHAR (100) NULL,
    [town]             NVARCHAR (100) NULL,
    [county]           NVARCHAR (100) NULL,
    [postcode]         NVARCHAR (10)  NULL,
    [phone]            NVARCHAR (20)  NULL,
    [tipstaffRecordID] INT            NOT NULL,
    [addresseeName]    NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([addressID] ASC),
    CONSTRAINT [Address_tipstaffRecord] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE
);

