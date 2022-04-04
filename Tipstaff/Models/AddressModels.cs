using System.Collections;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Security;
using System.Collections.Generic;
namespace Tipstaff.Models
{
    public class ListAddressesByTipstaffRecord : IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Address> Addresses { get; set; }
        //public IPagedList<Address> Addresses { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class AddressCreationModel
    {
        public int tipstaffRecordID { get; set; }
        public TipstaffRecord tipstaffRecord { get; set; }
        public Address address { get; set; }

        public AddressCreationModel() { }

        public AddressCreationModel(int id)
        {
            tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);
            tipstaffRecordID = id;
        }
    }

    public class Address
    {
        [Key]
        public int addressID { get; set; }
        [MaxLength(100), Display(Name = "Name")]
        public string addresseeName { get; set; }
        [Required, MaxLength(100), Display(Name = "Address Line 1")]
        public string addressLine1 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 2")]
        public string addressLine2 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 3")]
        public string addressLine3 { get; set; }
        [MaxLength(100), Display(Name = "Town")]
        public string town { get; set; }
        [MaxLength(100), Display(Name = "County")]
        public string county { get; set; }
        [MaxLength(10), Display(Name = "Post code")]
        public string postcode { get; set; }
        [MaxLength(20), Display(Name = "Phone")]
        public string phone { get; set; }
        [MaxLength(100), Display(Name = "Email"), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string email { get; set; }
        [Required]
        public int tipstaffRecordID { get; set; }

        public virtual TipstaffRecord tipstaffRecord { get; set; }

        public virtual List<string> populatedLines
        {
            get
            {
                List<string> outputAddress = new List<string>();
                if (addresseeName != null) outputAddress.Add(addresseeName);
                outputAddress.Add(addressLine1);
                if (addressLine2 != null) outputAddress.Add(addressLine2);
                if (addressLine3 != null) outputAddress.Add(addressLine3);
                if (town != null) outputAddress.Add(town);
                if (county != null) outputAddress.Add(county);
                if (postcode != null) outputAddress.Add(postcode);
                return outputAddress;
            }
        }

        public virtual string xmlBlock
        {
            get
            {
                const string xmlString = "<w:tbl><w:tblPr><w:tblStyle w:val='TableGrid'/><w:tblW w:w='0' w:type='auto'/><w:tblLook w:val='01E0'/></w:tblPr><w:tblGrid><w:gridCol w:w='8856'/></w:tblGrid><w:tr wsp:rsidR='00DA2AE7' wsp:rsidTr='00DA2AE7'><w:tc><w:tcPr><w:tcW w:w='8856' w:type='dxa'/></w:tcPr><w:p wsp:rsidR='00DA2AE7' wsp:rsidRDefault='00DA2AE7'><w:pPr><w:rPr><w:lang w:val='EN-GB'/></w:rPr></w:pPr><w:r><w:rPr><w:lang w:val='EN-GB'/></w:rPr><w:t>{0}</w:t></w:r></w:p></w:tc></w:tr></w:tbl>";
                string xmlOutput = string.Format(xmlString, printAddressMultiLine);
                return xmlOutput;
            }
        }

        public virtual string printAddressMultiLine
        {
            get
            {
                List<string> popLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    popLines.Add(SecurityElement.Escape(line));
                }
                return string.Join("<w:br/>", popLines.ToArray());
            }
        }
        public virtual string screenAddressMultiLine
        {
            get
            {
                List<string> popLines = populatedLines;
                return string.Join("<br />", popLines.ToArray());
            }
        }
        public virtual string outputAddressSingleLine
        {
            get
            {
                List<string> popLines = populatedLines;
                string result = string.Join(",", popLines.ToArray());
                return result;
            }
        }
        public virtual string PrintAddressSingleLine
        {
            get
            {
                List<string> popLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    popLines.Add(SecurityElement.Escape(line));
                }
                return string.Join(", ", popLines.ToArray());
            }
        }
    }
}