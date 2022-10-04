using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Tipstaff.Models
{
    public class TipstaffDBInitializer : CreateDatabaseIfNotExists<TipstaffDB> // DropCreateDatabaseAlways<TipstaffDB> // DropCreateDatabaseIfModelChanges<TipstaffDB> //  
    {
        protected override void Seed(TipstaffDB context)
        {
            var caot = new List<CAOrderType>
            {
                new CAOrderType {Detail="Collection", active=true},
                new CAOrderType {Detail="Location", active=true},
                new CAOrderType {Detail="Passport Seizure", active=true}
            };

            var sal = new List<Salutation>
            {
                new Salutation {Detail="Mr", active=true},
                new Salutation {Detail="Miss", active=true},
                new Salutation {Detail="Ms", active=true},
                new Salutation {Detail="Mrs", active=true},
                new Salutation {Detail="Madam", active=true},
                new Salutation {Detail="Sir", active=true}
            };

            var contactTypes = new List<ContactType>
            {
                new ContactType {Detail="Judicial", active=true},
                new ContactType {Detail="Judges Clerk", active=true},
                new ContactType {Detail="Solicitor", active=true},
                new ContactType {Detail="Met Police", active=true},
                new ContactType {Detail="Met Police IBO", active=true}
            };
            var protMarks = new List<ProtectiveMarking>
            {
                new ProtectiveMarking {Detail="Confidential", active=true},
                new ProtectiveMarking {Detail="Restricted",active=true},
                new ProtectiveMarking {Detail="Protect",active=true},
                new ProtectiveMarking {Detail="Unclassified",active=true}
            };
            var results = new List<Result>
            {
                new Result{Detail="Executed",active=true},
                new Result{Detail="Suspended",active=true},
                new Result{Detail="Discharged",active=true},
                new Result{Detail="Expired",active=true},
                new Result{Detail="Lodged in Prison",active=true},
                new Result{Detail="Arrested",active=true}
            };
            var caCS = new List<CaseStatus>
            {
                new CaseStatus{Detail="Awaiting Information",active=true},
                new CaseStatus{Detail="Active",active=true},
                //new ChildAbductionCaseStatus{Detail="Port Alert Only",active=true},
                new CaseStatus{Detail="File Closed",active=true},
                new CaseStatus{Detail="File Archived",active=true},
            }; 
            //var caCS = new List<ChildAbductionCaseStatus>
            //{
            //    new ChildAbductionCaseStatus{Detail="Awaiting Information",active=true},
            //    new ChildAbductionCaseStatus{Detail="Active",active=true},
            //    //new ChildAbductionCaseStatus{Detail="Port Alert Only",active=true},
            //    new ChildAbductionCaseStatus{Detail="File Closed",active=true},
            //    new ChildAbductionCaseStatus{Detail="File Archived",active=true},
            //};
            //var wCS = new List<WarrantCaseStatus>
            //{
            //    new WarrantCaseStatus{Detail="Awaiting Information",active=true},
            //    new WarrantCaseStatus{Detail="Active",active=true},
            //    //new WarrantCaseStatus{Detail="Discharged/Suspended",active=true},
            //    //new WarrantCaseStatus{Detail="Lodged in Prison/File Closed",active=true},
            //    //new WarrantCaseStatus{Detail="Arrested/File Closed",active=true},
            //    new WarrantCaseStatus{Detail="File Closed",active=true},
            //    new WarrantCaseStatus{Detail="File Archived",active=true},
            //};
            var crS = new List<CaseReviewStatus>
            {
                new CaseReviewStatus{Detail="To be reviewed",active=true},
                new CaseReviewStatus{Detail="File Closed",active=true},
                new CaseReviewStatus{Detail="File Archived",active=true},
            };
            var dT = new List<DocumentType>
            {
                new DocumentType{Detail="Generated",active=true},
                new DocumentType{Detail="Order",active=true},
                new DocumentType{Detail="Passport",active=true},
                new DocumentType{Detail="Driving Licence",active=true},
                new DocumentType{Detail="ID Card",active=true},
                new DocumentType{Detail="SCD26 papers",active=true},
                new DocumentType{Detail="Updated document",active=true},
                new DocumentType{Detail="Other",active=true},
            };
            var cR = new List<ChildRelationship>
            {
                new ChildRelationship{Detail="None-Warrant",active=false},
                new ChildRelationship{Detail="Father",active=true},
                new ChildRelationship{Detail="Mother",active=true},
                new ChildRelationship{Detail="Grandfather",active=true},
                new ChildRelationship{Detail="Grandmother",active=true},
                new ChildRelationship{Detail="Uncle",active=true},
                new ChildRelationship{Detail="Aunt",active=true},
                new ChildRelationship{Detail="Brother",active=true},
                new ChildRelationship{Detail="Sister",active=true}

            };
            var dS = new List<DocumentStatus>
            {
                new DocumentStatus{Detail="Generated",active=true},
                new DocumentStatus{Detail="Stored in Tipstaff's Safe",active=true},
                new DocumentStatus{Detail="Disposed of",active=true},
                new DocumentStatus{Detail="Returned to Owner",active=true},
                new DocumentStatus{Detail="Stored on shared drive",active=true},
            };
            var g = new List<Gender>
            {
                new Gender{detail="Male", active=true},
                new Gender{detail="Female", active=true}
            };
            var pCC = new List<AttendanceNoteCode>
            {
                new AttendanceNoteCode{detail="Phone call", active=true},
                new AttendanceNoteCode{detail="Personal Attendance", active=true},
                new AttendanceNoteCode{detail="Note", active=true}
            };
            var iC = new List<Country>
            {
                #region Country List (260 rows)
                new Country{Detail="Afghanistan",active=true},
                new Country{Detail="Akrotiri",active=true},
                new Country{Detail="Albania",active=true},
                new Country{Detail="Algeria",active=true},
                new Country{Detail="American Samoa",active=true},
                new Country{Detail="Andorra",active=true},
                new Country{Detail="Angola",active=true},
                new Country{Detail="Anguilla",active=true},
                new Country{Detail="Antarctica",active=true},
                new Country{Detail="Antigua and Barbuda",active=true},
                new Country{Detail="Arctic Ocean",active=true},
                new Country{Detail="Argentina",active=true},
                new Country{Detail="Armenia",active=true},
                new Country{Detail="Aruba",active=true},
                new Country{Detail="Ashmore and Cartier Islands",active=true},
                new Country{Detail="Atlantic Ocean",active=true},
                new Country{Detail="Australia",active=true},
                new Country{Detail="Austria",active=true},
                new Country{Detail="Azerbaijan",active=true},
                new Country{Detail="Bahamas, The",active=true},
                new Country{Detail="Bahrain",active=true},
                new Country{Detail="Bangladesh",active=true},
                new Country{Detail="Barbados",active=true},
                new Country{Detail="Belarus",active=true},
                new Country{Detail="Belgium",active=true},
                new Country{Detail="Belize",active=true},
                new Country{Detail="Benin",active=true},
                new Country{Detail="Bermuda",active=true},
                new Country{Detail="Bhutan",active=true},
                new Country{Detail="Bolivia",active=true},
                new Country{Detail="Bosnia and Herzegovina",active=true},
                new Country{Detail="Botswana",active=true},
                new Country{Detail="Bouvet Island",active=true},
                new Country{Detail="Brazil",active=true},
                new Country{Detail="British Indian Ocean Territory",active=true},
                new Country{Detail="British Virgin Islands",active=true},
                new Country{Detail="Brunei",active=true},
                new Country{Detail="Bulgaria",active=true},
                new Country{Detail="Burkina Faso",active=true},
                new Country{Detail="Burma",active=true},
                new Country{Detail="Burundi",active=true},
                new Country{Detail="Cambodia",active=true},
                new Country{Detail="Cameroon",active=true},
                new Country{Detail="Canada",active=true},
                new Country{Detail="Cape Verde",active=true},
                new Country{Detail="Cayman Islands",active=true},
                new Country{Detail="Central African Republic",active=true},
                new Country{Detail="Chad",active=true},
                new Country{Detail="Chile",active=true},
                new Country{Detail="China",active=true},
                new Country{Detail="Christmas Island",active=true},
                new Country{Detail="Clipperton Island",active=true},
                new Country{Detail="Cocos (Keeling) Islands",active=true},
                new Country{Detail="Colombia",active=true},
                new Country{Detail="Comoros",active=true},
                new Country{Detail="Congo, Democratic Republic of the",active=true},
                new Country{Detail="Congo, Republic of the",active=true},
                new Country{Detail="Cook Islands",active=true},
                new Country{Detail="Coral Sea Islands",active=true},
                new Country{Detail="Costa Rica",active=true},
                new Country{Detail="Cote d'Ivoire",active=true},
                new Country{Detail="Croatia",active=true},
                new Country{Detail="Cuba",active=true},
                new Country{Detail="Curacao",active=true},
                new Country{Detail="Cyprus",active=true},
                new Country{Detail="Czech Republic",active=true},
                new Country{Detail="Denmark",active=true},
                new Country{Detail="Dhekelia",active=true},
                new Country{Detail="Djibouti",active=true},
                new Country{Detail="Dominica",active=true},
                new Country{Detail="Dominican Republic",active=true},
                new Country{Detail="Ecuador",active=true},
                new Country{Detail="Egypt",active=true},
                new Country{Detail="El Salvador",active=true},
                new Country{Detail="Equatorial Guinea",active=true},
                new Country{Detail="Eritrea",active=true},
                new Country{Detail="Estonia",active=true},
                new Country{Detail="Ethiopia",active=true},
                new Country{Detail="Falkland Islands (Islas Malvinas)",active=true},
                new Country{Detail="Faroe Islands",active=true},
                new Country{Detail="Fiji",active=true},
                new Country{Detail="Finland",active=true},
                new Country{Detail="France",active=true},
                new Country{Detail="French Polynesia",active=true},
                new Country{Detail="French Southern and Antarctic Lands",active=true},
                new Country{Detail="Gabon",active=true},
                new Country{Detail="Gambia, The",active=true},
                new Country{Detail="Gaza Strip",active=true},
                new Country{Detail="Georgia",active=true},
                new Country{Detail="Germany",active=true},
                new Country{Detail="Ghana",active=true},
                new Country{Detail="Gibraltar",active=true},
                new Country{Detail="Greece",active=true},
                new Country{Detail="Greenland",active=true},
                new Country{Detail="Grenada",active=true},
                new Country{Detail="Guam",active=true},
                new Country{Detail="Guatemala",active=true},
                new Country{Detail="Guernsey",active=true},
                new Country{Detail="Guinea",active=true},
                new Country{Detail="Guinea-Bissau",active=true},
                new Country{Detail="Guyana",active=true},
                new Country{Detail="Haiti",active=true},
                new Country{Detail="Heard Island and McDonald Islands",active=true},
                new Country{Detail="Holy See (Vatican City)",active=true},
                new Country{Detail="Honduras",active=true},
                new Country{Detail="Hong Kong",active=true},
                new Country{Detail="Hungary",active=true},
                new Country{Detail="Iceland",active=true},
                new Country{Detail="India",active=true},
                new Country{Detail="Indian Ocean",active=true},
                new Country{Detail="Indonesia",active=true},
                new Country{Detail="Iran",active=true},
                new Country{Detail="Iraq",active=true},
                new Country{Detail="Ireland",active=true},
                new Country{Detail="Isle of Man",active=true},
                new Country{Detail="Israel",active=true},
                new Country{Detail="Italy",active=true},
                new Country{Detail="Jamaica",active=true},
                new Country{Detail="Jan Mayen",active=true},
                new Country{Detail="Japan",active=true},
                new Country{Detail="Jersey",active=true},
                new Country{Detail="Jordan",active=true},
                new Country{Detail="Kazakhstan",active=true},
                new Country{Detail="Kenya",active=true},
                new Country{Detail="Kiribati",active=true},
                new Country{Detail="Korea, North",active=true},
                new Country{Detail="Korea, South",active=true},
                new Country{Detail="Kosovo",active=true},
                new Country{Detail="Kuwait",active=true},
                new Country{Detail="Kyrgyzstan",active=true},
                new Country{Detail="Laos",active=true},
                new Country{Detail="Latvia",active=true},
                new Country{Detail="Lebanon",active=true},
                new Country{Detail="Lesotho",active=true},
                new Country{Detail="Liberia",active=true},
                new Country{Detail="Libya",active=true},
                new Country{Detail="Liechtenstein",active=true},
                new Country{Detail="Lithuania",active=true},
                new Country{Detail="Luxembourg",active=true},
                new Country{Detail="Macau",active=true},
                new Country{Detail="Macedonia",active=true},
                new Country{Detail="Madagascar",active=true},
                new Country{Detail="Malawi",active=true},
                new Country{Detail="Malaysia",active=true},
                new Country{Detail="Maldives",active=true},
                new Country{Detail="Mali",active=true},
                new Country{Detail="Malta",active=true},
                new Country{Detail="Marshall Islands",active=true},
                new Country{Detail="Mauritania",active=true},
                new Country{Detail="Mauritius",active=true},
                new Country{Detail="Mexico",active=true},
                new Country{Detail="Micronesia, Federated States of",active=true},
                new Country{Detail="Moldova",active=true},
                new Country{Detail="Monaco",active=true},
                new Country{Detail="Mongolia",active=true},
                new Country{Detail="Montenegro",active=true},
                new Country{Detail="Montserrat",active=true},
                new Country{Detail="Morocco",active=true},
                new Country{Detail="Mozambique",active=true},
                new Country{Detail="Namibia",active=true},
                new Country{Detail="Nauru",active=true},
                new Country{Detail="Navassa Island",active=true},
                new Country{Detail="Nepal",active=true},
                new Country{Detail="Netherlands",active=true},
                new Country{Detail="New Caledonia",active=true},
                new Country{Detail="New Zealand",active=true},
                new Country{Detail="Nicaragua",active=true},
                new Country{Detail="Niger",active=true},
                new Country{Detail="Nigeria",active=true},
                new Country{Detail="Niue",active=true},
                new Country{Detail="Norfolk Island",active=true},
                new Country{Detail="Northern Mariana Islands",active=true},
                new Country{Detail="Norway",active=true},
                new Country{Detail="Oman",active=true},
                new Country{Detail="Pacific Ocean",active=true},
                new Country{Detail="Pakistan",active=true},
                new Country{Detail="Palau",active=true},
                new Country{Detail="Panama",active=true},
                new Country{Detail="Papua New Guinea",active=true},
                new Country{Detail="Paracel Islands",active=true},
                new Country{Detail="Paraguay",active=true},
                new Country{Detail="Peru",active=true},
                new Country{Detail="Philippines",active=true},
                new Country{Detail="Pitcairn Islands",active=true},
                new Country{Detail="Poland",active=true},
                new Country{Detail="Portugal",active=true},
                new Country{Detail="Puerto Rico",active=true},
                new Country{Detail="Qatar",active=true},
                new Country{Detail="Romania",active=true},
                new Country{Detail="Russia",active=true},
                new Country{Detail="Rwanda",active=true},
                new Country{Detail="Saint Barthelemy",active=true},
                new Country{Detail="Saint Helena, Ascension, and Tristan da Cunha",active=true},
                new Country{Detail="Saint Kitts and Nevis",active=true},
                new Country{Detail="Saint Lucia",active=true},
                new Country{Detail="Saint Martin",active=true},
                new Country{Detail="Saint Pierre and Miquelon",active=true},
                new Country{Detail="Saint Vincent and the Grenadines",active=true},
                new Country{Detail="Samoa",active=true},
                new Country{Detail="San Marino",active=true},
                new Country{Detail="Sao Tome and Principe",active=true},
                new Country{Detail="Saudi Arabia",active=true},
                new Country{Detail="Senegal",active=true},
                new Country{Detail="Serbia",active=true},
                new Country{Detail="Seychelles",active=true},
                new Country{Detail="Sierra Leone",active=true},
                new Country{Detail="Singapore",active=true},
                new Country{Detail="Sint Maarten",active=true},
                new Country{Detail="Slovakia",active=true},
                new Country{Detail="Slovenia",active=true},
                new Country{Detail="Solomon Islands",active=true},
                new Country{Detail="Somalia",active=true},
                new Country{Detail="South Africa",active=true},
                new Country{Detail="South Georgia and South Sandwich Islands",active=true},
                new Country{Detail="Southern Ocean",active=true},
                new Country{Detail="South Sudan",active=true},
                new Country{Detail="Spain",active=true},
                new Country{Detail="Spratly Islands",active=true},
                new Country{Detail="Sri Lanka",active=true},
                new Country{Detail="Sudan",active=true},
                new Country{Detail="Suriname",active=true},
                new Country{Detail="Svalbard",active=true},
                new Country{Detail="Swaziland",active=true},
                new Country{Detail="Sweden",active=true},
                new Country{Detail="Switzerland",active=true},
                new Country{Detail="Syria",active=true},
                new Country{Detail="Taiwan",active=true},
                new Country{Detail="Tajikistan",active=true},
                new Country{Detail="Tanzania",active=true},
                new Country{Detail="Thailand",active=true},
                new Country{Detail="Timor-Leste",active=true},
                new Country{Detail="Togo",active=true},
                new Country{Detail="Tokelau",active=true},
                new Country{Detail="Tonga",active=true},
                new Country{Detail="Trinidad and Tobago",active=true},
                new Country{Detail="Tunisia",active=true},
                new Country{Detail="Turkey",active=true},
                new Country{Detail="Turkmenistan",active=true},
                new Country{Detail="Turks and Caicos Islands",active=true},
                new Country{Detail="Tuvalu",active=true},
                new Country{Detail="Uganda",active=true},
                new Country{Detail="Ukraine",active=true},
                new Country{Detail="United Arab Emirates",active=true},
                new Country{Detail="United Kingdom",active=true},
                new Country{Detail="United States",active=true},
                new Country{Detail="United States Pacific Island Wildlife Refuges",active=true},
                new Country{Detail="Uruguay",active=true},
                new Country{Detail="Uzbekistan",active=true},
                new Country{Detail="Vanuatu",active=true},
                new Country{Detail="Venezuela",active=true},
                new Country{Detail="Vietnam",active=true},
                new Country{Detail="Virgin Islands",active=true},
                new Country{Detail="Wake Island",active=true},
                new Country{Detail="Wallis and Futuna",active=true},
                new Country{Detail="West Bank",active=true},
                new Country{Detail="Western Sahara",active=true},
                new Country{Detail="Yemen",active=true},
                new Country{Detail="Zambia",active=true},
                new Country{Detail="Zimbabwe",active=true},
                new Country{Detail="European Union",active=true}
                #endregion
            };
            //var sF = new List<SolicitorFirm>
            //{
            //    new SolicitorFirm{firmName="Cheetem and Run", addressLine1="test"}
            //};
            //var s = new List<Solicitor>
            //{
            //    new Solicitor{firstName="Sue M.", lastName="Blind", solicitorFirmID=1, salutationID=2},
            //    new Solicitor{firstName="Bill", lastName="Biggs", solicitorFirmID=1, salutationID=1}
            //};
            var d = new List<Division>
            {
                new Division{Detail="Bankruptcy", Prefix="B", active=true},
                new Division{Detail="Chancery", Prefix="CH", active=true},
                new Division{Detail="Family", Prefix="FAM", active=true},
                new Division{Detail="Insolvency", Prefix="IN", active=true},
                new Division{Detail="Queen's Bench", Prefix="QB", active=false},
                new Division{Detail="King's Bench", Prefix="KB", active=true}
            };

            //var w = new List<Warrant>
            //{
            //    new Warrant{createdOn=DateTime.Now.AddMonths(-2)
            //                    ,caseNumber="123456",protectiveMarkingID=2
            //                    ,nextReviewDate=DateTime.Today.AddMonths(-1), expiryDate=DateTime.Today.AddYears(1)
            //                    ,caseStatusID=2, createdBy="cbruce",divisionID=2},
            //    new Warrant{createdOn=DateTime.Now.AddDays(-42)
            //                    ,caseNumber="549873",protectiveMarkingID=3
            //                    ,nextReviewDate=DateTime.Today.AddDays(-42).AddMonths(1), expiryDate=DateTime.Today.AddYears(1)
            //                    ,caseStatusID=2, createdBy="cbruce",divisionID=1},
            //    new Warrant{createdOn=DateTime.Today.AddDays(-28)
            //                    ,caseNumber="82135",protectiveMarkingID=2
            //                    ,nextReviewDate=DateTime.Today.AddDays(2), expiryDate=DateTime.Today.AddYears(1)
            //                    ,caseStatusID=1, createdBy="cbruce",divisionID=3},
            //    new Warrant{createdOn=DateTime.Now.AddDays(-10)
            //                    ,caseNumber="675197",protectiveMarkingID=2
            //                    ,nextReviewDate=DateTime.Today.AddDays(20), expiryDate=DateTime.Today.AddYears(1)
            //                    ,caseStatusID=1, createdBy="cbruce",divisionID=3}
            //};
            //var ca = new List<ChildAbduction>
            //{
            //    new ChildAbduction{createdBy="cbruce", createdOn=DateTime.Now.AddDays(-35), officerDealing="Richard Cheesley"
            //                        , orderDated=DateTime.Today.AddDays(-35), orderReceived=DateTime.Today.AddDays(-35), caseStatusID=1
            //                        ,nextReviewDate=DateTime.Today.AddDays(2), protectiveMarkingID=2, sentSCD26=DateTime.Today.AddDays(-35), caOrderTypeID=1}
            //};
            var faq = new List<FAQ>
            {
                new FAQ{loggedInUser=false, question="What is the Tipstaff Database?", answer="It is a system used by the Tipstaffs Office of the RCJ"},
                new FAQ{loggedInUser=true, question="If I have a problem, how do I raise a call?", answer="Call the Atos Service desk on 0800 7830162"},
                new FAQ{loggedInUser=false, question="Who supports the system?", answer="The system was developed, and is supported by, Solutions Development, a part of MoJ ICT.  All help call sshould be raised via the Atos Servicedesk on 0800 7830162"}
            };
            var a = new List<AuditEventDescription>
            {
                #region Audit Event Descriptions
                new AuditEventDescription{idAuditEventDescription=10, AuditDescription = "FAQs"},
                new AuditEventDescription{idAuditEventDescription=11, AuditDescription = "FAQ added"},
                new AuditEventDescription{idAuditEventDescription=12, AuditDescription = "FAQ amended"},
                new AuditEventDescription{idAuditEventDescription=13 ,AuditDescription = "FAQ deleted"},
                new AuditEventDescription{idAuditEventDescription=20, AuditDescription = "Warrant Records"},
                new AuditEventDescription{idAuditEventDescription=21, AuditDescription = "Warrant added"},
                new AuditEventDescription{idAuditEventDescription=22, AuditDescription = "Warrant amended"},
                new AuditEventDescription{idAuditEventDescription=23 ,AuditDescription = "Warrant deleted"},
                new AuditEventDescription{idAuditEventDescription=24, AuditDescription = "Child Abduction Records"},
                new AuditEventDescription{idAuditEventDescription=25, AuditDescription = "ChildAbduction added"},
                new AuditEventDescription{idAuditEventDescription=26, AuditDescription = "ChildAbduction amended"},
                new AuditEventDescription{idAuditEventDescription=27, AuditDescription = "ChildAbduction deleted"},
                new AuditEventDescription{idAuditEventDescription=30, AuditDescription = "Contacts"},
                new AuditEventDescription{idAuditEventDescription=31, AuditDescription = "Contact added"},
                new AuditEventDescription{idAuditEventDescription=32, AuditDescription = "Contact amended"},
                new AuditEventDescription{idAuditEventDescription=33 ,AuditDescription = "Contact deleted"},
                new AuditEventDescription{idAuditEventDescription=40, AuditDescription = "Contact Types"},
                new AuditEventDescription{idAuditEventDescription=41, AuditDescription = "ContactType added"},
                new AuditEventDescription{idAuditEventDescription=42, AuditDescription = "ContactType amended"},
                new AuditEventDescription{idAuditEventDescription=43 ,AuditDescription = "ContactType deleted"},
                new AuditEventDescription{idAuditEventDescription=50, AuditDescription = "Documents"},
                new AuditEventDescription{idAuditEventDescription=51, AuditDescription = "Document added"},
                new AuditEventDescription{idAuditEventDescription=52, AuditDescription = "Document amended"},
                new AuditEventDescription{idAuditEventDescription=53 ,AuditDescription = "Document deleted"},
                new AuditEventDescription{idAuditEventDescription=60, AuditDescription = "Document Statuses"},
                new AuditEventDescription{idAuditEventDescription=61, AuditDescription = "DocumentStatus added"},
                new AuditEventDescription{idAuditEventDescription=62, AuditDescription = "DocumentStatus amended"},
                new AuditEventDescription{idAuditEventDescription=63 ,AuditDescription = "DocumentStatus deleted"},
                new AuditEventDescription{idAuditEventDescription=70, AuditDescription = "Templates"},
                new AuditEventDescription{idAuditEventDescription=71, AuditDescription = "Template added"},
                new AuditEventDescription{idAuditEventDescription=72, AuditDescription = "Template amended"},
                new AuditEventDescription{idAuditEventDescription=73 ,AuditDescription = "Template deleted"},
                new AuditEventDescription{idAuditEventDescription=80, AuditDescription = "Document Types"},
                new AuditEventDescription{idAuditEventDescription=81, AuditDescription = "DocumentType added"},
                new AuditEventDescription{idAuditEventDescription=82, AuditDescription = "DocumentType amended"},
                new AuditEventDescription{idAuditEventDescription=83 ,AuditDescription = "DocumentType deleted"},
                new AuditEventDescription{idAuditEventDescription=90, AuditDescription = "AttendanceNotes"},
                new AuditEventDescription{idAuditEventDescription=91, AuditDescription = "AttendanceNote added"},
                new AuditEventDescription{idAuditEventDescription=92, AuditDescription = "AttendanceNote amended"},
                new AuditEventDescription{idAuditEventDescription=93 ,AuditDescription = "AttendanceNote deleted"},
                new AuditEventDescription{idAuditEventDescription=100, AuditDescription = "Phone Codes"},
                new AuditEventDescription{idAuditEventDescription=101, AuditDescription = "AttendanceNoteCode added"},
                new AuditEventDescription{idAuditEventDescription=102, AuditDescription = "AttendanceNoteCode amended"},
                new AuditEventDescription{idAuditEventDescription=103 ,AuditDescription = "AttendanceNoteCode deleted"},
                new AuditEventDescription{idAuditEventDescription=110, AuditDescription = "Tipstaff Record/Solicitors"},
                new AuditEventDescription{idAuditEventDescription=111, AuditDescription = "TipstaffRecordSolicitor added"},
                new AuditEventDescription{idAuditEventDescription=112, AuditDescription = "TipstaffRecordSolicitor amended"},
                new AuditEventDescription{idAuditEventDescription=113 ,AuditDescription = "TipstaffRecordSolicitor deleted"},
                new AuditEventDescription{idAuditEventDescription=120, AuditDescription = "Solicitors"},
                new AuditEventDescription{idAuditEventDescription=121, AuditDescription = "Solicitor added"},
                new AuditEventDescription{idAuditEventDescription=122, AuditDescription = "Solicitor amended"},
                new AuditEventDescription{idAuditEventDescription=123 ,AuditDescription = "Solicitor deleted"},
                new AuditEventDescription{idAuditEventDescription=130, AuditDescription = "Solicitor Firms"},
                new AuditEventDescription{idAuditEventDescription=131, AuditDescription = "SolicitorFirm added"},
                new AuditEventDescription{idAuditEventDescription=132, AuditDescription = "SolicitorFirm amended"},
                new AuditEventDescription{idAuditEventDescription=133 ,AuditDescription = "SolicitorFirm deleted"},
                new AuditEventDescription{idAuditEventDescription=140, AuditDescription = "Protective Markings"},
                new AuditEventDescription{idAuditEventDescription=141, AuditDescription = "ProtectiveMarking added"},
                new AuditEventDescription{idAuditEventDescription=142, AuditDescription = "ProtectiveMarking amended"},
                new AuditEventDescription{idAuditEventDescription=143 ,AuditDescription = "ProtectiveMarking deleted"},
                new AuditEventDescription{idAuditEventDescription=150, AuditDescription = "Results"},
                new AuditEventDescription{idAuditEventDescription=151, AuditDescription = "Result added"},
                new AuditEventDescription{idAuditEventDescription=152, AuditDescription = "Result amended"},
                new AuditEventDescription{idAuditEventDescription=153 ,AuditDescription = "Result deleted"},
                new AuditEventDescription{idAuditEventDescription=160, AuditDescription = "Case Statuses"},
                new AuditEventDescription{idAuditEventDescription=161, AuditDescription = "CaseStatus added"},
                new AuditEventDescription{idAuditEventDescription=162, AuditDescription = "CaseStatus amended"},
                new AuditEventDescription{idAuditEventDescription=163 ,AuditDescription = "CaseStatus deleted"},
                //new AuditEventDescription{idAuditEventDescription=160, AuditDescription = "Child Abduction Case Statuses"},
                //new AuditEventDescription{idAuditEventDescription=161, AuditDescription = "ChildAbductionCaseStatus added"},
                //new AuditEventDescription{idAuditEventDescription=162, AuditDescription = "ChildAbductionCaseStatus amended"},
                //new AuditEventDescription{idAuditEventDescription=163 ,AuditDescription = "ChildAbductionCaseStatus deleted"},
                //new AuditEventDescription{idAuditEventDescription=170, AuditDescription = "Warrant Case Statuses"},
                //new AuditEventDescription{idAuditEventDescription=171, AuditDescription = "WarrantCaseStatus added"},
                //new AuditEventDescription{idAuditEventDescription=172, AuditDescription = "WarrantCaseStatus amended"},
                //new AuditEventDescription{idAuditEventDescription=173 ,AuditDescription = "WarrantCaseStatus deleted"},
                new AuditEventDescription{idAuditEventDescription=180, AuditDescription = "Case Reviews"},
                new AuditEventDescription{idAuditEventDescription=181, AuditDescription = "CaseReview added"},
                new AuditEventDescription{idAuditEventDescription=182, AuditDescription = "CaseReview amended"},
                new AuditEventDescription{idAuditEventDescription=183 ,AuditDescription = "CaseReview deleted"},
                new AuditEventDescription{idAuditEventDescription=190, AuditDescription = "Case Review Statuses"},
                new AuditEventDescription{idAuditEventDescription=191, AuditDescription = "CaseReviewStatus added"},
                new AuditEventDescription{idAuditEventDescription=192, AuditDescription = "CaseReviewStatus amended"},
                new AuditEventDescription{idAuditEventDescription=193 ,AuditDescription = "CaseReviewStatus deleted"},
                new AuditEventDescription{idAuditEventDescription=200, AuditDescription = "Genders"},
                new AuditEventDescription{idAuditEventDescription=201, AuditDescription = "Gender added"},
                new AuditEventDescription{idAuditEventDescription=202, AuditDescription = "Gender amended"},
                new AuditEventDescription{idAuditEventDescription=203 ,AuditDescription = "Gender deleted"},
                new AuditEventDescription{idAuditEventDescription=210, AuditDescription = "Countries"},
                new AuditEventDescription{idAuditEventDescription=211, AuditDescription = "Country added"},
                new AuditEventDescription{idAuditEventDescription=212, AuditDescription = "Country amended"},
                new AuditEventDescription{idAuditEventDescription=213 ,AuditDescription = "Country deleted"},
                new AuditEventDescription{idAuditEventDescription=220, AuditDescription = "Divisions"},
                new AuditEventDescription{idAuditEventDescription=221, AuditDescription = "Division added"},
                new AuditEventDescription{idAuditEventDescription=222, AuditDescription = "Division amended"},
                new AuditEventDescription{idAuditEventDescription=223 ,AuditDescription = "Division deleted"},
                new AuditEventDescription{idAuditEventDescription=230, AuditDescription = "Children"},
                new AuditEventDescription{idAuditEventDescription=231, AuditDescription = "Child added"},
                new AuditEventDescription{idAuditEventDescription=232, AuditDescription = "Child amended"},
                new AuditEventDescription{idAuditEventDescription=233 ,AuditDescription = "Child deleted"},
                new AuditEventDescription{idAuditEventDescription=240, AuditDescription = "Respondents"},
                new AuditEventDescription{idAuditEventDescription=241, AuditDescription = "Respondent added"},
                new AuditEventDescription{idAuditEventDescription=242, AuditDescription = "Respondent amended"},
                new AuditEventDescription{idAuditEventDescription=243 ,AuditDescription = "Respondent deleted"},
                new AuditEventDescription{idAuditEventDescription=250, AuditDescription = "Child Relationships"},
                new AuditEventDescription{idAuditEventDescription=251, AuditDescription = "ChildRelationship added"},
                new AuditEventDescription{idAuditEventDescription=252, AuditDescription = "ChildRelationship amended"},
                new AuditEventDescription{idAuditEventDescription=253 ,AuditDescription = "ChildRelationship deleted"},
                new AuditEventDescription{idAuditEventDescription=260, AuditDescription = "Salutations"},
                new AuditEventDescription{idAuditEventDescription=261, AuditDescription = "Salutation added"},
                new AuditEventDescription{idAuditEventDescription=262, AuditDescription = "Salutation amended"},
                new AuditEventDescription{idAuditEventDescription=263 ,AuditDescription = "Salutation deleted"},
                new AuditEventDescription{idAuditEventDescription=270, AuditDescription = "Contemnors"},
                new AuditEventDescription{idAuditEventDescription=271, AuditDescription = "Contemnor added"},
                new AuditEventDescription{idAuditEventDescription=272, AuditDescription = "Contemnor amended"},
                new AuditEventDescription{idAuditEventDescription=273 ,AuditDescription = "Contemnor deleted"},
                new AuditEventDescription{idAuditEventDescription=270, AuditDescription = "CAOrderTypes"},
                new AuditEventDescription{idAuditEventDescription=271, AuditDescription = "CAOrderType added"},
                new AuditEventDescription{idAuditEventDescription=272, AuditDescription = "CAOrderType amended"},
                new AuditEventDescription{idAuditEventDescription=273 ,AuditDescription = "CAOrderType deleted"},
                new AuditEventDescription{idAuditEventDescription=270, AuditDescription = "Addressess"},
                new AuditEventDescription{idAuditEventDescription=271, AuditDescription = "Address added"},
                new AuditEventDescription{idAuditEventDescription=272, AuditDescription = "Address amended"},
                new AuditEventDescription{idAuditEventDescription=273 ,AuditDescription = "Address deleted"}
                //new AuditEventDescription{idAuditEventDescription=0, AuditDescription = "XXXs"},
                //new AuditEventDescription{idAuditEventDescription=1, AuditDescription = "XXX added"},
                //new AuditEventDescription{idAuditEventDescription=2, AuditDescription = "XXX amended"},
                //new AuditEventDescription{idAuditEventDescription=3 ,AuditDescription = "XXX deleted"},
#endregion
            };
            a.ForEach(c => context.AuditDescriptions.Add(c));
            context.SaveChanges();
            caot.ForEach(c => context.CAOrderTypes.Add(c));
            sal.ForEach(c => context.Salutations.Add(c));
            context.SaveChanges();
            contactTypes.ForEach(cType => context.ContactTypes.Add(cType));
            protMarks.ForEach(p => context.ProtectiveMarkings.Add(p));
            results.ForEach(r => context.Results.Add(r));
            caCS.ForEach(c => context.CaseStatuses.Add(c));
            //caCS.ForEach(c => context.ChildAbductionCaseStatuses.Add(c));
            //wCS.ForEach(c => context.WarrantCaseStatuses.Add(c));
            crS.ForEach(c => context.CaseReviewStatuses.Add(c));
            dT.ForEach(c => context.DocumentTypes.Add(c));
            dS.ForEach(c => context.DocumentStatuses.Add(c));
            iC.ForEach(c => context.IssuingCountries.Add(c));
            g.ForEach(c => context.Genders.Add(c));
            pCC.ForEach(c => context.AttendanceNoteCodes.Add(c));
            //sF.ForEach(c => context.SolicitorsFirms.Add(c));
            cR.ForEach(c => context.ChildRelationships.Add(c));
            d.ForEach(c => context.Divisions.Add(c));
            faq.ForEach(c => context.FAQs.Add(c));
            context.SaveChanges();
            //s.ForEach(c => context.Solicitors.Add(c));
            //context.SaveChanges();
            //w.ForEach(c => context.Warrants.Add(c));
            //context.SaveChanges();
            //ca.ForEach(c => context.ChildAbductions.Add(c));
            //context.SaveChanges();

            //configure constraints
            //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX UX_Preferences_StudentId_PresentationId ON Preferences (StudentId, PresentationId)")
        }
    }
}