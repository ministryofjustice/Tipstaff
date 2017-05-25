if not exists (select top 1 * from dbo.roles)
begin
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (0, N'Deactivated')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (25, N'User')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (75, N'Admin')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (100, N'System Admin')
end