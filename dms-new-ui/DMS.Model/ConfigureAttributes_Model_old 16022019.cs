using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
   public class ConfigureAttributes_Model
    {
       public string Attri_Name { get; set; }
       public int Attri_Length { get; set; }
       public string Attri_type { get; set; }
       public string Attri_Mandatory { get; set; }
       public int Dept_Id { get; set; }
       public int Unit_Id { get; set; }
       public int DocGroup_Id { get; set; }
       public int DocName_Id { get; set; }

       public class Dep_union_dropdown
       {
           public Int64 Dept_Id { get; set; }
           public string Dept_Name { get; set; }

           public Int64 unit_id { get; set; }
           public string unit_name { get; set; }

           public Int64 Dgroup_Id { get; set; }
           public string Dgroup_Name { get; set; }

           public Int64 Dname_Id { get; set; }
           public string Dname_Name { get; set; }

           //public Int64 Lov_Id { get; set; }
           //public string Lov_Name { get; set; }
       }

    }
}
