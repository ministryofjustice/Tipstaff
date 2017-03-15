-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPNCAttendanceNotes]

AS
BEGIN
select	[Record ID] = CASE 
		WHEN TR.discriminator='ChildAbduction' THEN 'CA' 
		WHEN TR.discriminator='Warrant' THEN D.prefix 
		ELSE '' END + right('000000'+ rtrim(TR.tipstaffrecordid), 6),
		Casename = CASE 
		WHEN TR.discriminator='ChildAbduction' THEN EldestChild 
		WHEN TR.discriminator='Warrant' THEN tr.respondentname 
		ELSE '' END,
		AN.CallDetails
		

from dbo.attendancenotes AN
LEFT OUTER JOIN dbo.TipstaffRecords TR on TR.tipstaffrecordid=an.tipstaffrecordid
LEFT OUTER JOIN dbo.Divisions D ON D.divisionID = TR.divisionID
WHERE	CallDetails like '%PNCID%'
	AND CaseStatusID = 2
END
