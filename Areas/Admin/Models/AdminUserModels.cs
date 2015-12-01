using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using PagedList;
namespace Tipstaff.Models
{

    public class DetailsViewModel
    {
        #region StatusEnum enum
        public enum StatusEnum
        {
            Offline,
            Online,
            LockedOut,
            Disabled
        }

        #endregion

        public string DisplayName { get; set; }
        public StatusEnum Status { get; set; }
        public MembershipUser User { get; set; }
        public IDictionary<string, bool> Roles { get; set; }
        public bool IsRolesEnabled { get; set; }
    }

    public class IndexViewModel
    {
        public IPagedList<MembershipUser> Users { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool IsRolesEnabled { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}