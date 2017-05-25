CREATE TABLE [dbo].[AuditEvents] (
    [idAuditEvent]            INT            IDENTITY (1, 1) NOT NULL,
    [EventDate]               DATETIME       NOT NULL,
    [UserID]                  NVARCHAR (40)  NOT NULL,
    [idAuditEventDescription] INT            NOT NULL,
    [RecordChanged]           NVARCHAR (256) NOT NULL,
    [RecordAddedTo]           INT            NULL,
    [DeletedReasonID]         INT            NULL,
    PRIMARY KEY CLUSTERED ([idAuditEvent] ASC),
    CONSTRAINT [AuditEventDescription_AuditEvents] FOREIGN KEY ([idAuditEventDescription]) REFERENCES [dbo].[AuditEventDescriptions] ([idAuditEventDescription]) ON DELETE CASCADE
);

