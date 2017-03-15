CREATE TABLE [dbo].[Templates] (
    [templateID]        INT           IDENTITY (1, 1) NOT NULL,
    [Discriminator]     VARCHAR (128) NOT NULL,
    [templateName]      NVARCHAR (80) NOT NULL,
    [addresseerequired] BIT           NOT NULL,
    [active]            BIT           DEFAULT (1) NOT NULL,
    [deactivated]       DATETIME      NULL,
    [deactivatedBy]     NVARCHAR (50) NULL,
    [templateXML]       TEXT          NULL,
    CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED ([templateID] ASC)
);

