using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace DMS.Model
{
    public class SetDocAttributes_Model
    {
        //[Required(ErrorMessage = "Department is required.")]
        public string DeptName { get; set; }
        //[Required(ErrorMessage = "Unit is required.")]
        public string UnitName { get; set; }
        //[Required(ErrorMessage = "Category is required.")]
        public string CateName { get; set; }
        //[Required(ErrorMessage = "SubCategory is required.")]
        public string SubCateName { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public string VersionName { get; set; }
        public string FileSize { get; set; }

         [Display(Name = "Enter Date")] 
        public DateTime VersionDate { get; set; }

        public string Signature { get; set; }
        public Int32 HideDocArchId { get; set; }
        public Int32 DocArchID { get; set; }
        public string FilePath { get; set; }
        public byte[] filedata { get; set; }

        public string Filepathfrom { get; set; }
        public string Filepathto { get; set; }

       public string attrbname {get; set;}
       public string attrid { get; set; }
       public string attrbtype { get; set; }
       public string attrblen { get; set; }
       public string attrbMand { get; set; }
       public string attrbdesc { get; set; }
       public string attrbmode { get; set; }
       public SetDocAttributes_Model()
        {
            dept = new List<SetDocAttributes_Model>();
        }
       public List<SetDocAttributes_Model> dept { get; set; }
      
       public string lblattribname { get; set; }
       public string attrctlname { get; set; }

       public string AtrLabel1 { get; set; }
       public string AtrText1 { get; set; }
       public string AtrLen { get; set; }
       public string AtrType { get; set; }
       public string AtrMand { get; set; }
       public int AtrLovId { get; set; }
       public List<string> txtattribdes { get; set; }
       public int UserId { get; set; }
       public List<SelectListItem> LovName { get; set; }

        public string Satr_Name {get;set;}
        public int Satr_Length {get;set;}
        public string Satr_Type {get;set;}
        public string Satr_Mandotry {get;set;}
        public string Sattribdtl_Description {get;set;}
        public string autonumberrowid { get; set; }
        public string autonumbervalue { get; set; }

        public string activeflag { get; set; }
    }
}
