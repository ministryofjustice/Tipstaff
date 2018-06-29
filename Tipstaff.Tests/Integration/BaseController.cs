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

        //Repositories
        protected ICaseReviewRepository _caseReviewRepository;
        protected ITipstaffRecordRepository _tipstaffRecordRepository;
        protected IAddressRepository _addressRepository;
        protected IAttendanceNotesRepository _attendanceNotesRepository;
        protected IRespondentRepository _respondentRepository;
        protected IDeletedTipstaffRecordRepository _deleteTipstaffRecordRepository;
        
        [SetUp]
        public void SetUp()
        {
            //Repositories
            _caseReviewRepository = new CaseReviewRepository(new DynamoAPI<CaseReview>());
            _tipstaffRecordRepository = new TipstaffRecordRepository(new DynamoAPI<TipstaffRecord>());
            _addressRepository = new AddressRepository(new DynamoAPI<Address>());
            _attendanceNotesRepository = new AttendanceNotesRepository(new DynamoAPI<AttendanceNote>());
            _respondentRepository = new RespondentRepository(new DynamoAPI<Respondent>());
            _deleteTipstaffRecordRepository = new DeletedTipstaffRecordRepository(new DynamoAPI<DeletedTipstaffRecord>());

            //Presenters
            _caseReviewPresenter = new CaseReviewPresenter(_caseReviewRepository);
            _addressPresenter = new AddressPresenter(_addressRepository);
            _attendanceNotePresenter = new AttendanceNotePresenter(_attendanceNotesRepository, _tipstaffRecordPresenter);
            // _respondentPresenter = new RespondentPresenter(_tipstaffRecordPresenter, _respondentRepository);
            // _tipstaffRecordPresenter = new TipstaffRecordPresenter(_tipstaffRecordRepository);
            // _childAbductionPresenter = new ChildAbductionPresenter(_tipstaffRecordRepository, _deleteTipstaffRecordRepository);
            _warrantPresenter = new WarrantPresenter(_tipstaffRecordRepository, _addressPresenter, _caseReviewPresenter, _respondentPresenter);
        }
        
    }
}
