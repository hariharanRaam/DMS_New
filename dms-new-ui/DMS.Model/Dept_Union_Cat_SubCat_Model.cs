using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DMS.Model
{
    public class Dept_Union_Cat_SubCat_Model
    {
        public Dept_Union_Cat_SubCat_Model()
        {
            this.Depts = new List<SelectListItem>();
            this.Unions = new List<SelectListItem>();
            this.Categorys = new List<SelectListItem>();
            this.SubCategorys = new List<SelectListItem>();
        }
        public List<SelectListItem> Depts { get; set; }
        public List<SelectListItem> Unions { get; set; }
        public List<SelectListItem> Categorys { get; set; }
        public List<SelectListItem> SubCategorys { get; set; }

        public int DeptID { get; set; }
        public int UnionID { get; set; }
        public int CateID { get; set; }
        public int SubCateID { get; set; }
        public string ActionMode { get; set; }
        public int? FileId { get; set; }

        public int Unit { get; set; }
        public int Catgery { get; set; }
        public int subCatgery { get; set; }
    }

    public class Dep_union_dropdown
    {
        public Int64 Dept_Id { get; set; }
        public string Dept_Name { get; set; }
        public string Doc_Arch_Name { get; set; }

        public Int64 unit_id { get; set; }
        public string unit_name { get; set; }

        public string master_code { get; set; }
        public string master_name { get; set; }


        //public Int64 Catg_Id { get; set; }
        //public string Catg_Name { get; set; }


        //public Int64 SubCatg_Id { get; set; }
        //public string SubCatg_Name { get; set; }

        public Int64 Dgroup_Id { get; set; }
        public string Dgroup_Name { get; set; }


        public Int64 Dname_Id { get; set; }
        public string Dname_Name { get; set; }

        public Int64 Lov_Id { get; set; }
        public string Lov_Name { get; set; }

        public Int64 autonumber_rowid { get; set; }
        public string autonumberdtl { get; set; }
    }
}
