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
       [Required (ErrorMessage="please enter the DocGroup Name")]
       public string DgroupName { get; set; }

       public string Dept_Name { get; set; }
       [Range(1, int.MaxValue, ErrorMessage = "Select deparment.!")]
       public Int64 Dept_Id { get; set; }
       
       public string Unit{ get; set; }
       [Range(1, int.MaxValue, ErrorMessage = "Select unit.!")]
       public Int64 UnitID { get; set; }
       public string DgroupCreatedBy { get; set; }     
    }
}
