using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
    public class DocNameMaster_Model
    {
        public int DNameID { get; set; }
        [Required(ErrorMessage="Doc Name Should not be blank")]
        public string DocName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select department.!")]
        public Int64 Dept_Id { get; set; }
        public string Dept_Name { get; set; }
       [Range(1, int.MaxValue, ErrorMessage = "Select unit.!")]
        public Int64 UnitID { get; set; }
        public string Unit { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select docgroup.!")]
        public Int64 DgroupID { get; set; }
        public string DgroupName { get; set; }
        public int UserID { get; set; }
    }
}
