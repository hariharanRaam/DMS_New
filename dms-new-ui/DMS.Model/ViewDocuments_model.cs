using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DMS.Model
{
    public class ViewDocuments_Model
    {
        public int DocId { get; set; }
        public string DocName { get; set; }
        public string DocDep { get; set; }
        public string DocUnit { get; set; }
        public string DocCatg { get; set; }
        public string DocSubCatg { get; set; }
        public string View { get; set; }
        public string filepath { get; set; }

        public ViewDocuments_Model()
        {
            dept = new List<ViewDocuments_Model>();
        }
        public List<ViewDocuments_Model> dept { get; set; }

        public string attrbname { get; set; }
        public string attrctlname { get; set; }
        public string attrbtype { get; set; }

        public int DeptName { get; set; }
        public int UnitName { get; set; }
        public int CateName { get; set; }
        public int SubCateName { get; set; }

        public string AnimalType { get; set; }
        public string Name { get; set; }

        public List<MyColumnSettings> GridColumns { get; set; }

        public Int32 Attrib_Id { get; set; }


        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailSubject { get; set; }
        public string MailAttachement { get; set; }
        public string MailContent { get; set; }
        public string MailCc { get; set; }

        public List<SelectListItem> LovName { get; set; }
        public string[] Attribid { get; set; }

        public string Request_No { get; set; }
        [Display(Name = "Enter Date")]
        public DateTime RequestDate { get; set; }
        public int NOofDoc { get; set; }
        public int Request_From { get; set; }
        [Display(Name = "Enter Dates")]
        public DateTime RequiredDate { get; set; }
        public string Request_Note { get; set; }


        public class RequestRerival
        {
            public string Request_No { get; set; }
            [Display(Name = "Enter Date")]
            public string RequestDate { get; set; }
            public int NOofDoc { get; set; }
            public string Request_From { get; set; }
            [Display(Name = "Enter Dates")]
            public string RequiredDate { get; set; }
            public string Request_Note { get; set; }
        }

        public class RequestRerival1
        {
            public string Request_No { get; set; }
            public string RequestDate { get; set; }
            public int NOofDoc { get; set; }
            public string Request_From { get; set; }
            public string RequiredDate { get; set; }
            public string Request_Note { get; set; }
            public string Docname { get; set; }
            public string Docgroup { get; set; }
            public int Docname_Id { get; set; }
            public int Docgroup_Id { get; set; }
            public string Request_type { get; set; }
            public int Request_gid { get; set; }
            public string Attrib_ids { get; set; }
            public List<DespatchDropDown> Despatchdropdown { get; set; }
        }
    }

    public class DespatchDropDown {
        public int despatchmode_gid { get; set; }
        public string despatchmode_Name { get; set; }
    }

    public class MyColumnSettings
    {

        /// <summary>Title used in header.</summary>
        public string Title { get; set; }

        /// <summary>Property name in row viewmodel that the column is bound to.</summary>
        public string PropertyName { get; set; }


        /// <summary>True if field can be edited</summary>
        public bool Editable { get; set; }

        /// <summary>System type of the property being edited. Required for grid edtiable setting.</summary>
        public Type ColType { get; set; }

        /// <summary>Width to set the column</summary>
        public int Width { get; set; }

    }

}
