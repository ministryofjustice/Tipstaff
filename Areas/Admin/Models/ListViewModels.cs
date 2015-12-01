using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Tipstaff.Models;

namespace Tipstaff.Models
{

    public class AdminListView
    {
        public bool onlyActive { get; set; }
        public int page { get; set; }
        public string detailContains { get; set; }
        public string sortOrder { get; set; }

        public AdminListView()
        {
            onlyActive = true;
            page = 1;
            detailContains = "";
        }
    }

    public class ChildRelationshipListView : AdminListView
    {
        public IPagedList<ChildRelationship> ChildRelationships { get; set; }
    }
    public class ContactTypeListView : AdminListView
    {
        public IPagedList<ContactType> ContactTypes { get; set; }
    }
    public class CountryListView : AdminListView
    {
    public IPagedList<Country> Countries { get; set; }
    }
        public class DocumentStatusListView : AdminListView
    {
    public IPagedList<DocumentStatus> DocumentStatuses { get; set; }
    }
    public class DocumentTypeListView : AdminListView
    {
        public IPagedList<DocumentType> DocumentTypes { get; set; }
    }
    public class GenderListView : AdminListView
    {
        public IPagedList<Gender> Genders { get; set; }
    }
    public class NationalityListView : AdminListView
    {
        public IPagedList<Nationality> Nationalities { get; set; }
    }
    public class SalutationListView : AdminListView
    {
        public IPagedList<Salutation> Salutations { get; set; }
    }
    public class SolicitorListView : AdminListView
    {
        public IPagedList<Solicitor> Solicitors { get; set; }
    }
    public class SolicitorFirmListView : AdminListView
    {
        public IPagedList<SolicitorFirm> SolicitorFirms { get; set; }
    }
    public class DeletionReasonsListView : AdminListView
    {
        public IPagedList<DeletedReason> DeletionReasons { get; set; }
    }
    public class PoliceForcesListView : AdminListView
    {
        public IPagedList<PoliceForce> PoliceForces { get; set; }
    }
}