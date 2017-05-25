if not exists (select top 1 * from dbo.roles)
begin
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (1, N'carlos.fernandez@justice.gov.uk', N'Carlos Fernandez', N'2016-05-27 15:30:34', 100)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (3, N'ucr71k', N'Richard Cheesley', N'2016-09-07 10:57:10', 75)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (4, N'lch54l', N'Sally Land', N'2016-08-23 00:57:19', 25)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (5, N'MUG48M', N'Bill Brennan', N'2016-01-15 16:34:38', 0)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (6, N'scheesley', N'Sue Cheesley', N'2016-04-20 20:29:14', 25)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (7, N'cez62z', N'Chelsea Young', N'2016-09-07 11:39:49', 25)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (9, N'carlosfernandez', N'Carlos Fernandez', N'2016-09-29 10:43:24', 100)
SET IDENTITY_INSERT [dbo].[Users] OFF
end