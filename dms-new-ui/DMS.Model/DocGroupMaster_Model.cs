using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
   public class DocGroupMaster_Model
    {
        public int DgroupId { get; set; }
        [Required(ErrorMessage = "please enter the DocGroup Name")]
        public string DgroupName { get; set; }
        // [Required]  
        [StringLength(5)]
        public string DgrpId { get; set; }
        public string mode { get; set; }
        public string Dgroup_shortname { get; set; }
        public string Org_Level1 { get; set; }
        public string Org_Level2 { get; set; }
        public string Org_Level3 { get; set; }
        public string Org_Level4 { get; set; }
        public string Org_Level1code { get; set; }
        public string Org_Level2code { get; set; }
        public string Org_Level3code { get; set; }
        public string Org_Level4code { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select deparment.!")]
        public Int64 Dept_Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select unit.!")]
        public Int64 UnitID { get; set; }
        public string DgroupCreatedBy { get; set; }
        public List<DocGroupDynamicLop> docgrouplist { get; set; }
        public List<DocGroupDynamicLabel> doclabellist { get; set; }
       public class DocGroupDynamicLabel
       {
           public string orghierarchy_code { get; set; }
           public string orghierarchy_name { get; set; }

       }

       public class DocGroupDynamicLop {
           public string master_code { get; set; }
           public string master_name { get; set; }
           public string parent_code { get; set; }
       }
    
    }
}
