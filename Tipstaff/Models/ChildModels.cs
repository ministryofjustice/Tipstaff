using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Security;

namespace Tipstaff.Models
{
    public class Child : IModel
    {
        [Key]
        //public int childID { get; set; }
        public string childID { get; set; }
        [Required, MaxLength(50), Display(Name = "Last name")]
        public string nameLast { get; set; }
        [MaxLength(50), Display(Name = "First name")]
        public string nameFirst { get; set; }
        [MaxLength(50), Display(Name = "Middle name(s)")]
        public string nameMiddle { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth"),PastDateorNull(ErrorMessage="Birth date cannot be in the future")]
        public DateTime? dateOfBirth { get; set; }
        [Required,Display(Name="Height")]
        public string height { get; set; }
        [Required, Display(Name = "Build")]
        public string build { get; set; }
        [Required, Display(Name = "Hair colour")]
        public string hairColour { get; set; }
        [Required, Display(Name = "Eye colour")]
        public string eyeColour { get; set; }
        [Required, Display(Name = "Special Features")]
        public string specialfeatures { get; set; }
        [Required]
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }

        public string PNCID { get; set; }

        //public virtual Gender gender { get; set; }
        //[Display(Name = "Skin colour")]
        //public virtual SkinColour SkinColour { get; set; }
        //[Display(Name = "Country of Origin")]
        //public virtual Country country { get; set; }
        //[Display(Name = "Nationality")]
        //public virtual Nationality nationality { get; set; }
        
        
        [Required, Display(Name = "Gender")]
        public MemoryCollections.Gender gender { get; set; }
        [Required, Display(Name = "Skin colour")]
        public MemoryCollections.SkinColour skinColour { get; set; }
        [Required, Display(Name = "Country of Origin")]
        public MemoryCollections.Country country { get; set; }
        [Required, Display(Name = "Nationality")]
        public MemoryCollections.Nationality nationality { get; set; }



        public ChildAbduction childAbduction { get; set; }
        //public virtual TipstaffRecord tipstaffRecord { get; set; }
        [Display(Name="Full name of Child")]
        public virtual string fullname
        {
            get
            {
                return string.Format("{0} {1} {2}", nameFirst, nameMiddle, nameLast).Replace("  ", " ");
            }
        }
        [Display(Name="Full name of Child")]
        public virtual string PoliceDisplayName
        {
            get
            {
                return string.Format("{0}, {1} {2}", nameLast.ToUpper(), nameFirst, nameMiddle).Replace("  ", " ");
            }
        }
        [Display(Name = "Age")]
        public virtual string Age
        {
            get
            {
                if ((this.dateOfBirth.Equals(new DateTime(1901, 1, 1))) || (this.dateOfBirth.Equals(new DateTime(0001, 1, 1))) || (this.dateOfBirth == null))
                {
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
        [Display(Name = "Date of Birth")]
        public virtual string DateofBirthDisplay
        {
            get
            {
                if ((this.dateOfBirth.Equals(new DateTime(1901, 1, 1))) || (this.dateOfBirth.Equals(new DateTime(0001, 1, 1))) || (this.dateOfBirth == null))
                {
                    return "Unknown";
                }
                else
                {
                    return ((DateTime)this.dateOfBirth).ToShortDateString();
                }
            }
        }
        public string xmlBlock
        {
            get
            {
                const string xmlstring = "<w:tbl><w:tblPr><w:tblStyle w:val='TableGrid'/><w:tblW w:w='0' w:type='auto'/><w:tblLook w:val='01E0'/></w:tblPr><w:tblGrid><w:gridCol w:w='1715'/><w:gridCol w:w='1337'/><w:gridCol w:w='1268'/><w:gridCol w:w='1650'/><w:gridCol w:w='2886'/></w:tblGrid><w:tr wsp:rsidR='002C5CD3' wsp:rsidRPr='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='8856' w:type='dxa'/><w:gridSpan w:val='5'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRPr='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs><w:rPr><w:b/></w:rPr></w:pPr><w:r wsp:rsidRPr='002C5CD3'><w:rPr><w:b/></w:rPr><w:t>CHILD ||CHILDNUMBER||</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Surname</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{0}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Forenames</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{1} {2}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Date of Birth</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{3}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Height</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{4}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='007652A5'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Age</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='0058043C'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{5}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00977B83'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Build</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{6}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00D83113'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Sex</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DE0A95'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{7}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00CE7D29'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Hair colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{8}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:vmerge w:val='restart'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='001746C1'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Nationality</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:vmerge w:val='restart'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{9}</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Eye colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{10}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:tc><w:tcPr><w:tcW w:w='1715' w:type='dxa'/><w:vmerge/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2605' w:type='dxa'/><w:gridSpan w:val='2'/><w:vmerge/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='1650' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='008116E5'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Skin colour</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='2886' w:type='dxa'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{11}</w:t></w:r></w:p></w:tc></w:tr><w:tr wsp:rsidR='002C5CD3' wsp:rsidTr='002C5CD3'><w:trPr><w:trHeight w:val='564'/></w:trPr><w:tc><w:tcPr><w:tcW w:w='3052' w:type='dxa'/><w:gridSpan w:val='2'/><w:shd w:val='clear' w:color='auto' w:fill='D9D9D9'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='001F1BBC'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>Special features</w:t></w:r></w:p></w:tc><w:tc><w:tcPr><w:tcW w:w='5804' w:type='dxa'/><w:gridSpan w:val='3'/><w:shd w:val='clear' w:color='auto' w:fill='auto'/></w:tcPr><w:p wsp:rsidR='002C5CD3' wsp:rsidRDefault='002C5CD3' wsp:rsidP='00DB2571'><w:pPr><w:tabs><w:tab w:val='right' w:pos='1620'/><w:tab w:val='left' w:pos='1800'/><w:tab w:val='right' w:pos='3600'/><w:tab w:val='left' w:pos='3780'/><w:tab w:val='right' w:pos='5940'/><w:tab w:val='left' w:pos='6120'/></w:tabs></w:pPr><w:r><w:t>{12}</w:t></w:r></w:p></w:tc></w:tr></w:tbl><w:p wsp:rsidR='002367C6' wsp:rsidRDefault='002367C6'><w:pPr><w:rPr><w:rFonts w:cs='Tahoma'/><w:lang w:val='EN-US'/></w:rPr></w:pPr></w:p>";
                string xmlOutput = string.Format(xmlstring,
                                                    SecurityElement.Escape(nameLast.ToUpper()),
                                                    SecurityElement.Escape(nameFirst),
                                                    SecurityElement.Escape(nameMiddle),
                                                    SecurityElement.Escape(DateofBirthDisplay),
                                                    SecurityElement.Escape(height),
                                                    SecurityElement.Escape(Age),
                                                    SecurityElement.Escape(build),
                                                    SecurityElement.Escape(gender.Detail),
                                                    SecurityElement.Escape(hairColour),
                                                    SecurityElement.Escape(country.Detail),
                                                    SecurityElement.Escape(eyeColour),
                                                    SecurityElement.Escape(skinColour.Detail),
                                                    SecurityElement.Escape(specialfeatures));
                return xmlOutput;
            }
        }
    }
    public class ChildCreationModel
    {
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
        public Child child { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList GenderList { get; set; }
        public SelectList NationalityList { get; set; }
        public SelectList SkinColourList { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }
        public bool initial { get; set; }

        public ChildCreationModel()
        {
            GenderList = new SelectList(MemoryCollections.GenderList.GetGenderList().Where(x => x.Active == 1).ToList(), "GenderID", "Detail");
            SkinColourList = new SelectList(MemoryCollections.SkinColourList.GetSkinColourList().Where(x => x.Active == 1).ToList(), "SkinColourId", "Detail");
            CountryList = new SelectList(MemoryCollections.CountryList.GetCountryList().Where(x => x.Active == 1).ToList(), "CountryID", "Detail");
            NationalityList = new SelectList(MemoryCollections.NationalityList.GetNationalityList().Where(x => x.Active == 1).ToList(), "NationalityID", "Detail");
        }
        ////public ChildCreationModel(int id)
        ////{
        ////    tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);

        ////    GenderList = new SelectList(MemoryCollections.GenderList.GetGenderList().Where(x => x.Active == 1).ToList(), "GenderID", "Detail");
        ////    SkinColourList = new SelectList(MemoryCollections.SkinColourList.GetSkinColourList().Where(x => x.Active == 1).ToList(), "SkinColourId", "Detail");
        ////    CountryList = new SelectList(MemoryCollections.CountryList.GetCountryList().Where(x => x.Active == 1).ToList(), "CountryID", "Detail");
        ////    NationalityList = new SelectList(MemoryCollections.NationalityList.GetNationalityList().Where(x => x.Active == 1).ToList(), "NationalityID", "Detail");
        ////    //Note: ChildCreationModel working dropdown validation (partially, no specific message)
        ////}

        ////public ChildCreationModel(string id)
        ////{
        ////    tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);
        ////    tipstaffRecordID = id;

        ////    GenderList = new SelectList(MemoryCollections.GenderList.GetGenderList().Where(x => x.Active == 1));
        ////    CountryList = new SelectList(MemoryCollections.CountryList.GetCountryList().Where(x => x.Active == 1).ToList(), "CountryID", "Detail");
        ////    NationalityList = new SelectList(MemoryCollections.NationalityList.GetNationalityList().Where(x => x.Active == 1).ToList(), "NationalityID", "Detail");
        ////    SkinColourList = new SelectList(MemoryCollections.SkinColourList.GetSkinColourList().Where(x => x.Active == 1), "SkinColourID", "Detail");
        ////    //Note: ChildCreationModel working dropdown validation (partially, no specific message)
        ////}

    }
    public class ListChildrenByTipstaffRecord:IListByTipstaffRecord
    {
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Child> Children { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }
}