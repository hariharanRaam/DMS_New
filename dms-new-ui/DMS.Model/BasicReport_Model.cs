using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class BasicReport_Model
    {
        public string mastercode { get; set; }
        public string mastername { get; set; }
        public Int64 DeptID { get; set; }
        public string DeptName { get; set; }
        public Int64 UnitID { get; set; }
        public string UnitName { get; set; }
        public Int64 CateID { get; set; }
        public string CateName { get; set; }
        public Int64 SubCateID { get; set; }
        public string SubCateName { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public string Flag { get; set; }
        public string FileName { get; set; }
        public Int64 DocID { get; set; }
        public Int64 SlNo { get; set; }
        public string UploadedBy { get; set; }
        public string UploadStatus { get; set; }
        public string UploadedFilePath { get; set; }

        public string Lbl_Dept { get; set; }
        public string Lbl_Unit { get; set; }
        public Int64 UserID { get; set; }

        public Int64 AttribID { get; set; }
        public string AttribName { get; set; }
        public string inAction { get; set; }
        public string LovName { get; set; }

        public string Atr_Conditions { get; set; }

        public string Atr_load_action { get; set; }

        public string activeflag { get; set; }

        public string Docstat { get; set; }
    }
}
