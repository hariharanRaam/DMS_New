using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class PhysicalArchival_Model
    {
        public string Satrname { get; set; }
        public string Satrid { get; set; }
        public string Satrtype { get; set; }
        public string Satrlen { get; set; }
        public string SatrMand { get; set; }
        public string Satrdesc { get; set; }
        public string Satrmode { get; set; }
        public PhysicalArchival_Model()
        {
            dept = new List<PhysicalArchival_Model>();
        }
        public List<PhysicalArchival_Model> dept { get; set; }
        public int UserId { get; set; }
        public int SatrCount { get; set; }
        public string DynamicVal { get; set; }
        public int Atrgid { get; set; }
        public string action { get; set; }


        public string Lbl_Dept { get; set; }
        public string Lbl_Unit { get; set; }



        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }

        public int DeptId { get; set; }
        public int UnitId { get; set; }
        public int CatgId { get; set; }
        public int SubCatgId { get; set; }

        public Int64? FileID { get; set; }


        public string Attributescol { get; set; }
        public string StorageAttribcol { get; set; }

        public string Attributesval { get; set; }
        public string StorageAttribval { get; set; }

        public string activeflag { get; set; }
    }
}
