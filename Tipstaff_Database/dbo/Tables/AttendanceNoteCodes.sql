CREATE TABLE [dbo].[AttendanceNoteCodes] (
    [AttendanceNoteCodeID] INT           IDENTITY (1, 1) NOT NULL,
    [detail]               NVARCHAR (50) NOT NULL,
    [active]               BIT           NOT NULL,
    [deactivated]          DATETIME      NULL,
    [deactivatedBy]        NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([AttendanceNoteCodeID] ASC)
);

