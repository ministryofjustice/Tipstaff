CREATE TABLE [dbo].[Solicitors] (
    [solicitorID]     INT           IDENTITY (1, 1) NOT NULL,
    [firstName]       NVARCHAR (50) NULL,
    [lastName]        NVARCHAR (50) NOT NULL,
    [solicitorFirmID] INT           NULL,
    [salutationID]    INT           NOT NULL,
    [phoneDayTime]    NVARCHAR (20) NULL,
    [phoneOutofHours] NVARCHAR (20) NULL,
    [email]           NVARCHAR (60) NULL,
    [active]          BIT           DEFAULT (1) NOT NULL,
    [deactivated]     DATETIME      NULL,
    [deactivatedBy]   NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([solicitorID] ASC),
    CONSTRAINT [Solicitor_salutation] FOREIGN KEY ([salutationID]) REFERENCES [dbo].[Salutations] ([salutationID]) ON DELETE CASCADE,
    CONSTRAINT [SolicitorFirm_Solicitors] FOREIGN KEY ([solicitorFirmID]) REFERENCES [dbo].[SolicitorFirms] ([solicitorFirmID])
);

