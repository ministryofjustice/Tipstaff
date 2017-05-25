CREATE TABLE [dbo].[TipstaffPoliceForces] (
    [tipstaffRecordPoliceForceID] INT IDENTITY (1, 1) NOT NULL,
    [tipstaffRecordID]            INT NOT NULL,
    [policeForceID]               INT NOT NULL,
    CONSTRAINT [PK_Tipstaff_PoliceForce] PRIMARY KEY CLUSTERED ([tipstaffRecordPoliceForceID] ASC),
    CONSTRAINT [FK_Tipstaff_PoliceForce_PoliceForces] FOREIGN KEY ([policeForceID]) REFERENCES [dbo].[PoliceForces] ([policeForceID]),
    CONSTRAINT [FK_Tipstaff_PoliceForce_TipstaffRecords] FOREIGN KEY ([tipstaffRecordID]) REFERENCES [dbo].[TipstaffRecords] ([tipstaffRecordID])
);

