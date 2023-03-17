using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
    public class UnitMaster_Model
    {
        public int UnitID { get; set; }
        [Required(ErrorMessage="Unit Code should not be blank")]
        public string UnitCode { get; set; }
        [Required(ErrorMessage = "Unit Name should not be blank")]
        public string UnitName { get; set;}

        [Range(1, int.MaxValue, ErrorMessage = "Select department.!")]
        public Int64 Dept_Id { get; set; }
        public string Dept_Name { get; set; }
        public int UserID { get; set; }
    }
}
