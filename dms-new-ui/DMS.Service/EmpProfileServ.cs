using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Data;
using DMS.Model;
using System.Data;


namespace DMS.Service
{
   public class EmpProfileServ
    {
       Emp_Profile obdata = new Emp_Profile();

       public DataTable EmpProfileSer(int emp_id)
       {
           return obdata.EmpDetail(emp_id);
   }
    }
}
