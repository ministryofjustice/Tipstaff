using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tipstaff.Models
{
    #region Contact Models
    public class Contact
    {
        [Key]
        public string contactID { get; set; }
        ////[Required, Display(Name = "Title")]
        ////public int salutationID { get; set; }
        [MaxLength(50), Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required, MaxLength(50), Display(Name = "Surname")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "The first line of the address is mandatory"), MaxLength(100), Display(Name = "Address Line 1")]
        public string addressLine1 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 2")]
        public string addressLine2 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 3")]
        public string addressLine3 { get; set; }
        [MaxLength(100), Display(Name = "Town")]
        public string town { get; set; }
        [MaxLength(100), Display(Name = "County")]
        public string county { get; set; }
        [MaxLength(10), Display(Name = "Postcode")]
        [Required]
        public string postcode { get; set; }
        [MaxLength(50), Display(Name = "Full DX address")]
        public string DX { get; set; }
        [MaxLength(20), Display(Name = "Home Phone")]
        public string phoneHome { get; set; }
        [MaxLength(20), Display(Name = "Mobile Phone")]
        public string phoneMobile { get; set; }
        [MaxLength(60), Display(Name = "e-mail address")]
        public string email { get; set; }
        [MaxLength(2000), Display(Name = "Notes")]
        public string notes { get; set; }
        ////[Display(Name = "Contact Type")]
        ////public int contactTypeID { get; set; }
        //[Display(Name = "Contact Type")]
        //public virtual ContactType contactType { get; set; }
        //public virtual Salutation salutation { get; set; }
        [Display(Name = "Contact Type")]
        public MemoryCollections.ContactType contactType { get; set; }
        public MemoryCollections.Salutation salutation { get; set; }


        //[Display(Name = "Full name of Child")]
        public  string PoliceDisplayName
        {
            get
            {
                return string.Format("{0}, {1} {2}", lastName.ToUpper(), salutation?.Detail ?? "", firstName).Replace("  ", " ");
            }
        }
        [Display(Name = "Name")]
        public virtual string fullName
        {
            get
            {
                return string.Join(" ", salutation?.Detail ?? "", firstName, lastName);
            }
        }

        [NotMapped, Display(Name = "Address")]
        public string MultiLineAddress
        {
            get
            {
                List<string> address = new List<string>();

                address.Add(addressLine1);
                if (addressLine2 != null) { address.Add(addressLine2); }
                if (addressLine3 != null) { address.Add(addressLine3); }
                if (town != null) { address.Add(town); }
                if (county != null) { address.Add(county); }
                if (postcode != null) { address.Add(postcode); }

                string output = string.Join("<br />", address);

                return output;
            }
        }

    }
    //public class ContactType
    //{
    //    [Key]
    //    public int contactTypeID { get; set; }
    //    [Required, MaxLength(50), Display(Name = "Contact Type")]
    //    public string Detail { get; set; }
    //    [ScaffoldColumn(false)]
    //    public bool active { get; set; }
    //    [ScaffoldColumn(false)]
    //    public DateTime? deactivated { get; set; }
    //    [ScaffoldColumn(false), MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    #endregion

    public class ContactListView : ListViewModel
    {
        public IPagedList<Contact> Contacts { get; set; }
        public string NameContains { get; set; }
        public SelectList ContactTypeList { get; set; }
        public int ContactTypeID { get; set; }
        public ContactListView()
        {
            ContactTypeID = -1;
            //ContactTypeList = new SelectList(myDBContextHelper.CurrentContext.ContactTypes.Where(x => x.active == true).OrderBy(x=>x.Detail).ToList(), "contactTypeID", "Detail");
            ContactTypeList = new SelectList(MemoryCollections.ContactTypeList.GetContactTypeList().Where(x => x.Active == 1).OrderBy(x => x.Detail).ToList(), "ContactTypeID", "Detail");
        }
    }

    public class ContactModification
    {
        public Contact contact { get; set; }
        public SelectList SalutationList { get; set; }
        public SelectList ContactTypeList { get; set; }

        public ContactModification()
        {
            //SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x => x.active == true).OrderBy(x => x.Detail).ToList(), "salutationID", "Detail");
            //ContactTypeList = new SelectList(myDBContextHelper.CurrentContext.ContactTypes.Where(x => x.active == true).OrderBy(x => x.Detail).ToList(), "contactTypeID", "Detail");
            ContactTypeList = new SelectList(MemoryCollections.ContactTypeList.GetContactTypeList().Where(x => x.Active == 1).OrderBy(x => x.Detail).ToList(), "ContactTypeID", "Detail");
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");
        }

        ////public ContactModification()
        ////{
        ////   ////// contact = myDBContextHelper.CurrentContext.Contacts.Find(id);
        ////    //SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x => x.active == true).ToList().OrderBy(x => x.Detail), "salutationID", "Detail");
        ////    //ContactTypeList = new SelectList(myDBContextHelper.CurrentContext.ContactTypes.Where(x => x.active == true).ToList().OrderBy(x => x.Detail), "contactTypeID", "Detail");
        ////    ContactTypeList = new SelectList(MemoryCollections.ContactTypeList.GetContactTypeList().Where(x => x.Active == 1).OrderBy(x => x.Detail).ToList(), "ContactTypeID", "Detail");
        ////    SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");
        ////}
    }
}