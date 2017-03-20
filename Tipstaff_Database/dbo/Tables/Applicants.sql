CREATE TABLE [dbo].[Applicants] (
    [ApplicantID]      INT            IDENTITY (1, 1) NOT NULL,
    [salutationID]     INT            NOT NULL,
    [nameLast]         NVARCHAR (50)  NOT NULL,
    [nameFirst]        NVARCHAR (50)  NULL,
    [addressLine1]     NVARCHAR (100) NOT NULL,
    [addressLine2]     NVARCHAR (100) NULL,
    [addressLine3]     NVARCHAR (100) NULL,
    [town]             NVARCHAR (100) NULL,
    [county]           NVARCHAR (100) NULL,
    [postcode]         NVARCHAR (10)  NOT NULL,
    [phone]            NVARCHAR (20)  NULL,
    [tipstaffRecordID] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ApplicantID] ASC),
    CONSTRAINT [Applicant_childAbduction] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE,
    CONSTRAINT [Applicant_salutation] FOREIGN KEY ([salutationID]) REFERENCES [dbo].[Salutations] ([salutationID]) ON DELETE CASCADE
);

