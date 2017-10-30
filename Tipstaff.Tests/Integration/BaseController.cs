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
            _caseReviewRepository = new CaseReviewRepository(new Infrastructure.DynamoAPI.DynamoAPI<CaseReview>());
            _tipstaffRecordRepository = new TipstaffRecordRepository(new Infrastructure.DynamoAPI.DynamoAPI<TipstaffRecord>());
            _addressRepository = new AddressRepository(new Infrastructure.DynamoAPI.DynamoAPI<Address>());
            _attendanceNotesRepository = new AttendanceNotesRepository(new Infrastructure.DynamoAPI.DynamoAPI<AttendanceNote>());
            _respondentRepository = new RespondentRepository(new Infrastructure.DynamoAPI.DynamoAPI<Respondent>());
            _deleteTipstaffRecordRepository = new DeletedTipstaffRecordRepository(new Infrastructure.DynamoAPI.DynamoAPI<DeletedTipstaffRecord>());

            //Presenters
            _caseReviewPresenter = new CaseReviewPresenter(_caseReviewRepository);
            _addressPresenter = new AddressPresenter(_addressRepository, _tipstaffRecordPresenter);
            _attendanceNotePresenter = new AttendanceNotePresenter(_attendanceNotesRepository, _tipstaffRecordPresenter);
            _respondentPresenter = new RespondentPresenter(_tipstaffRecordPresenter, _respondentRepository);
            _tipstaffRecordPresenter = new TipstaffRecordPresenter(_tipstaffRecordRepository, _addressPresenter, _attendanceNotePresenter, _respondentPresenter, _caseReviewPresenter);
            _childAbductionPresenter = new ChildAbductionPresenter(_tipstaffRecordRepository, _deleteTipstaffRecordRepository);
        }
        
    }
}
