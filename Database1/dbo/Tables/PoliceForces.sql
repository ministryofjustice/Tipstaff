CREATE TABLE [dbo].[PoliceForces] (
    [policeForceID]    INT            IDENTITY (1, 1) NOT NULL,
    [policeForceName]  NVARCHAR (255) NOT NULL,
    [policeForceEmail] NVARCHAR (255) NOT NULL,
    [active]           BIT            CONSTRAINT [DF_PoliceForces_active] DEFAULT (1) NULL,
    [deactivated]      DATETIME       NULL,
    [deactivatedBy]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_PoliceForces] PRIMARY KEY CLUSTERED ([policeForceID] ASC)
);

