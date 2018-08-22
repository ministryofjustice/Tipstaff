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
using TPLibrary.GuidGenerator;

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
        protected ITipstaffPoliceForcesPresenter _tipstaffPolicePresenter;
        protected IPoliceForcesPresenter _policePresenter;

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
        protected IAuditEventRepository _auditRepo;
        protected ISolicitorFirmRepository _solicitorFirmRepository;
        protected ITipstaffRecordSolicitorsRepository _tipstaffRecordSolicitorRepository;
        protected ITipstaffPoliceForcesRepository _tipstaffPoliceRepository;
        protected IPoliceForcesRepository _policeRepository;
        private ICacheRepository _cacheRepository;
        
        
        public BaseController()
        {
            //Repositories
            _auditRepo = new AuditEventRepository(new DynamoAPI<AuditEvent>(), new GuidGenerator());
            _caseReviewRepository = new CaseReviewRepository(new DynamoAPI<CaseReview>(), _auditRepo);
            _tipstaffRecordRepository = new TipstaffRecordRepository(new DynamoAPI<TipstaffRecord>(), _auditRepo);
            _addressRepository = new AddressRepository(new DynamoAPI<Address>(), _auditRepo);
            _attendanceNotesRepository = new AttendanceNotesRepository(new DynamoAPI<AttendanceNote>(), _auditRepo);
            _respondentRepository = new RespondentRepository(new DynamoAPI<Respondent>(), _auditRepo);
            _deleteTipstaffRecordRepository = new DeletedTipstaffRecordRepository(new DynamoAPI<DeletedTipstaffRecord>(), _auditRepo);
            _childRepository = new ChildRepository(new DynamoAPI<Child>(), _auditRepo);
            _applicantRepository = new ApplicantRepository(new DynamoAPI<Applicant>(), _auditRepo);
            _templateRepository = new TemplateRepository(new DynamoAPI<Template>(), _auditRepo);
            _docRepository = new DocumentsRepository(new DynamoAPI<Document>(), _auditRepo);
            _solicitorFirmRepository = new SolicitorFirmRepository(new DynamoAPI<SolicitorFirm>(), _auditRepo);
            _solicitorRepository = new SolicitorRepository(new DynamoAPI<Solicitor>(), _auditRepo);
            _tipstaffRecordSolicitorRepository = new TipstaffRecordSolicitorsRepository(new DynamoAPI<Tipstaff_Solicitors>());
             _tipstaffPoliceRepository = new TipstaffPoliceForcesRepository(new DynamoAPI<Tipstaff_PoliceForces>(), _auditRepo);
            _policeRepository = new PoliceForcesRepository(new DynamoAPI<PoliceForces>(), _auditRepo);
            _cacheRepository = new CacheRepository(new DynamoAPI<CacheStore>());

            //Presenters
            _addressPresenter = new AddressPresenter(_addressRepository);
            _respondentPresenter = new RespondentPresenter(_respondentRepository);
            _docPresenter = new DocumentPresenter(_docRepository);
            _templatePresenter = new TemplatePresenter(_templateRepository);
            _caseReviewPresenter = new CaseReviewPresenter(_caseReviewRepository);
            _solicitorPresenter = new SolicitorPresenter(_solicitorRepository, _solicitorFirmRepository, _tipstaffRecordSolicitorRepository);
            _attendanceNotePresenter = new AttendanceNotePresenter(_attendanceNotesRepository,
                                                                   _tipstaffRecordPresenter);
            _applicantPresenter = new ApplicantPresenter(_applicantRepository, 
                                                        _tipstaffRecordPresenter);
            _childPresenter = new ChildPresenter(_childRepository, _tipstaffRecordPresenter);
            _policePresenter = new PoliceForcesPresenter(_policeRepository);
            _tipstaffPolicePresenter = new TipstaffPoliceForcesPresenter(_tipstaffPoliceRepository, _policePresenter);                              
                                                 
            _tipstaffRecordPresenter = new TipstaffRecordPresenter(_tipstaffRecordRepository, 
                                                                   _respondentPresenter, 
                                                                   _caseReviewPresenter, 
                                                                   _addressPresenter, 
                                                                   _solicitorPresenter, _cacheRepository);
             _childAbductionPresenter = new ChildAbductionPresenter(_tipstaffRecordRepository, 
                                                                    _deleteTipstaffRecordRepository, 
                                                                    _caseReviewPresenter, 
                                                                    _respondentPresenter, 
                                                                    _childPresenter, 
                                                                    _addressPresenter, 
                                                                    _applicantPresenter,
                                                                    _solicitorPresenter, 
                                                                    _attendanceNotePresenter , _docPresenter, _cacheRepository);
            

            _warrantPresenter = new WarrantPresenter(_tipstaffRecordRepository, _addressPresenter, _caseReviewPresenter, _respondentPresenter, _attendanceNotePresenter, _docPresenter,_tipstaffPolicePresenter, _solicitorPresenter, _cacheRepository);

        }
        
    }
}
