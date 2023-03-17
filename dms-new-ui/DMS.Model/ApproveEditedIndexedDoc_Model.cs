using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class ApproveEditedIndexedDoc_Model
    {
        public string DeptName { get; set; }
        public string UnitName { get; set; }
        public string CateName { get; set; }
        public string SubCateName { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public string VersionName { get; set; }
        public DateTime VersionDate { get; set; }

        public string Signature { get; set; }
        public Int32 HideDocArchId { get; set; }
        public Int32 DocArchID { get; set; }
        public string FilePath { get; set; }

        public string Filepathfrom { get; set; }
        public string Filepathto { get; set; }

        public string attrbname { get; set; }
        public string attrid { get; set; }
        public string attrbtype { get; set; }
        public string attrblen { get; set; }
        public string attrbMand { get; set; }
        public string attrbdesc { get; set; }
        public string attrbmode { get; set; }
        public ApproveEditedIndexedDoc_Model()
        {
            dept = new List<ApproveEditedIndexedDoc_Model>();
        }
        public List<ApproveEditedIndexedDoc_Model> dept { get; set; }

        public string lblattribname { get; set; }
        public string attrctlname { get; set; }
        public int AtrLovId { get; set; }
        public List<string> txtattribdes { get; set; }
        public int UserId { get; set; }
    }
}
