namespace Tipstaff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        addressID = c.Int(nullable: false, identity: true),
                        addresseeName = c.String(maxLength: 100),
                        addressLine1 = c.String(nullable: false, maxLength: 100),
                        addressLine2 = c.String(maxLength: 100),
                        addressLine3 = c.String(maxLength: 100),
                        town = c.String(maxLength: 100),
                        county = c.String(maxLength: 100),
                        postcode = c.String(maxLength: 10),
                        phone = c.String(maxLength: 20),
                        tipstaffRecordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.addressID)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.TipstaffRecords",
                c => new
                    {
                        tipstaffRecordID = c.Int(nullable: false, identity: true),
                        createdBy = c.String(nullable: false, maxLength: 50),
                        createdOn = c.DateTime(nullable: false),
                        protectiveMarkingID = c.Int(nullable: false),
                        resultID = c.Int(),
                        nextReviewDate = c.DateTime(nullable: false),
                        resultDate = c.DateTime(),
                        DateExecuted = c.DateTime(),
                        arrestCount = c.Int(),
                        prisonCount = c.Int(),
                        resultEnteredBy = c.String(maxLength: 50),
                        NPO = c.String(),
                        caseStatusID = c.Int(nullable: false),
                        sentSCD26 = c.DateTime(),
                        orderDated = c.DateTime(),
                        orderReceived = c.DateTime(),
                        officerDealing = c.String(maxLength: 50),
                        EldestChild = c.String(maxLength: 50),
                        caOrderTypeID = c.Int(),
                        caseNumber = c.String(maxLength: 50),
                        expiryDate = c.DateTime(),
                        RespondentName = c.String(maxLength: 153),
                        divisionID = c.Int(),
                        DateCirculated = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.tipstaffRecordID)
                .ForeignKey("dbo.CaseStatus", t => t.caseStatusID, cascadeDelete: true)
                .ForeignKey("dbo.ProtectiveMarkings", t => t.protectiveMarkingID, cascadeDelete: true)
                .ForeignKey("dbo.Results", t => t.resultID)
                .ForeignKey("dbo.CAOrderTypes", t => t.caOrderTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.divisionID, cascadeDelete: true)
                .Index(t => t.protectiveMarkingID)
                .Index(t => t.resultID)
                .Index(t => t.caseStatusID)
                .Index(t => t.caOrderTypeID)
                .Index(t => t.divisionID);
            
            CreateTable(
                "dbo.AttendanceNotes",
                c => new
                    {
                        AttendanceNoteID = c.Int(nullable: false, identity: true),
                        callDated = c.DateTime(nullable: false),
                        callStarted = c.DateTime(),
                        callEnded = c.DateTime(),
                        callDetails = c.String(nullable: false, maxLength: 1000),
                        AttendanceNoteCodeID = c.Int(nullable: false),
                        tipstaffRecordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttendanceNoteID)
                .ForeignKey("dbo.AttendanceNoteCodes", t => t.AttendanceNoteCodeID, cascadeDelete: true)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.AttendanceNoteCodeID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.AttendanceNoteCodes",
                c => new
                    {
                        AttendanceNoteCodeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AttendanceNoteCodeID);
            
            CreateTable(
                "dbo.CaseReviews",
                c => new
                    {
                        caseReviewID = c.Int(nullable: false, identity: true),
                        reviewDate = c.DateTime(),
                        actionTaken = c.String(maxLength: 800),
                        caseReviewStatusID = c.Int(nullable: false),
                        nextReviewDate = c.DateTime(nullable: false),
                        tipstaffRecordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.caseReviewID)
                .ForeignKey("dbo.CaseReviewStatus", t => t.caseReviewStatusID, cascadeDelete: true)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.caseReviewStatusID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.CaseReviewStatus",
                c => new
                    {
                        caseReviewStatusID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 20),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.caseReviewStatusID);
            
            CreateTable(
                "dbo.CaseStatus",
                c => new
                    {
                        caseStatusID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 30),
                        active = c.Boolean(nullable: false),
                        sequence = c.Int(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.caseStatusID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        documentID = c.Int(nullable: false, identity: true),
                        documentReference = c.String(nullable: false, maxLength: 60),
                        countryID = c.Int(),
                        nationalityID = c.Int(nullable: false),
                        documentStatusID = c.Int(nullable: false),
                        documentTypeID = c.Int(nullable: false),
                        templateID = c.Int(),
                        createdOn = c.DateTime(nullable: false),
                        createdBy = c.String(maxLength: 50),
                        tipstaffRecordID = c.Int(nullable: false),
                        binaryFile = c.Binary(),
                        fileName = c.String(maxLength: 256),
                        mimeType = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.documentID)
                .ForeignKey("dbo.Countries", t => t.countryID)
                .ForeignKey("dbo.DocumentStatus", t => t.documentStatusID, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.documentTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Nationalities", t => t.nationalityID, cascadeDelete: true)
                .ForeignKey("dbo.Templates", t => t.templateID)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.countryID)
                .Index(t => t.nationalityID)
                .Index(t => t.documentStatusID)
                .Index(t => t.documentTypeID)
                .Index(t => t.templateID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        countryID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.countryID);
            
            CreateTable(
                "dbo.DocumentStatus",
                c => new
                    {
                        DocumentStatusID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 40),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.DocumentStatusID);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        documentTypeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 100),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.documentTypeID);
            
            CreateTable(
                "dbo.Nationalities",
                c => new
                    {
                        nationalityID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.nationalityID);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        templateID = c.Int(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        templateName = c.String(nullable: false, maxLength: 80),
                        templateXML = c.String(nullable: false),
                        addresseeRequired = c.Boolean(nullable: false),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.templateID);
            
            CreateTable(
                "dbo.TipstaffRecordSolicitors",
                c => new
                    {
                        tipstaffRecordID = c.Int(nullable: false),
                        solicitorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.tipstaffRecordID, t.solicitorID })
                .ForeignKey("dbo.Solicitors", t => t.solicitorID, cascadeDelete: true)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.tipstaffRecordID)
                .Index(t => t.solicitorID);
            
            CreateTable(
                "dbo.Solicitors",
                c => new
                    {
                        solicitorID = c.Int(nullable: false, identity: true),
                        firstName = c.String(maxLength: 50),
                        lastName = c.String(nullable: false, maxLength: 50),
                        solicitorFirmID = c.Int(),
                        salutationID = c.Int(nullable: false),
                        phoneDayTime = c.String(maxLength: 20),
                        phoneOutofHours = c.String(maxLength: 20),
                        email = c.String(maxLength: 60),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.solicitorID)
                .ForeignKey("dbo.Salutations", t => t.salutationID, cascadeDelete: true)
                .ForeignKey("dbo.SolicitorFirms", t => t.solicitorFirmID)
                .Index(t => t.solicitorFirmID)
                .Index(t => t.salutationID);
            
            CreateTable(
                "dbo.Salutations",
                c => new
                    {
                        salutationID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 10),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.salutationID);
            
            CreateTable(
                "dbo.SolicitorFirms",
                c => new
                    {
                        solicitorFirmID = c.Int(nullable: false, identity: true),
                        firmName = c.String(nullable: false, maxLength: 50),
                        addressLine1 = c.String(nullable: false, maxLength: 100),
                        addressLine2 = c.String(maxLength: 100),
                        addressLine3 = c.String(maxLength: 100),
                        town = c.String(maxLength: 100),
                        county = c.String(maxLength: 100),
                        postcode = c.String(maxLength: 10),
                        DX = c.String(maxLength: 50),
                        phoneDayTime = c.String(maxLength: 20),
                        phoneOutofHours = c.String(maxLength: 20),
                        email = c.String(maxLength: 60),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.solicitorFirmID);
            
            CreateTable(
                "dbo.TipstaffPoliceForces",
                c => new
                    {
                        tipstaffRecordPoliceForceID = c.Int(nullable: false, identity: true),
                        tipstaffRecordID = c.Int(nullable: false),
                        policeForceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tipstaffRecordPoliceForceID)
                .ForeignKey("dbo.PoliceForces", t => t.policeForceID, cascadeDelete: true)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.tipstaffRecordID)
                .Index(t => t.policeForceID);
            
            CreateTable(
                "dbo.PoliceForces",
                c => new
                    {
                        policeForceID = c.Int(nullable: false, identity: true),
                        policeForceName = c.String(nullable: false, maxLength: 255),
                        policeForceEmail = c.String(nullable: false, maxLength: 255),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.policeForceID);
            
            CreateTable(
                "dbo.ProtectiveMarkings",
                c => new
                    {
                        protectiveMarkingID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 15),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.protectiveMarkingID);
            
            CreateTable(
                "dbo.Respondents",
                c => new
                    {
                        respondentID = c.Int(nullable: false, identity: true),
                        nameLast = c.String(nullable: false, maxLength: 50),
                        nameFirst = c.String(maxLength: 50),
                        nameMiddle = c.String(maxLength: 50),
                        dateOfBirth = c.DateTime(),
                        genderID = c.Int(nullable: false),
                        childRelationshipID = c.Int(nullable: false),
                        hairColour = c.String(nullable: false, maxLength: 50),
                        eyeColour = c.String(nullable: false, maxLength: 50),
                        skinColourID = c.Int(nullable: false),
                        height = c.String(nullable: false, maxLength: 50),
                        build = c.String(nullable: false, maxLength: 50),
                        specialfeatures = c.String(nullable: false, maxLength: 250),
                        countryID = c.Int(nullable: false),
                        nationalityID = c.Int(nullable: false),
                        riskOfViolence = c.String(nullable: false, maxLength: 100),
                        riskOfDrugs = c.String(nullable: false, maxLength: 100),
                        tipstaffRecordID = c.Int(nullable: false),
                        PNCID = c.String(),
                    })
                .PrimaryKey(t => t.respondentID)
                .ForeignKey("dbo.ChildRelationships", t => t.childRelationshipID, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.countryID, cascadeDelete: true)
                .ForeignKey("dbo.Genders", t => t.genderID, cascadeDelete: true)
                .ForeignKey("dbo.Nationalities", t => t.nationalityID, cascadeDelete: true)
                .ForeignKey("dbo.SkinColours", t => t.skinColourID, cascadeDelete: true)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .Index(t => t.genderID)
                .Index(t => t.childRelationshipID)
                .Index(t => t.skinColourID)
                .Index(t => t.countryID)
                .Index(t => t.nationalityID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.ChildRelationships",
                c => new
                    {
                        childRelationshipID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 40),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.childRelationshipID);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        genderID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.genderID);
            
            CreateTable(
                "dbo.SkinColours",
                c => new
                    {
                        skinColourID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.skinColourID);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        resultID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 20),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.resultID);
            
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        ApplicantID = c.Int(nullable: false, identity: true),
                        salutationID = c.Int(nullable: false),
                        nameLast = c.String(nullable: false, maxLength: 50),
                        nameFirst = c.String(maxLength: 50),
                        addressLine1 = c.String(nullable: false, maxLength: 100),
                        addressLine2 = c.String(maxLength: 100),
                        addressLine3 = c.String(maxLength: 100),
                        town = c.String(maxLength: 100),
                        county = c.String(maxLength: 100),
                        postcode = c.String(nullable: false, maxLength: 10),
                        phone = c.String(maxLength: 20),
                        tipstaffRecordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantID)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .ForeignKey("dbo.Salutations", t => t.salutationID, cascadeDelete: true)
                .Index(t => t.salutationID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.CAOrderTypes",
                c => new
                    {
                        caOrderTypeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.caOrderTypeID);
            
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        childID = c.Int(nullable: false, identity: true),
                        nameLast = c.String(nullable: false, maxLength: 50),
                        nameFirst = c.String(maxLength: 50),
                        nameMiddle = c.String(maxLength: 50),
                        dateOfBirth = c.DateTime(),
                        genderID = c.Int(nullable: false),
                        height = c.String(nullable: false),
                        build = c.String(nullable: false),
                        hairColour = c.String(nullable: false),
                        eyeColour = c.String(nullable: false),
                        skinColourID = c.Int(nullable: false),
                        specialfeatures = c.String(nullable: false),
                        countryID = c.Int(nullable: false),
                        nationalityID = c.Int(nullable: false),
                        tipstaffRecordID = c.Int(nullable: false),
                        PNCID = c.String(),
                    })
                .PrimaryKey(t => t.childID)
                .ForeignKey("dbo.TipstaffRecords", t => t.tipstaffRecordID, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.countryID, cascadeDelete: true)
                .ForeignKey("dbo.Genders", t => t.genderID, cascadeDelete: true)
                .ForeignKey("dbo.Nationalities", t => t.nationalityID, cascadeDelete: true)
                .ForeignKey("dbo.SkinColours", t => t.skinColourID, cascadeDelete: true)
                .Index(t => t.genderID)
                .Index(t => t.skinColourID)
                .Index(t => t.countryID)
                .Index(t => t.nationalityID)
                .Index(t => t.tipstaffRecordID);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        divisionID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        Prefix = c.String(nullable: false),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.divisionID);
            
            CreateTable(
                "dbo.AuditEventDescriptions",
                c => new
                    {
                        idAuditEventDescription = c.Int(nullable: false, identity: true),
                        AuditDescription = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.idAuditEventDescription);
            
            CreateTable(
                "dbo.AuditEvents",
                c => new
                    {
                        idAuditEvent = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 40),
                        idAuditEventDescription = c.Int(nullable: false),
                        RecordChanged = c.String(nullable: false, maxLength: 256),
                        RecordAddedTo = c.Int(),
                        DeletedReasonID = c.Int(),
                    })
                .PrimaryKey(t => t.idAuditEvent)
                .ForeignKey("dbo.AuditEventDescriptions", t => t.idAuditEventDescription, cascadeDelete: true)
                .ForeignKey("dbo.DeletedReasons", t => t.DeletedReasonID)
                .Index(t => t.idAuditEventDescription)
                .Index(t => t.DeletedReasonID);
            
            CreateTable(
                "dbo.AuditEventDataRows",
                c => new
                    {
                        idAuditData = c.Int(nullable: false, identity: true),
                        idAuditEvent = c.Int(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 200),
                        Was = c.String(nullable: false, maxLength: 200),
                        Now = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.idAuditData)
                .ForeignKey("dbo.AuditEvents", t => t.idAuditEvent, cascadeDelete: true)
                .Index(t => t.idAuditEvent);
            
            CreateTable(
                "dbo.DeletedReasons",
                c => new
                    {
                        deletedReasonID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.deletedReasonID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        contactID = c.Int(nullable: false, identity: true),
                        salutationID = c.Int(nullable: false),
                        firstName = c.String(maxLength: 50),
                        lastName = c.String(nullable: false, maxLength: 50),
                        addressLine1 = c.String(nullable: false, maxLength: 100),
                        addressLine2 = c.String(maxLength: 100),
                        addressLine3 = c.String(maxLength: 100),
                        town = c.String(maxLength: 100),
                        county = c.String(maxLength: 100),
                        postcode = c.String(nullable: false, maxLength: 10),
                        DX = c.String(maxLength: 50),
                        phoneHome = c.String(maxLength: 20),
                        phoneMobile = c.String(maxLength: 20),
                        email = c.String(maxLength: 60),
                        notes = c.String(maxLength: 2000),
                        contactTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.contactID)
                .ForeignKey("dbo.ContactTypes", t => t.contactTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Salutations", t => t.salutationID, cascadeDelete: true)
                .Index(t => t.salutationID)
                .Index(t => t.contactTypeID);
            
            CreateTable(
                "dbo.ContactTypes",
                c => new
                    {
                        contactTypeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.contactTypeID);
            
            CreateTable(
                "dbo.DeletedTipstaffRecords",
                c => new
                    {
                        TipstaffRecordID = c.Int(nullable: false),
                        deletedReasonID = c.Int(nullable: false),
                        discriminator = c.String(nullable: false, maxLength: 50),
                        UniqueRecordID = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.TipstaffRecordID, t.deletedReasonID })
                .ForeignKey("dbo.DeletedReasons", t => t.deletedReasonID, cascadeDelete: true)
                .Index(t => t.deletedReasonID);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        faqID = c.Int(nullable: false, identity: true),
                        loggedInUser = c.Boolean(nullable: false),
                        question = c.String(nullable: false, maxLength: 150),
                        answer = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.faqID);
            
            CreateTable(
                "dbo.FaxCodes",
                c => new
                    {
                        faxCodeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false, maxLength: 50),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.faxCodeID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        strength = c.Int(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.strength);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        DisplayName = c.String(nullable: false, maxLength: 30),
                        LastActive = c.DateTime(),
                        RoleStrength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleStrength, cascadeDelete: true)
                .Index(t => t.RoleStrength);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleStrength", "dbo.Roles");
            DropForeignKey("dbo.DeletedTipstaffRecords", "deletedReasonID", "dbo.DeletedReasons");
            DropForeignKey("dbo.Contacts", "salutationID", "dbo.Salutations");
            DropForeignKey("dbo.Contacts", "contactTypeID", "dbo.ContactTypes");
            DropForeignKey("dbo.AuditEvents", "DeletedReasonID", "dbo.DeletedReasons");
            DropForeignKey("dbo.AuditEvents", "idAuditEventDescription", "dbo.AuditEventDescriptions");
            DropForeignKey("dbo.AuditEventDataRows", "idAuditEvent", "dbo.AuditEvents");
            DropForeignKey("dbo.TipstaffRecords", "divisionID", "dbo.Divisions");
            DropForeignKey("dbo.Children", "skinColourID", "dbo.SkinColours");
            DropForeignKey("dbo.Children", "nationalityID", "dbo.Nationalities");
            DropForeignKey("dbo.Children", "genderID", "dbo.Genders");
            DropForeignKey("dbo.Children", "countryID", "dbo.Countries");
            DropForeignKey("dbo.Children", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.TipstaffRecords", "caOrderTypeID", "dbo.CAOrderTypes");
            DropForeignKey("dbo.Applicants", "salutationID", "dbo.Salutations");
            DropForeignKey("dbo.Applicants", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.TipstaffRecords", "resultID", "dbo.Results");
            DropForeignKey("dbo.Respondents", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.Respondents", "skinColourID", "dbo.SkinColours");
            DropForeignKey("dbo.Respondents", "nationalityID", "dbo.Nationalities");
            DropForeignKey("dbo.Respondents", "genderID", "dbo.Genders");
            DropForeignKey("dbo.Respondents", "countryID", "dbo.Countries");
            DropForeignKey("dbo.Respondents", "childRelationshipID", "dbo.ChildRelationships");
            DropForeignKey("dbo.TipstaffRecords", "protectiveMarkingID", "dbo.ProtectiveMarkings");
            DropForeignKey("dbo.TipstaffPoliceForces", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.TipstaffPoliceForces", "policeForceID", "dbo.PoliceForces");
            DropForeignKey("dbo.TipstaffRecordSolicitors", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.TipstaffRecordSolicitors", "solicitorID", "dbo.Solicitors");
            DropForeignKey("dbo.Solicitors", "solicitorFirmID", "dbo.SolicitorFirms");
            DropForeignKey("dbo.Solicitors", "salutationID", "dbo.Salutations");
            DropForeignKey("dbo.Documents", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.Documents", "templateID", "dbo.Templates");
            DropForeignKey("dbo.Documents", "nationalityID", "dbo.Nationalities");
            DropForeignKey("dbo.Documents", "documentTypeID", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "documentStatusID", "dbo.DocumentStatus");
            DropForeignKey("dbo.Documents", "countryID", "dbo.Countries");
            DropForeignKey("dbo.TipstaffRecords", "caseStatusID", "dbo.CaseStatus");
            DropForeignKey("dbo.CaseReviews", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.CaseReviews", "caseReviewStatusID", "dbo.CaseReviewStatus");
            DropForeignKey("dbo.AttendanceNotes", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropForeignKey("dbo.AttendanceNotes", "AttendanceNoteCodeID", "dbo.AttendanceNoteCodes");
            DropForeignKey("dbo.Addresses", "tipstaffRecordID", "dbo.TipstaffRecords");
            DropIndex("dbo.Users", new[] { "RoleStrength" });
            DropIndex("dbo.DeletedTipstaffRecords", new[] { "deletedReasonID" });
            DropIndex("dbo.Contacts", new[] { "contactTypeID" });
            DropIndex("dbo.Contacts", new[] { "salutationID" });
            DropIndex("dbo.AuditEventDataRows", new[] { "idAuditEvent" });
            DropIndex("dbo.AuditEvents", new[] { "DeletedReasonID" });
            DropIndex("dbo.AuditEvents", new[] { "idAuditEventDescription" });
            DropIndex("dbo.Children", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Children", new[] { "nationalityID" });
            DropIndex("dbo.Children", new[] { "countryID" });
            DropIndex("dbo.Children", new[] { "skinColourID" });
            DropIndex("dbo.Children", new[] { "genderID" });
            DropIndex("dbo.Applicants", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Applicants", new[] { "salutationID" });
            DropIndex("dbo.Respondents", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Respondents", new[] { "nationalityID" });
            DropIndex("dbo.Respondents", new[] { "countryID" });
            DropIndex("dbo.Respondents", new[] { "skinColourID" });
            DropIndex("dbo.Respondents", new[] { "childRelationshipID" });
            DropIndex("dbo.Respondents", new[] { "genderID" });
            DropIndex("dbo.TipstaffPoliceForces", new[] { "policeForceID" });
            DropIndex("dbo.TipstaffPoliceForces", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Solicitors", new[] { "salutationID" });
            DropIndex("dbo.Solicitors", new[] { "solicitorFirmID" });
            DropIndex("dbo.TipstaffRecordSolicitors", new[] { "solicitorID" });
            DropIndex("dbo.TipstaffRecordSolicitors", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Documents", new[] { "tipstaffRecordID" });
            DropIndex("dbo.Documents", new[] { "templateID" });
            DropIndex("dbo.Documents", new[] { "documentTypeID" });
            DropIndex("dbo.Documents", new[] { "documentStatusID" });
            DropIndex("dbo.Documents", new[] { "nationalityID" });
            DropIndex("dbo.Documents", new[] { "countryID" });
            DropIndex("dbo.CaseReviews", new[] { "tipstaffRecordID" });
            DropIndex("dbo.CaseReviews", new[] { "caseReviewStatusID" });
            DropIndex("dbo.AttendanceNotes", new[] { "tipstaffRecordID" });
            DropIndex("dbo.AttendanceNotes", new[] { "AttendanceNoteCodeID" });
            DropIndex("dbo.TipstaffRecords", new[] { "divisionID" });
            DropIndex("dbo.TipstaffRecords", new[] { "caOrderTypeID" });
            DropIndex("dbo.TipstaffRecords", new[] { "caseStatusID" });
            DropIndex("dbo.TipstaffRecords", new[] { "resultID" });
            DropIndex("dbo.TipstaffRecords", new[] { "protectiveMarkingID" });
            DropIndex("dbo.Addresses", new[] { "tipstaffRecordID" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.FaxCodes");
            DropTable("dbo.FAQs");
            DropTable("dbo.DeletedTipstaffRecords");
            DropTable("dbo.ContactTypes");
            DropTable("dbo.Contacts");
            DropTable("dbo.DeletedReasons");
            DropTable("dbo.AuditEventDataRows");
            DropTable("dbo.AuditEvents");
            DropTable("dbo.AuditEventDescriptions");
            DropTable("dbo.Divisions");
            DropTable("dbo.Children");
            DropTable("dbo.CAOrderTypes");
            DropTable("dbo.Applicants");
            DropTable("dbo.Results");
            DropTable("dbo.SkinColours");
            DropTable("dbo.Genders");
            DropTable("dbo.ChildRelationships");
            DropTable("dbo.Respondents");
            DropTable("dbo.ProtectiveMarkings");
            DropTable("dbo.PoliceForces");
            DropTable("dbo.TipstaffPoliceForces");
            DropTable("dbo.SolicitorFirms");
            DropTable("dbo.Salutations");
            DropTable("dbo.Solicitors");
            DropTable("dbo.TipstaffRecordSolicitors");
            DropTable("dbo.Templates");
            DropTable("dbo.Nationalities");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentStatus");
            DropTable("dbo.Countries");
            DropTable("dbo.Documents");
            DropTable("dbo.CaseStatus");
            DropTable("dbo.CaseReviewStatus");
            DropTable("dbo.CaseReviews");
            DropTable("dbo.AttendanceNoteCodes");
            DropTable("dbo.AttendanceNotes");
            DropTable("dbo.TipstaffRecords");
            DropTable("dbo.Addresses");
        }
    }
}
