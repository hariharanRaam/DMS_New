using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
    public class OrgHierarchy_Model
    {
       public Int32 OrgGId{ get; set; }
       [Required(ErrorMessage = "Please enter OrGcode")]
       [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
       public string OrgCode { get; set; }
        public string OrgName { get; set; }

       
      //  public List<OrgHierarchy_Model> OrgModel { get; set; }
       public string Createdby { get; set; }
       
 }
}
