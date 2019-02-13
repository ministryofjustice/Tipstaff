CREATE SCHEMA IF NOT EXISTS "dbo"
;

CREATE TABLE "dbo"."Addresses"("addressID" serial4 NOT NULL,"addresseeName" varchar(100),"addressLine1" varchar(100) NOT NULL,"addressLine2" varchar(100),"addressLine3" varchar(100),"town" varchar(100),"county" varchar(100),"postcode" varchar(10),"phone" varchar(20),"tipstaffRecordID" int4 NOT NULL,CONSTRAINT "PK_dbo.Addresses" PRIMARY KEY ("addressID"))
;

CREATE INDEX "Addresses_IX_tipstaffRecordID" ON "dbo"."Addresses" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."TipstaffRecords"("tipstaffRecordID" serial4 NOT NULL,"createdBy" varchar(50) NOT NULL,"createdOn" timestamp NOT NULL,"protectiveMarkingID" int4 NOT NULL,"resultID" int4,"nextReviewDate" timestamp NOT NULL,"resultDate" timestamp,"DateExecuted" timestamp,"arrestCount" int4,"prisonCount" int4,"resultEnteredBy" varchar(50),"NPO" text,"caseStatusID" int4 NOT NULL,"sentSCD26" timestamp,"orderDated" timestamp,"orderReceived" timestamp,"officerDealing" varchar(50),"EldestChild" varchar(50),"caOrderTypeID" int4,"caseNumber" varchar(50),"expiryDate" timestamp,"RespondentName" varchar(153),"divisionID" int4,"DateCirculated" timestamp,"Discriminator" varchar(128) NOT NULL,CONSTRAINT "PK_dbo.TipstaffRecords" PRIMARY KEY ("tipstaffRecordID"))
;

CREATE INDEX "TipstaffRecords_IX_protectiveMarkingID" ON "dbo"."TipstaffRecords" ("protectiveMarkingID")
;

CREATE INDEX "TipstaffRecords_IX_resultID" ON "dbo"."TipstaffRecords" ("resultID")
;

CREATE INDEX "TipstaffRecords_IX_caseStatusID" ON "dbo"."TipstaffRecords" ("caseStatusID")
;

CREATE INDEX "TipstaffRecords_IX_caOrderTypeID" ON "dbo"."TipstaffRecords" ("caOrderTypeID")
;

CREATE INDEX "TipstaffRecords_IX_divisionID" ON "dbo"."TipstaffRecords" ("divisionID")
;

CREATE TABLE "dbo"."AttendanceNotes"("AttendanceNoteID" serial4 NOT NULL,"callDated" timestamp NOT NULL,"callStarted" timestamp,"callEnded" timestamp,"callDetails" varchar(1000) NOT NULL,"AttendanceNoteCodeID" int4 NOT NULL,"tipstaffRecordID" int4 NOT NULL,CONSTRAINT "PK_dbo.AttendanceNotes" PRIMARY KEY ("AttendanceNoteID"))
;

CREATE INDEX "AttendanceNotes_IX_AttendanceNoteCodeID" ON "dbo"."AttendanceNotes" ("AttendanceNoteCodeID")
;

CREATE INDEX "AttendanceNotes_IX_tipstaffRecordID" ON "dbo"."AttendanceNotes" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."AttendanceNoteCodes"("AttendanceNoteCodeID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.AttendanceNoteCodes" PRIMARY KEY ("AttendanceNoteCodeID"))
;

CREATE TABLE "dbo"."CaseReviews"("caseReviewID" serial4 NOT NULL,"reviewDate" timestamp,"actionTaken" varchar(800),"caseReviewStatusID" int4 NOT NULL,"nextReviewDate" timestamp NOT NULL,"tipstaffRecordID" int4 NOT NULL,CONSTRAINT "PK_dbo.CaseReviews" PRIMARY KEY ("caseReviewID"))
;

CREATE INDEX "CaseReviews_IX_caseReviewStatusID" ON "dbo"."CaseReviews" ("caseReviewStatusID")
;

CREATE INDEX "CaseReviews_IX_tipstaffRecordID" ON "dbo"."CaseReviews" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."CaseReviewStatus"("caseReviewStatusID" serial4 NOT NULL,"Detail" varchar(20) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.CaseReviewStatus" PRIMARY KEY ("caseReviewStatusID"))
;

CREATE TABLE "dbo"."CaseStatus"("caseStatusID" serial4 NOT NULL,"Detail" varchar(30) NOT NULL,"active" boolean NOT NULL,"sequence" int4 NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.CaseStatus" PRIMARY KEY ("caseStatusID"))
;

CREATE TABLE "dbo"."Documents"("documentID" serial4 NOT NULL,"documentReference" varchar(60) NOT NULL,"countryID" int4,"nationalityID" int4 NOT NULL,"documentStatusID" int4 NOT NULL,"documentTypeID" int4 NOT NULL,"templateID" int4,"createdOn" timestamp NOT NULL,"createdBy" varchar(50),"tipstaffRecordID" int4 NOT NULL,"binaryFile" bytea,"fileName" varchar(256),"mimeType" varchar(300),CONSTRAINT "PK_dbo.Documents" PRIMARY KEY ("documentID"))
;

CREATE INDEX "Documents_IX_countryID" ON "dbo"."Documents" ("countryID")
;

CREATE INDEX "Documents_IX_nationalityID" ON "dbo"."Documents" ("nationalityID")
;

CREATE INDEX "Documents_IX_documentStatusID" ON "dbo"."Documents" ("documentStatusID")
;

CREATE INDEX "Documents_IX_documentTypeID" ON "dbo"."Documents" ("documentTypeID")
;

CREATE INDEX "Documents_IX_templateID" ON "dbo"."Documents" ("templateID")
;

CREATE INDEX "Documents_IX_tipstaffRecordID" ON "dbo"."Documents" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."Countries"("countryID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Countries" PRIMARY KEY ("countryID"))
;

CREATE TABLE "dbo"."DocumentStatus"("DocumentStatusID" serial4 NOT NULL,"Detail" varchar(40) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.DocumentStatus" PRIMARY KEY ("DocumentStatusID"))
;

CREATE TABLE "dbo"."DocumentTypes"("documentTypeID" serial4 NOT NULL,"Detail" varchar(100) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.DocumentTypes" PRIMARY KEY ("documentTypeID"))
;

CREATE TABLE "dbo"."Nationalities"("nationalityID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Nationalities" PRIMARY KEY ("nationalityID"))
;

CREATE TABLE "dbo"."Templates"("templateID" serial4 NOT NULL,"Discriminator" varchar(128) NOT NULL,"templateName" varchar(80) NOT NULL,"templateXML" text NOT NULL,"addresseeRequired" boolean NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Templates" PRIMARY KEY ("templateID"))
;

CREATE TABLE "dbo"."TipstaffRecordSolicitors"("tipstaffRecordID" int4 NOT NULL,"solicitorID" int4 NOT NULL,CONSTRAINT "PK_dbo.TipstaffRecordSolicitors" PRIMARY KEY ("tipstaffRecordID","solicitorID"))
;

CREATE INDEX "TipstaffRecordSolicitors_IX_tipstaffRecordID" ON "dbo"."TipstaffRecordSolicitors" ("tipstaffRecordID")
;

CREATE INDEX "TipstaffRecordSolicitors_IX_solicitorID" ON "dbo"."TipstaffRecordSolicitors" ("solicitorID")
;

CREATE TABLE "dbo"."Solicitors"("solicitorID" serial4 NOT NULL,"firstName" varchar(50),"lastName" varchar(50) NOT NULL,"solicitorFirmID" int4,"salutationID" int4 NOT NULL,"phoneDayTime" varchar(20),"phoneOutofHours" varchar(20),"email" varchar(60),"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Solicitors" PRIMARY KEY ("solicitorID"))
;

CREATE INDEX "Solicitors_IX_solicitorFirmID" ON "dbo"."Solicitors" ("solicitorFirmID")
;

CREATE INDEX "Solicitors_IX_salutationID" ON "dbo"."Solicitors" ("salutationID")
;

CREATE TABLE "dbo"."Salutations"("salutationID" serial4 NOT NULL,"Detail" varchar(10) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Salutations" PRIMARY KEY ("salutationID"))
;

CREATE TABLE "dbo"."SolicitorFirms"("solicitorFirmID" serial4 NOT NULL,"firmName" varchar(50) NOT NULL,"addressLine1" varchar(100) NOT NULL,"addressLine2" varchar(100),"addressLine3" varchar(100),"town" varchar(100),"county" varchar(100),"postcode" varchar(10),"DX" varchar(50),"phoneDayTime" varchar(20),"phoneOutofHours" varchar(20),"email" varchar(60),"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.SolicitorFirms" PRIMARY KEY ("solicitorFirmID"))
;

CREATE TABLE "dbo"."TipstaffPoliceForces"("tipstaffRecordPoliceForceID" serial4 NOT NULL,"tipstaffRecordID" int4 NOT NULL,"policeForceID" int4 NOT NULL,CONSTRAINT "PK_dbo.TipstaffPoliceForces" PRIMARY KEY ("tipstaffRecordPoliceForceID"))
;

CREATE INDEX "TipstaffPoliceForces_IX_tipstaffRecordID" ON "dbo"."TipstaffPoliceForces" ("tipstaffRecordID")
;

CREATE INDEX "TipstaffPoliceForces_IX_policeForceID" ON "dbo"."TipstaffPoliceForces" ("policeForceID")
;

CREATE TABLE "dbo"."PoliceForces"("policeForceID" serial4 NOT NULL,"policeForceName" varchar(255) NOT NULL,"policeForceEmail" varchar(255) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.PoliceForces" PRIMARY KEY ("policeForceID"))
;

CREATE TABLE "dbo"."ProtectiveMarkings"("protectiveMarkingID" serial4 NOT NULL,"Detail" varchar(15) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.ProtectiveMarkings" PRIMARY KEY ("protectiveMarkingID"))
;

CREATE TABLE "dbo"."Respondents"("respondentID" serial4 NOT NULL,"nameLast" varchar(50) NOT NULL,"nameFirst" varchar(50),"nameMiddle" varchar(50),"dateOfBirth" timestamp,"genderID" int4 NOT NULL,"childRelationshipID" int4 NOT NULL,"hairColour" varchar(50) NOT NULL,"eyeColour" varchar(50) NOT NULL,"skinColourID" int4 NOT NULL,"height" varchar(50) NOT NULL,"build" varchar(50) NOT NULL,"specialfeatures" varchar(250) NOT NULL,"countryID" int4 NOT NULL,"nationalityID" int4 NOT NULL,"riskOfViolence" varchar(100) NOT NULL,"riskOfDrugs" varchar(100) NOT NULL,"tipstaffRecordID" int4 NOT NULL,"PNCID" text,CONSTRAINT "PK_dbo.Respondents" PRIMARY KEY ("respondentID"))
;

CREATE INDEX "Respondents_IX_genderID" ON "dbo"."Respondents" ("genderID")
;

CREATE INDEX "Respondents_IX_childRelationshipID" ON "dbo"."Respondents" ("childRelationshipID")
;

CREATE INDEX "Respondents_IX_skinColourID" ON "dbo"."Respondents" ("skinColourID")
;

CREATE INDEX "Respondents_IX_countryID" ON "dbo"."Respondents" ("countryID")
;

CREATE INDEX "Respondents_IX_nationalityID" ON "dbo"."Respondents" ("nationalityID")
;

CREATE INDEX "Respondents_IX_tipstaffRecordID" ON "dbo"."Respondents" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."ChildRelationships"("childRelationshipID" serial4 NOT NULL,"Detail" varchar(40) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.ChildRelationships" PRIMARY KEY ("childRelationshipID"))
;

CREATE TABLE "dbo"."Genders"("genderID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Genders" PRIMARY KEY ("genderID"))
;

CREATE TABLE "dbo"."SkinColours"("skinColourID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.SkinColours" PRIMARY KEY ("skinColourID"))
;

CREATE TABLE "dbo"."Results"("resultID" serial4 NOT NULL,"Detail" varchar(20) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Results" PRIMARY KEY ("resultID"))
;

CREATE TABLE "dbo"."Applicants"("ApplicantID" serial4 NOT NULL,"salutationID" int4 NOT NULL,"nameLast" varchar(50) NOT NULL,"nameFirst" varchar(50),"addressLine1" varchar(100) NOT NULL,"addressLine2" varchar(100),"addressLine3" varchar(100),"town" varchar(100),"county" varchar(100),"postcode" varchar(10) NOT NULL,"phone" varchar(20),"tipstaffRecordID" int4 NOT NULL,CONSTRAINT "PK_dbo.Applicants" PRIMARY KEY ("ApplicantID"))
;

CREATE INDEX "Applicants_IX_salutationID" ON "dbo"."Applicants" ("salutationID")
;

CREATE INDEX "Applicants_IX_tipstaffRecordID" ON "dbo"."Applicants" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."CAOrderTypes"("caOrderTypeID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.CAOrderTypes" PRIMARY KEY ("caOrderTypeID"))
;

CREATE TABLE "dbo"."Children"("childID" serial4 NOT NULL,"nameLast" varchar(50) NOT NULL,"nameFirst" varchar(50),"nameMiddle" varchar(50),"dateOfBirth" timestamp,"genderID" int4 NOT NULL,"height" text NOT NULL,"build" text NOT NULL,"hairColour" text NOT NULL,"eyeColour" text NOT NULL,"skinColourID" int4 NOT NULL,"specialfeatures" text NOT NULL,"countryID" int4 NOT NULL,"nationalityID" int4 NOT NULL,"tipstaffRecordID" int4 NOT NULL,"PNCID" text,CONSTRAINT "PK_dbo.Children" PRIMARY KEY ("childID"))
;

CREATE INDEX "Children_IX_genderID" ON "dbo"."Children" ("genderID")
;

CREATE INDEX "Children_IX_skinColourID" ON "dbo"."Children" ("skinColourID")
;

CREATE INDEX "Children_IX_countryID" ON "dbo"."Children" ("countryID")
;

CREATE INDEX "Children_IX_nationalityID" ON "dbo"."Children" ("nationalityID")
;

CREATE INDEX "Children_IX_tipstaffRecordID" ON "dbo"."Children" ("tipstaffRecordID")
;

CREATE TABLE "dbo"."Divisions"("divisionID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"Prefix" text NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Divisions" PRIMARY KEY ("divisionID"))
;

CREATE TABLE "dbo"."AuditEventDescriptions"("idAuditEventDescription" serial4 NOT NULL,"AuditDescription" varchar(40) NOT NULL,CONSTRAINT "PK_dbo.AuditEventDescriptions" PRIMARY KEY ("idAuditEventDescription"))
;

CREATE TABLE "dbo"."AuditEvents"("idAuditEvent" serial4 NOT NULL,"EventDate" timestamp NOT NULL,"UserID" varchar(40) NOT NULL,"idAuditEventDescription" int4 NOT NULL,"RecordChanged" varchar(256) NOT NULL,"RecordAddedTo" int4,"DeletedReasonID" int4,CONSTRAINT "PK_dbo.AuditEvents" PRIMARY KEY ("idAuditEvent"))
;

CREATE INDEX "AuditEvents_IX_idAuditEventDescription" ON "dbo"."AuditEvents" ("idAuditEventDescription")
;

CREATE INDEX "AuditEvents_IX_DeletedReasonID" ON "dbo"."AuditEvents" ("DeletedReasonID")
;

CREATE TABLE "dbo"."AuditEventDataRows"("idAuditData" serial4 NOT NULL,"idAuditEvent" int4 NOT NULL,"ColumnName" varchar(200) NOT NULL,"Was" varchar(200) NOT NULL,"Now" varchar(200) NOT NULL,CONSTRAINT "PK_dbo.AuditEventDataRows" PRIMARY KEY ("idAuditData"))
;

CREATE INDEX "AuditEventDataRows_IX_idAuditEvent" ON "dbo"."AuditEventDataRows" ("idAuditEvent")
;

CREATE TABLE "dbo"."DeletedReasons"("deletedReasonID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.DeletedReasons" PRIMARY KEY ("deletedReasonID"))
;

CREATE TABLE "dbo"."Contacts"("contactID" serial4 NOT NULL,"salutationID" int4 NOT NULL,"firstName" varchar(50),"lastName" varchar(50) NOT NULL,"addressLine1" varchar(100) NOT NULL,"addressLine2" varchar(100),"addressLine3" varchar(100),"town" varchar(100),"county" varchar(100),"postcode" varchar(10) NOT NULL,"DX" varchar(50),"phoneHome" varchar(20),"phoneMobile" varchar(20),"email" varchar(60),"notes" varchar(2000),"contactTypeID" int4 NOT NULL,CONSTRAINT "PK_dbo.Contacts" PRIMARY KEY ("contactID"))
;

CREATE INDEX "Contacts_IX_salutationID" ON "dbo"."Contacts" ("salutationID")
;

CREATE INDEX "Contacts_IX_contactTypeID" ON "dbo"."Contacts" ("contactTypeID")
;

CREATE TABLE "dbo"."ContactTypes"("contactTypeID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.ContactTypes" PRIMARY KEY ("contactTypeID"))
;

CREATE TABLE "dbo"."DeletedTipstaffRecords"("TipstaffRecordID" int4 NOT NULL,"deletedReasonID" int4 NOT NULL,"discriminator" varchar(50) NOT NULL,"UniqueRecordID" varchar(10) NOT NULL,CONSTRAINT "PK_dbo.DeletedTipstaffRecords" PRIMARY KEY ("TipstaffRecordID","deletedReasonID"))
;

CREATE INDEX "DeletedTipstaffRecords_IX_deletedReasonID" ON "dbo"."DeletedTipstaffRecords" ("deletedReasonID")
;

CREATE TABLE "dbo"."FAQs"("faqID" serial4 NOT NULL,"loggedInUser" boolean NOT NULL,"question" varchar(150) NOT NULL,"answer" varchar(4000) NOT NULL,CONSTRAINT "PK_dbo.FAQs" PRIMARY KEY ("faqID"))
;

CREATE TABLE "dbo"."FaxCodes"("faxCodeID" serial4 NOT NULL,"Detail" varchar(50) NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.FaxCodes" PRIMARY KEY ("faxCodeID"))
;

CREATE TABLE "dbo"."Roles"("strength" int4 NOT NULL,"Detail" varchar(20) NOT NULL,CONSTRAINT "PK_dbo.Roles" PRIMARY KEY ("strength"))
;

CREATE TABLE "dbo"."Users"("UserID" serial4 NOT NULL,"Name" varchar(150) NOT NULL,"DisplayName" varchar(30) NOT NULL,"LastActive" timestamp,"RoleStrength" int4 NOT NULL,CONSTRAINT "PK_dbo.Users" PRIMARY KEY ("UserID"))
;

CREATE INDEX "Users_IX_RoleStrength" ON "dbo"."Users" ("RoleStrength")
;

ALTER TABLE "dbo"."Addresses" ADD CONSTRAINT "FK_dbo.Addresses_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecords" ADD CONSTRAINT "FK_dbo.TipstaffRecords_dbo.CaseStatus_caseStatusID" FOREIGN KEY ("caseStatusID") REFERENCES "dbo"."CaseStatus" ("caseStatusID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecords" ADD CONSTRAINT "FK_dbo.TipstaffRecords_dbo.ProtectiveMarkings_protectiveMarkingID" FOREIGN KEY ("protectiveMarkingID") REFERENCES "dbo"."ProtectiveMarkings" ("protectiveMarkingID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecords" ADD CONSTRAINT "FK_dbo.TipstaffRecords_dbo.Results_resultID" FOREIGN KEY ("resultID") REFERENCES "dbo"."Results" ("resultID")
;

ALTER TABLE "dbo"."TipstaffRecords" ADD CONSTRAINT "FK_dbo.TipstaffRecords_dbo.CAOrderTypes_caOrderTypeID" FOREIGN KEY ("caOrderTypeID") REFERENCES "dbo"."CAOrderTypes" ("caOrderTypeID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecords" ADD CONSTRAINT "FK_dbo.TipstaffRecords_dbo.Divisions_divisionID" FOREIGN KEY ("divisionID") REFERENCES "dbo"."Divisions" ("divisionID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AttendanceNotes" ADD CONSTRAINT "FK_dbo.AttendanceNotes_dbo.AttendanceNoteCodes_AttendanceNoteCodeID" FOREIGN KEY ("AttendanceNoteCodeID") REFERENCES "dbo"."AttendanceNoteCodes" ("AttendanceNoteCodeID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AttendanceNotes" ADD CONSTRAINT "FK_dbo.AttendanceNotes_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."CaseReviews" ADD CONSTRAINT "FK_dbo.CaseReviews_dbo.CaseReviewStatus_caseReviewStatusID" FOREIGN KEY ("caseReviewStatusID") REFERENCES "dbo"."CaseReviewStatus" ("caseReviewStatusID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."CaseReviews" ADD CONSTRAINT "FK_dbo.CaseReviews_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.Countries_countryID" FOREIGN KEY ("countryID") REFERENCES "dbo"."Countries" ("countryID")
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.DocumentStatus_documentStatusID" FOREIGN KEY ("documentStatusID") REFERENCES "dbo"."DocumentStatus" ("DocumentStatusID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.DocumentTypes_documentTypeID" FOREIGN KEY ("documentTypeID") REFERENCES "dbo"."DocumentTypes" ("documentTypeID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.Nationalities_nationalityID" FOREIGN KEY ("nationalityID") REFERENCES "dbo"."Nationalities" ("nationalityID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.Templates_templateID" FOREIGN KEY ("templateID") REFERENCES "dbo"."Templates" ("templateID")
;

ALTER TABLE "dbo"."Documents" ADD CONSTRAINT "FK_dbo.Documents_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecordSolicitors" ADD CONSTRAINT "FK_dbo.TipstaffRecordSolicitors_dbo.Solicitors_solicitorID" FOREIGN KEY ("solicitorID") REFERENCES "dbo"."Solicitors" ("solicitorID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffRecordSolicitors" ADD CONSTRAINT "FK_dbo.TipstaffRecordSolicitors_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Solicitors" ADD CONSTRAINT "FK_dbo.Solicitors_dbo.Salutations_salutationID" FOREIGN KEY ("salutationID") REFERENCES "dbo"."Salutations" ("salutationID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Solicitors" ADD CONSTRAINT "FK_dbo.Solicitors_dbo.SolicitorFirms_solicitorFirmID" FOREIGN KEY ("solicitorFirmID") REFERENCES "dbo"."SolicitorFirms" ("solicitorFirmID")
;

ALTER TABLE "dbo"."TipstaffPoliceForces" ADD CONSTRAINT "FK_dbo.TipstaffPoliceForces_dbo.PoliceForces_policeForceID" FOREIGN KEY ("policeForceID") REFERENCES "dbo"."PoliceForces" ("policeForceID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."TipstaffPoliceForces" ADD CONSTRAINT "FK_dbo.TipstaffPoliceForces_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.ChildRelationships_childRelationshipID" FOREIGN KEY ("childRelationshipID") REFERENCES "dbo"."ChildRelationships" ("childRelationshipID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.Countries_countryID" FOREIGN KEY ("countryID") REFERENCES "dbo"."Countries" ("countryID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.Genders_genderID" FOREIGN KEY ("genderID") REFERENCES "dbo"."Genders" ("genderID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.Nationalities_nationalityID" FOREIGN KEY ("nationalityID") REFERENCES "dbo"."Nationalities" ("nationalityID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.SkinColours_skinColourID" FOREIGN KEY ("skinColourID") REFERENCES "dbo"."SkinColours" ("skinColourID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Respondents" ADD CONSTRAINT "FK_dbo.Respondents_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Applicants" ADD CONSTRAINT "FK_dbo.Applicants_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Applicants" ADD CONSTRAINT "FK_dbo.Applicants_dbo.Salutations_salutationID" FOREIGN KEY ("salutationID") REFERENCES "dbo"."Salutations" ("salutationID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Children" ADD CONSTRAINT "FK_dbo.Children_dbo.TipstaffRecords_tipstaffRecordID" FOREIGN KEY ("tipstaffRecordID") REFERENCES "dbo"."TipstaffRecords" ("tipstaffRecordID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Children" ADD CONSTRAINT "FK_dbo.Children_dbo.Countries_countryID" FOREIGN KEY ("countryID") REFERENCES "dbo"."Countries" ("countryID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Children" ADD CONSTRAINT "FK_dbo.Children_dbo.Genders_genderID" FOREIGN KEY ("genderID") REFERENCES "dbo"."Genders" ("genderID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Children" ADD CONSTRAINT "FK_dbo.Children_dbo.Nationalities_nationalityID" FOREIGN KEY ("nationalityID") REFERENCES "dbo"."Nationalities" ("nationalityID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Children" ADD CONSTRAINT "FK_dbo.Children_dbo.SkinColours_skinColourID" FOREIGN KEY ("skinColourID") REFERENCES "dbo"."SkinColours" ("skinColourID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AuditEvents" ADD CONSTRAINT "FK_dbo.AuditEvents_dbo.AuditEventDescriptions_idAuditEventDescription" FOREIGN KEY ("idAuditEventDescription") REFERENCES "dbo"."AuditEventDescriptions" ("idAuditEventDescription") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AuditEvents" ADD CONSTRAINT "FK_dbo.AuditEvents_dbo.DeletedReasons_DeletedReasonID" FOREIGN KEY ("DeletedReasonID") REFERENCES "dbo"."DeletedReasons" ("deletedReasonID")
;

ALTER TABLE "dbo"."AuditEventDataRows" ADD CONSTRAINT "FK_dbo.AuditEventDataRows_dbo.AuditEvents_idAuditEvent" FOREIGN KEY ("idAuditEvent") REFERENCES "dbo"."AuditEvents" ("idAuditEvent") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Contacts" ADD CONSTRAINT "FK_dbo.Contacts_dbo.ContactTypes_contactTypeID" FOREIGN KEY ("contactTypeID") REFERENCES "dbo"."ContactTypes" ("contactTypeID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Contacts" ADD CONSTRAINT "FK_dbo.Contacts_dbo.Salutations_salutationID" FOREIGN KEY ("salutationID") REFERENCES "dbo"."Salutations" ("salutationID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."DeletedTipstaffRecords" ADD CONSTRAINT "FK_dbo.DeletedTipstaffRecords_dbo.DeletedReasons_deletedReasonID" FOREIGN KEY ("deletedReasonID") REFERENCES "dbo"."DeletedReasons" ("deletedReasonID") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Users" ADD CONSTRAINT "FK_dbo.Users_dbo.Roles_RoleStrength" FOREIGN KEY ("RoleStrength") REFERENCES "dbo"."Roles" ("strength") ON DELETE CASCADE
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL,"ContextKey" varchar(300) NOT NULL,"Model" bytea NOT NULL,"ProductVersion" varchar(32) NOT NULL,CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201902131202108_InitialMigration',E'Tipstaff.Models.TipstaffDB',decode('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', 'base64'),E'6.2.0-61023')
