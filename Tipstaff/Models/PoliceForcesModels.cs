using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tipstaff.Models
{
    public class PoliceForces:IModel
    {
        [Key]
        public string policeForceID { get; set; }

        public bool loggedInUser { get; set; }

        [Required, MaxLength(255), Display(Name = "Police Force Name")]

        public string policeForceName { get; set; }
        [Required, MaxLength(255), Display(Name = "Police Force Email"),DataType(DataType.EmailAddress)]

        public string policeForceEmail { get; set; }

        public bool active { get; set; }
        [Display(Name = "Deactivated On")]

        public DateTime? deactivated { get; set; }
        [MaxLength(50), Display(Name = "Deactivated By")]

        public string deactivatedBy { get; set; }

        
    }

    public class TipstaffPoliceForce
    {
        [Key]
        public int tipstaffRecordPoliceForceID { get; set; }
        public int tipstaffRecordID { get; set; }
        public int policeForceID  { get; set; }
        

        public virtual TipstaffRecord tipstaffRecord { get; set; }
        public virtual PoliceForces policeForces { get; set; }
    }

    public class PoliceForceCreation
    {
        public TipstaffPoliceForce TS_PoliceForce { get; set; }
        public SelectList PoliceForceList { get; set; }
        [Required, Display(Name="Police Force")]
        public int policeForceID { get; set; }
        public PoliceForceCreation()
        {
            TS_PoliceForce = new TipstaffPoliceForce();
            PoliceForceList = new SelectList(myDBContextHelper.CurrentContext.PoliceForces.Where(x => x.active == true).OrderBy(x => x.policeForceName).ToList(), "policeForceID", "policeForceName");
        }
    }

    public class ListPoliceForcesByTipstaffRecord : IListByTipstaffRecord
    {
        public string tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<TipstaffPoliceForce> PoliceForces { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }
}