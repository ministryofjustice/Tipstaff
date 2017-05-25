CREATE TABLE [dbo].[Documents] (
    [documentID]        INT            IDENTITY (1, 1) NOT NULL,
    [documentReference] NVARCHAR (60)  NOT NULL,
    [countryID]         INT            NULL,
    [documentStatusID]  INT            NOT NULL,
    [documentTypeID]    INT            NOT NULL,
    [templateID]        INT            NULL,
    [createdOn]         DATETIME       NOT NULL,
    [createdBy]         NVARCHAR (50)  NULL,
    [tipstaffRecordID]  INT            NOT NULL,
    [binaryFile]        IMAGE          NULL,
    [fileName]          NVARCHAR (256) NULL,
    [mimeType]          NVARCHAR (60)  NULL,
    [nationalityID]     INT            NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED ([documentID] ASC),
    CONSTRAINT [Document_country] FOREIGN KEY ([countryID]) REFERENCES [dbo].[Countries] ([countryID]) ON DELETE CASCADE,
    CONSTRAINT [Document_documentStatus] FOREIGN KEY ([documentStatusID]) REFERENCES [dbo].[DocumentStatus] ([DocumentStatusID]) ON DELETE CASCADE,
    CONSTRAINT [Document_documentType] FOREIGN KEY ([documentTypeID]) REFERENCES [dbo].[DocumentTypes] ([documentTypeID]) ON DELETE CASCADE,
    CONSTRAINT [Document_nationality] FOREIGN KEY ([nationalityID]) REFERENCES [dbo].[Nationalities] ([nationalityID]) ON DELETE CASCADE,
    CONSTRAINT [Document_template] FOREIGN KEY ([templateID]) REFERENCES [dbo].[Templates] ([templateID]),
    CONSTRAINT [Document_tipstaffRecord] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID]) ON DELETE CASCADE
);

