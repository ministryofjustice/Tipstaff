using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Presenters;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Tests.Integration
{
    public abstract class BaseController
    {
        //Presenters
        protected ITipstaffRecordPresenter _tipstaffRecordPresenter;
        protected ICaseReviewPresenter _caseReviewPresenter;
        protected IAddressPresenter _addressPresenter;
        protected IAttendanceNotePresenter _attendanceNotePresenter;
        protected IRespondentPresenter _respondentPresenter;
        protected IChildAbductionPresenter _childAbductionPresenter;
        protected IWarrantPresenter _warrantPresenter;
        protected IChildPresenter _childPresenter;
        protected IApplicantPresenter _applicantPresenter;
        protected ISolicitorPresenter _solicitorPresenter;
        protected ITemplatePresenter _templatePresenter;
        protected IDocumentPresenter _docPresenter;

        //Repositories
        protected ICaseReviewRepository _caseReviewRepository;
        protected ITipstaffRecordRepository _tipstaffRecordRepository;
        protected IAddressRepository _addressRepository;
        protected IAttendanceNotesRepository _attendanceNotesRepository;
        protected IRespondentRepository _respondentRepository;
        protected IDeletedTipstaffRecordRepository _deleteTipstaffRecordRepository;
        protected IChildRepository _childRepository;
        protected IApplicantRepository _applicantRepository;
        protected ISolicitorRepository _solicitorRepository;
        protected ITemplateRepository _templateRepository;
        protected IDocumentsRepository _docRepository;

        //protected ITipstaffRecordPresenter


        
        
       // [SetUp]
        public BaseController()
        {
            //Repositories
            _caseReviewRepository = new CaseReviewRepository(new DynamoAPI<CaseReview>());
            _tipstaffRecordRepository = new TipstaffRecordRepository(new DynamoAPI<TipstaffRecord>());
            _addressRepository = new AddressRepository(new DynamoAPI<Address>());
            _attendanceNotesRepository = new AttendanceNotesRepository(new DynamoAPI<AttendanceNote>());
            _respondentRepository = new RespondentRepository(new DynamoAPI<Respondent>());
            _deleteTipstaffRecordRepository = new DeletedTipstaffRecordRepository(new DynamoAPI<DeletedTipstaffRecord>());
            _childRepository = new ChildRepository(new DynamoAPI<Child>());
            _applicantRepository = new ApplicantRepository(new DynamoAPI<Applicant>());
            _solicitorRepository = new SolicitorRepository(new DynamoAPI<Solicitor>());
            _templateRepository = new TemplateRepository(new DynamoAPI<Template>());
            _docRepository = new DocumentsRepository(new DynamoAPI<Document>());

            //Presenters
            _addressPresenter = new AddressPresenter(_addressRepository);
            _respondentPresenter = new RespondentPresenter(_respondentRepository);
            
            _caseReviewPresenter = new CaseReviewPresenter(_caseReviewRepository);
            _tipstaffRecordPresenter = new TipstaffRecordPresenter(_tipstaffRecordRepository, _respondentPresenter, _caseReviewPresenter, _addressPresenter);
            _childPresenter = new ChildPresenter(_childRepository, _tipstaffRecordRepository, _tipstaffRecordPresenter);
            _attendanceNotePresenter = new AttendanceNotePresenter(_attendanceNotesRepository, _tipstaffRecordPresenter);
            _solicitorPresenter = new SolicitorPresenter(_solicitorRepository);
            _applicantPresenter = new ApplicantPresenter(_applicantRepository, _tipstaffRecordPresenter);
             _childAbductionPresenter = new ChildAbductionPresenter(_tipstaffRecordRepository, 
                                                                    _deleteTipstaffRecordRepository, 
                                                                    _caseReviewPresenter, 
                                                                    _respondentPresenter, 
                                                                    _childPresenter, 
                                                                    _addressPresenter, 
                                                                    _applicantPresenter,
                                                                    _solicitorPresenter, _attendanceNotePresenter);

            _warrantPresenter = new WarrantPresenter(_tipstaffRecordRepository, _addressPresenter, _caseReviewPresenter, _respondentPresenter, _attendanceNotePresenter);
            _templatePresenter = new TemplatePresenter(_templateRepository, _solicitorPresenter);
            _docPresenter = new DocumentPresenter(_docRepository, _solicitorPresenter);
        }
        
    }
}
