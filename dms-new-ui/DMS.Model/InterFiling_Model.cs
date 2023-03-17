using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
   public  class InterFiling_Model
    {
        public int DocId { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string DocGroup { get; set; }
        public string DocName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }        
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Supplier { get; set; }
        public int Attachid { get; set; }


        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }

        public int DeptId { get; set; }
        public int UnitId { get; set; }
        public int CatgId { get; set; }
        public int SubCatgId { get; set; }

        public string remarks { get; set; }
        public string activeflag { get; set; }

    }
}
