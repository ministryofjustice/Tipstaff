CREATE TABLE [dbo].[Contacts] (
    [contactID]     INT             IDENTITY (1, 1) NOT NULL,
    [lastName]      NVARCHAR (50)   NOT NULL,
    [addressLine1]  NVARCHAR (100)  NOT NULL,
    [addressLine2]  NVARCHAR (100)  NULL,
    [addressLine3]  NVARCHAR (100)  NULL,
    [town]          NVARCHAR (100)  NULL,
    [county]        NVARCHAR (100)  NULL,
    [postcode]      NVARCHAR (10)   NOT NULL,
    [DX]            NVARCHAR (50)   NULL,
    [phoneHome]     NVARCHAR (20)   NULL,
    [phoneMobile]   NVARCHAR (20)   NULL,
    [email]         NVARCHAR (60)   NULL,
    [notes]         NVARCHAR (2000) NULL,
    [contactTypeID] INT             NOT NULL,
    [firstName]     NVARCHAR (50)   NULL,
    [salutationID]  INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([contactID] ASC),
    CONSTRAINT [Contact_contactType] FOREIGN KEY ([contactTypeID]) REFERENCES [dbo].[ContactTypes] ([contactTypeID]) ON DELETE CASCADE,
    CONSTRAINT [Contact_salutation] FOREIGN KEY ([salutationID]) REFERENCES [dbo].[Salutations] ([salutationID]) ON DELETE CASCADE
);

