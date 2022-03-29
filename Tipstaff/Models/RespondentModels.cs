using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.ComponentModel;

namespace Tipstaff.Models
{
    public class Respondent
    {
        [Key]
        public int respondentID { get; set; }

        [Required, MaxLength(50), Display(Name = "Last name")]
        public string nameLast { get; set; }

        [Required, MaxLength(50), Display(Name = "First name")]
        public string nameFirst { get; set; }

        [MaxLength(50), Display(Name = "Middle name(s)"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string nameMiddle { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true),
            Display(Name = "Date of Birth"), PastDateorNull(ErrorMessage = "Birth date cannot be in the future")]
        public DateTime? dateOfBirth { get; set; }

        [Required, Display(Name = "Gender")]
        public int genderID { get; set; }

        [Required, Display(Name="Relationship to child")]
        public int? childRelationshipID { get; set; }

        [MaxLength(50), Display(Name = "Hair colour"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string hairColour { get; set; } = "N/K";

        [MaxLength(50), Display(Name = "Eye colour"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string eyeColour { get; set; }

        [Required, Display(Name = "Skin colour")]
        public int skinColourID { get; set; }

        [MaxLength(50), Display(Name = "Height"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string height { get; set; }

        [MaxLength(50), Display(Name = "Build"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string build { get; set; }

        [MaxLength(250), Display(Name = "Special Features"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string specialfeatures { get; set; }

        [Required, Display(Name = "Country of Origin")]
        public int countryID { get; set; }

        [Required, Display(Name = "Nationality")]
        public int? nationalityID { get; set; }

        [MaxLength(100), Display(Name = "Risk of Violence?"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string riskOfViolence { get; set; }

        [MaxLength(100), Display(Name="Risk of Drugs?"), DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/K")]
        public string riskOfDrugs { get; set; }

        [Required]
        public int tipstaffRecordID  { get; set; }
        
        public string PNCID { get; set; }
        
        [Display(Name ="Gender")]
        public virtual Gender gender { get; set; }
        [Display(Name = "Country of Origin")]
        public virtual Country country { get; set; }
        [Display(Name = "Nationality")]
        public virtual Nationality nationality { get; set; }
        [Display(Name = "Skin colour")]
        public virtual SkinColour SkinColour { get; set; }
        //public virtual ChildAbduction childAbduction { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }
        public virtual ChildRelationship childRelationship { get; set; }

        [Display(Name = "Full name of respondent")]
        public virtual string fullname
        {
            get
            {
                return string.Format("{0} {1} {2}", nameFirst, nameMiddle, nameLast).Replace("  ", " ");
            }
        }
        [Display(Name = "Full name of respondent")]
        public virtual string PoliceDisplayName
        {
            get
            {
                return string.Format("{0}, {1} {2}", nameLast.ToUpper(), nameFirst, nameMiddle).Replace("  ", " ");
            }
        }
        [Display(Name = "Date of Birth")]
        public virtual string DateofBirthDisplay
        {
            get
            {
                if ((this.dateOfBirth.Equals(new DateTime(1901,1,1))) || (this.dateOfBirth==null)) 
                {
                    return "Unknown";
                } 
                else
                {
                    return ((DateTime)this.dateOfBirth).ToShortDateString();
                }
            }
        }

        [Display(Name = "Age")]
        public virtual string Age
        {
            get
            {
                if ((this.dateOfBirth.Equals(new DateTime(1901,1,1))) || (this.dateOfBirth==null)) {
                    return "Unknown";
                }
                int now = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
                int dob = int.Parse(((DateTime)dateOfBirth).ToString("yyyyMMdd"));
                string dif = (now - dob).ToString();
                int age = 0;
                if (dif.Length > 4)
                    age = int.Parse(dif.Substring(0, dif.Length - 4));
                return age.ToString();
            }
        }

        public virtual string xmlBlock
        {
            get
            {
                //const string xmlString = "<w:tbl><w:tblPr><w:tblStyle w:val='TableGrid'/><w:tblW w:w='0' w:type='auto'/><w:tblLook w:val='01E0'/></w:tblPr><w:tblGrid><w:gridCol w:w='2448'/><w:gridCol w:w='2520'/><w:gridCol w:w='1674'/><w:gridCol w:w='2214'/></w:tblGrid><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00D347D1'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='8856' w:type='dxa'/><w:gridSpan w:val='4'/><w:tcBorders><w:top w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r wsp:rsidRPr='00583929'><w:t>Known Risks</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00D347D1'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r wsp:rsidRPr='00583929'><w:t>Violence</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='001E0976' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{0}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00D347D1'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r wsp:rsidRPr='00583929'><w:t>Drugs</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{1}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00D347D1'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r wsp:rsidRPr='00583929'><w:t>Name</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{2}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Relationship to child</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{3}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Date of Birth</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{4}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Age</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{5}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Hair colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{6}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Eye Colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{7}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Skin colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{8}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Height</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{9}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Build</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{10}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:r><w:t>Nationality</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:r><w:t>{11}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00583929'><w:trPr><w:trHeight w:val='1114'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00583929' wsp:rsidP='00583929'><w:pPr><w:spacing w:before='60'/></w:pPr><w:r><w:t>Special features</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='004837DC' wsp:rsidP='00583929'><w:pPr><w:spacing w:before='60'/></w:pPr><w:r><w:t>{12}</w:t></w:r></w:p></w:tc></w:tr></w:tbl>";
                const string xmlString = "<w:tbl><w:tblPr><w:tblW w:w='0' w:type='auto'/><w:tblLook w:val='01E0'/></w:tblPr><w:tblGrid><w:gridCol w:w='2448'/><w:gridCol w:w='2520'/><w:gridCol w:w='1674'/><w:gridCol w:w='2214'/></w:tblGrid><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r wsp:rsidRPr='00583929'><w:t>Name</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{2}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Relationship to child</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{3}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Date of Birth</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{4}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Age</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{5}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Hair </w:t></w:r><w:proofErr w:type='spellStart'/><w:r><w:t>colour</w:t></w:r><w:proofErr w:type='spellEnd'/></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{6}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Eye </w:t></w:r><w:proofErr w:type='spellStart'/><w:r><w:t>Colour</w:t></w:r><w:proofErr w:type='spellEnd'/></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{7}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Skin </w:t></w:r><w:proofErr w:type='spellStart'/><w:r><w:t>colour</w:t></w:r><w:proofErr w:type='spellEnd'/></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{8}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Height</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2520' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{9}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1674' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Build</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2214' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{10}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>Nationality</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:r><w:t>{11}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidTr='00CF1AB3'><w:trPr><w:trHeight w:val='1114'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:pPr><w:spacing w:before='60'/></w:pPr><w:r><w:t>Special features</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='4' wx:bdrwidth='10' w:space='0' w:color='auto'/></w:tcBorders></w:tcPr><w:p wsp:rsidR='00583929' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='00583929'><w:pPr><w:spacing w:before='60'/></w:pPr><w:r><w:t>{12}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidTr='001A1CEE'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='8856' w:type='dxa'/><w:gridSpan w:val='4'/><w:tcBorders><w:top w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='001A1CEE'><w:pPr><w:rPr><w:lang w:val='EN-GB'/></w:rPr></w:pPr><w:r wsp:rsidRPr='00583929'><w:t>Known Risks</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidTr='001A1CEE'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='001A1CEE'><w:r wsp:rsidRPr='00583929'><w:t>Violence</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00CF1AB3' wsp:rsidRPr='001E0976' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='001A1CEE'><w:r><w:t>{0}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidTr='001A1CEE'><w:trPr><w:trHeight w:val='425'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='2448' w:type='dxa'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/></w:tcBorders><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='001A1CEE'><w:r wsp:rsidRPr='00583929'><w:t>Drugs</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='6408' w:type='dxa'/><w:gridSpan w:val='3'/><w:tcBorders><w:top w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:left w:val='single' w:sz='2' wx:bdrwidth='5' w:space='0' w:color='auto'/><w:bottom w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/><w:right w:val='single' w:sz='18' wx:bdrwidth='45' w:space='0' w:color='auto'/></w:tcBorders><w:vAlign w:val='center'/></w:tcPr><w:p wsp:rsidR='00CF1AB3' wsp:rsidRPr='00583929' wsp:rsidRDefault='00CF1AB3' wsp:rsidP='001A1CEE'><w:r><w:t>{1}</w:t></w:r></w:p></w:tc></w:tr></w:tbl>";
                string xmlOutput = string.Format(xmlString,
                                                    SecurityElement.Escape(riskOfViolence),
                                                    SecurityElement.Escape(riskOfDrugs),
                                                    SecurityElement.Escape(PoliceDisplayName),
                                                    SecurityElement.Escape(childRelationship.Detail),
                                                    SecurityElement.Escape(DateofBirthDisplay),
                                                    SecurityElement.Escape(Age),
                                                    SecurityElement.Escape(hairColour),
                                                    SecurityElement.Escape(eyeColour),
                                                    SecurityElement.Escape(SkinColour.Detail),
                                                    SecurityElement.Escape(height),
                                                    SecurityElement.Escape(build),
                                                    SecurityElement.Escape(country.Detail),
                                                    SecurityElement.Escape(specialfeatures));
                return xmlOutput;
            }
        }
    }

    public class RespondentCreationModel
    {
        public int tipstaffRecordID { get; set; }
        public Respondent respondent { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList GenderList { get; set; }
        public SelectList RelationToChildList { get; set; }
        public SelectList NationalityList { get; set; }
        public SelectList SkinColourList { get; set; }
        public TipstaffRecord tipstaffRecord { get; set; }
        public bool initial { get; set; }
        public RespondentCreationModel()
        {
            GenderList = new SelectList(myDBContextHelper.CurrentContext.Genders.Where(x => x.active == true).ToList(), "genderID", "Detail");
            CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
            RelationToChildList = new SelectList(myDBContextHelper.CurrentContext.ChildRelationships.Where(x => x.active == true).ToList(), "childRelationshipID", "Detail");
            SkinColourList = new SelectList(myDBContextHelper.CurrentContext.SkinColours.Where(x => x.active == true).ToList(), "skinColourID", "Detail");
        }
        public RespondentCreationModel(int id)
        {
            tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);
            tipstaffRecordID = id;
            GenderList = new SelectList(myDBContextHelper.CurrentContext.Genders.Where(x => x.active == true).ToList(), "genderID", "Detail");
            CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
            RelationToChildList = new SelectList(myDBContextHelper.CurrentContext.ChildRelationships.Where(x => x.active == true).ToList(), "childRelationshipID", "Detail");
            SkinColourList = new SelectList(myDBContextHelper.CurrentContext.SkinColours.Where(x => x.active == true).ToList(), "skinColourID", "Detail");
        }
    }
    public class ListRespondentsByTipstaffRecord:IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Respondent> Respondents { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }
}