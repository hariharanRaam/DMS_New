using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
   public class CreateMaster_Model
    {
       public string Id{ get; set; }
       
        public string DependName { get; set; }
       
        [Required(ErrorMessage = "Please Enter The Department Name ")]
       public string Name { get; set; }
       //[Range(1, int.MaxValue, ErrorMessage = "Select Value.!")]
        public string DependId { get; set; }
       // [Required(ErrorMessage = "Please Enter The Department Created Date ")]
       //public DateTime CreatedDate { get; set; }
       // [Required(ErrorMessage = "Please Enter The Department Created By")]
       public string Createdby { get; set; }
        public string MasterTypeId { get; set; }
        public string MasterTypeName { get;set;}
 }
}
