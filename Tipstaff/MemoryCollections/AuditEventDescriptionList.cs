using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class AuditEventDescription
    {
        public int Id { get; set; }

        public string AuditDescription { get; set; }
    }

    public class AuditEventDescriptionList
    {
        public static List<AuditEventDescription> GetAuditEventDescriptionList()
        {
            return new List<AuditEventDescription>()
            {
                new AuditEventDescription() { Id=1, AuditDescription="AuditDescription"},
                new AuditEventDescription() { Id=2, AuditDescription="FAQs"},
                new AuditEventDescription() { Id=3, AuditDescription="FAQ added"},
                new AuditEventDescription() { Id=4, AuditDescription="FAQ amended"},
                new AuditEventDescription() { Id=5, AuditDescription="FAQ deleted"},
                new AuditEventDescription() { Id=6, AuditDescription="Warrant Records"},
                new AuditEventDescription() { Id=7, AuditDescription="Warrant added"},
                new AuditEventDescription() { Id=8, AuditDescription="Warrant amended"},
                new AuditEventDescription() { Id=9, AuditDescription="Warrant deleted"},
                new AuditEventDescription() { Id=10, AuditDescription="Child Abduction Records"},
                new AuditEventDescription() { Id=11, AuditDescription="ChildAbduction added"},
                new AuditEventDescription() { Id=12, AuditDescription="ChildAbduction amended"},
                new AuditEventDescription() { Id=13, AuditDescription="ChildAbduction deleted"},
                new AuditEventDescription() { Id=14, AuditDescription="Contacts"},
                new AuditEventDescription() { Id=15, AuditDescription="Contact added"},
                new AuditEventDescription() { Id=16, AuditDescription="Contact amended"},
                new AuditEventDescription() { Id=17, AuditDescription="Contact deleted"},
                new AuditEventDescription() { Id=18, AuditDescription="Contact Types"},
                new AuditEventDescription() { Id=19, AuditDescription="ContactType added"},
                new AuditEventDescription() { Id=20, AuditDescription="ContactType amended"},
                new AuditEventDescription() { Id=21, AuditDescription="ContactType deleted"},
                new AuditEventDescription() { Id=22, AuditDescription="Documents"},
                new AuditEventDescription() { Id=23, AuditDescription="Document added"},
                new AuditEventDescription() { Id=24, AuditDescription="Document amended"},
                new AuditEventDescription() { Id=25, AuditDescription="Document deleted"},
                new AuditEventDescription() { Id=26, AuditDescription="Document Statuses"},
                new AuditEventDescription() { Id=27, AuditDescription="DocumentStatus added"},
                new AuditEventDescription() { Id=28, AuditDescription="DocumentStatus amended"},
                new AuditEventDescription() { Id=29, AuditDescription="DocumentStatus deleted"},
                new AuditEventDescription() { Id=30, AuditDescription="Templates"},
                new AuditEventDescription() { Id=31, AuditDescription="Template added"},
                new AuditEventDescription() { Id=32, AuditDescription="Template amended"},
                new AuditEventDescription() { Id=33, AuditDescription="Template deleted"},
                new AuditEventDescription() { Id=34, AuditDescription="Document Types"},
                new AuditEventDescription() { Id=35, AuditDescription="DocumentType added"},
                new AuditEventDescription() { Id=36, AuditDescription="DocumentType amended"},
                new AuditEventDescription() { Id=37, AuditDescription="DocumentType deleted"},
                new AuditEventDescription() { Id=38, AuditDescription="AttendanceNotes"},
                new AuditEventDescription() { Id=39, AuditDescription="AttendanceNote added"},
                new AuditEventDescription() { Id=40, AuditDescription="AttendanceNote amended"},
                new AuditEventDescription() { Id=41, AuditDescription="AttendanceNote deleted"},
                new AuditEventDescription() { Id=42, AuditDescription="Phone Codes"},
                new AuditEventDescription() { Id=43, AuditDescription="AttendanceNoteCode added"},
                new AuditEventDescription() { Id=44, AuditDescription="AttendanceNoteCode amended"},
                new AuditEventDescription() { Id=45, AuditDescription="AttendanceNoteCode deleted"},
                new AuditEventDescription() { Id=46, AuditDescription="Tipstaff Record/Solicitors"},
                new AuditEventDescription() { Id=47, AuditDescription="TipstaffRecordSolicitor added"},
                new AuditEventDescription() { Id=48, AuditDescription="TipstaffRecordSolicitor amended"},
                new AuditEventDescription() { Id=49, AuditDescription="TipstaffRecordSolicitor deleted"},
                new AuditEventDescription() { Id=50, AuditDescription="Solicitors"},
                new AuditEventDescription() { Id=51, AuditDescription="Solicitor added"},
                new AuditEventDescription() { Id=52, AuditDescription="Solicitor amended"},
                new AuditEventDescription() { Id=53, AuditDescription="Solicitor deleted"},
                new AuditEventDescription() { Id=54, AuditDescription="Solicitor Firms"},
                new AuditEventDescription() { Id=55, AuditDescription="SolicitorFirm added"},
                new AuditEventDescription() { Id=56, AuditDescription="SolicitorFirm amended"},
                new AuditEventDescription() { Id=57, AuditDescription="SolicitorFirm deleted"},
                new AuditEventDescription() { Id=58, AuditDescription="Protective Markings"},
                new AuditEventDescription() { Id=59, AuditDescription="ProtectiveMarking added"},
                new AuditEventDescription() { Id=60, AuditDescription="ProtectiveMarking amended"},
                new AuditEventDescription() { Id=61, AuditDescription="ProtectiveMarking deleted"},
                new AuditEventDescription() { Id=62, AuditDescription="Results"},
                new AuditEventDescription() { Id=63, AuditDescription="Result added"},
                new AuditEventDescription() { Id=64, AuditDescription="Result amended"},
                new AuditEventDescription() { Id=65, AuditDescription="Result deleted"},
                new AuditEventDescription() { Id=66, AuditDescription="Case Statuses"},
                new AuditEventDescription() { Id=67, AuditDescription="CaseStatus added"},
                new AuditEventDescription() { Id=68, AuditDescription="CaseStatus amended"},
                new AuditEventDescription() { Id=69, AuditDescription="CaseStatus deleted"},
                new AuditEventDescription() { Id=70, AuditDescription="Case Reviews"},
                new AuditEventDescription() { Id=71, AuditDescription="CaseReview added"},
                new AuditEventDescription() { Id=72, AuditDescription="CaseReview amended"},
                new AuditEventDescription() { Id=73, AuditDescription="CaseReview deleted"},
                new AuditEventDescription() { Id=74, AuditDescription="Case Review Statuses"},
                new AuditEventDescription() { Id=75, AuditDescription="CaseReviewStatus added"},
                new AuditEventDescription() { Id=76, AuditDescription="CaseReviewStatus amended"},
                new AuditEventDescription() { Id=77, AuditDescription="CaseReviewStatus deleted"},
                new AuditEventDescription() { Id=78, AuditDescription="Genders"},
                new AuditEventDescription() { Id=79, AuditDescription="Gender added"},
                new AuditEventDescription() { Id=80, AuditDescription="Gender amended"},
                new AuditEventDescription() { Id=81, AuditDescription="Gender deleted"},
                new AuditEventDescription() { Id=82, AuditDescription="Countries"},
                new AuditEventDescription() { Id=83, AuditDescription="Country added"},
                new AuditEventDescription() { Id=84, AuditDescription="Country amended"},
                new AuditEventDescription() { Id=85, AuditDescription="Country deleted"},
                new AuditEventDescription() { Id=86, AuditDescription="Divisions"},
                new AuditEventDescription() { Id=87, AuditDescription="Division added"},
                new AuditEventDescription() { Id=88, AuditDescription="Division amended"},
                new AuditEventDescription() { Id=89, AuditDescription="Division deleted"},
                new AuditEventDescription() { Id=90, AuditDescription="Children"},
                new AuditEventDescription() { Id=91, AuditDescription="Child added"},
                new AuditEventDescription() { Id=92, AuditDescription="Child amended"},
                new AuditEventDescription() { Id=93, AuditDescription="Child deleted"},
                new AuditEventDescription() { Id=94, AuditDescription="Respondents"},
                new AuditEventDescription() { Id=95, AuditDescription="Respondent added"},
                new AuditEventDescription() { Id=96, AuditDescription="Respondent amended"},
                new AuditEventDescription() { Id=97, AuditDescription="Respondent deleted"},
                new AuditEventDescription() { Id=98, AuditDescription="Child Relationships"},
                new AuditEventDescription() { Id=99, AuditDescription="ChildRelationship added"},
                new AuditEventDescription() { Id=100, AuditDescription="ChildRelationship amended"},
                new AuditEventDescription() { Id=101, AuditDescription="ChildRelationship deleted"},
                new AuditEventDescription() { Id=102, AuditDescription="Salutations"},
                new AuditEventDescription() { Id=103, AuditDescription="Salutation added"},
                new AuditEventDescription() { Id=104, AuditDescription="Salutation amended"},
                new AuditEventDescription() { Id=105, AuditDescription="Salutation deleted"},
                new AuditEventDescription() { Id=106, AuditDescription="Contemnors"},
                new AuditEventDescription() { Id=107, AuditDescription="CAOrderTypes"},
                new AuditEventDescription() { Id=108, AuditDescription="Addressess"},
                new AuditEventDescription() { Id=109, AuditDescription="Contemnor added"},
                new AuditEventDescription() { Id=110, AuditDescription="CAOrderType added"},
                new AuditEventDescription() { Id=111, AuditDescription="Address added"},
                new AuditEventDescription() { Id=112, AuditDescription="Contemnor amended"},
                new AuditEventDescription() { Id=113, AuditDescription="CAOrderType amended"},
                new AuditEventDescription() { Id=114, AuditDescription="Address amended"},
                new AuditEventDescription() { Id=115, AuditDescription="Contemnor deleted"},
                new AuditEventDescription() { Id=116, AuditDescription="CAOrderType deleted"},
                new AuditEventDescription() { Id=117, AuditDescription="Address deleted"},
                new AuditEventDescription() { Id=118, AuditDescription="Nationalities"},
                new AuditEventDescription() { Id=119, AuditDescription="Nationality added"},
                new AuditEventDescription() { Id=120, AuditDescription="Nationality amended"},
                new AuditEventDescription() { Id=121, AuditDescription="Nationality deleted"},
                new AuditEventDescription() { Id=122, AuditDescription="Applicants"},
                new AuditEventDescription() { Id=123, AuditDescription="Applicant added"},
                new AuditEventDescription() { Id=124, AuditDescription="Applicant amended"},
                new AuditEventDescription() { Id=125, AuditDescription="Applicant deleted"},
                new AuditEventDescription() { Id=126, AuditDescription="DeletedTipstaffRecords"},
                new AuditEventDescription() { Id=127, AuditDescription="DeletedTipstaffRecord added"},
                new AuditEventDescription() { Id=128, AuditDescription="DeletedReasons"},
                new AuditEventDescription() { Id=129, AuditDescription="DeletedReason added"},
                new AuditEventDescription() { Id=130, AuditDescription="DeletedReason amended"},
                new AuditEventDescription() { Id=131, AuditDescription="DeletedReason deleted"},
                new AuditEventDescription() { Id=132, AuditDescription="PoliceForces"},
                new AuditEventDescription() { Id=133, AuditDescription="PoliceForce added"},
                new AuditEventDescription() { Id=134, AuditDescription="PoliceForce amended"},
                new AuditEventDescription() { Id=135, AuditDescription="PoliceForce deleted"},
                new AuditEventDescription() { Id=136, AuditDescription="TipstaffPoliceForces"},
                new AuditEventDescription() { Id=137, AuditDescription="TipstaffPoliceForce added"},
                new AuditEventDescription() { Id=138, AuditDescription="TipstaffPoliceForce amended"},
                new AuditEventDescription() { Id=139, AuditDescription="TipstaffPoliceForce deleted"},
                new AuditEventDescription() { Id=140, AuditDescription="Users amended"},
                new AuditEventDescription() { Id=141, AuditDescription="User added"},
                new AuditEventDescription() { Id=142, AuditDescription="User amended"}
            };
        }

        public static AuditEventDescription GetAuditEventDescriptionByDetail(string c)
        {
            return GetAuditEventDescriptionList().FirstOrDefault(x => x.AuditDescription == c);
        }

        public static AuditEventDescription GetAuditEventDescriptionByID(int id)
        {
            return GetAuditEventDescriptionList().FirstOrDefault(x => x.Id == id);
        }
    }
}