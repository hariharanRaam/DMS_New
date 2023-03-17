using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DMS.Model
{
    public class TreeSearch_Model
    {
        public Int64 Gid { get; set; }
        public String HierarchyName { get; set; }
        //public Nullable<int> PerentId { get; set; }
        public String PerentId { get; set; }
        public String Flag { get; set; }
        public Int64 Hirerarchy_Id { get; set; }


        public Int64 Atr_Id { get; set; }
        public String Atr_Name { get; set; }
        public Int64 Atr_Length { get; set; }
        public String Atr_Type { get; set; }
        public String Atr_Mandotry { get; set; }
        public Int64 Dgroup_Id { get; set; }
        public Int64 Dname_Id { get; set; }
        public String Dgroup_Name { get; set; }
        public String Dname_Name { get; set; }
        public String attrctlname { get; set; }
        public List<SelectListItem> LovName { get; set; }
        public List<SelectListItem> operators { get; set; }
        public List<SelectListItem> Conditions { get; set; }
        public TreeSearch_Model()
        {
            dept = new List<TreeSearch_Model>();
        }
        public List<TreeSearch_Model> dept { get; set; }

        
    }

    public class HierarchyDetail
    {
        public int Id { get; set; }
        public String HierarchyName { get; set; }
        public Int64 PerentId { get; set; }
        public String Flag { get; set; }
        public Int64 Hirerarchy_Id { get; set; }
    }

    public class Attributes
    {
        public Int64 Atr_Id { get; set; }
        public String Atr_Name { get; set; }
        public Int64 Atr_Length { get; set; }
        public String Atr_Type { get; set; }
        public String Atr_Mandotry { get; set; }
        public Int64 Dgroup_Id { get; set; }
        public Int64 Dname_Id { get; set; }
        public String attrctlname { get; set; }
        public Attributes()
        {
            dept = new List<Attributes>();
        }
        public List<Attributes> dept { get; set; }
    }

    public class HierarchyViewModel
    {
        public int Id { get; set; }
        public string text { get; set; }
        public Int64? perentId { get; set; }
        //public string perentId { get; set; }
        public String Flag { get; set; }
        public Int64 Hirerarchy_Id { get; set; }
        public virtual List<HierarchyViewModel> children { get; set; }
    }
}
