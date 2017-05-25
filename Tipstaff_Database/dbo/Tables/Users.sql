CREATE TABLE [dbo].[Users] (
    [UserID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (150) NOT NULL,
    [DisplayName]  NVARCHAR (30)  NOT NULL,
    [LastActive]   DATETIME       NULL,
    [RoleStrength] INT            NOT NULL,
    CONSTRAINT [PK__Users__1788CCAC2739D489] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [User_Role] FOREIGN KEY ([RoleStrength]) REFERENCES [dbo].[Roles] ([strength]) ON DELETE CASCADE
);

