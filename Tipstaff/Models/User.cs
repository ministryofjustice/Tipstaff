using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tipstaff.Models
{
    
    public class User
    {
        [Key]
        public string UserID { get; set; }

        [Required, MaxLength(150), Display(Name = "Login name")]
        public string Name { get; set; }

        [Required, MaxLength(30), Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [AdditionalMetadata("IgnoreAudit", true), Display(Name = "Last active")]

        public DateTime? LastActive { get; set; }

        [Required, Display(Name = "Role")]
        public int RoleStrength { get; set; }
        
        public MemoryCollections.Role Role { get; set; }
    }
}