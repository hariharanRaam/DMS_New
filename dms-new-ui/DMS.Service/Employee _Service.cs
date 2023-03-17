using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;
using DMS.Model;


namespace DMS.Service
{
   public class Employee_Service
    {
       Employee_Model empmdlob = new Employee_Model();
       Employee_Data empdataobj = new Employee_Data();
       DataTable dt1 =new DataTable();
       DataSet ds = new DataSet();
       public DataSet SaveEmpdtl(List<Model.Employee_Model> lstmodel, DataTable dt1)
        {
          
         try
             {
                
              ds= empdataobj.Saveemdtl(lstmodel,dt1);
               return ds;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
         }
     public DataSet GetFile(string Filename)
     {
          
         try
         {
             ds = empdataobj.GetFile(Filename);
         }
         catch (Exception ex)
         {
             throw ex;
         }

         return ds;
     }



     public object DeptDtl()
     {
         throw new NotImplementedException();
     }

     public DataTable getexceldata(string data)
     {
         DataTable dt = new DataTable();
         try
         {
             dt = empdataobj.exceldata(data);
         }
         catch(Exception ex)
         {
             throw ex;
         }
         return dt;
     }
    }
}
