using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Model;
using DMS.Data;
using System.Data;
using System.Reflection;


namespace DMS.Service
{
     public class DepartmentMaster_Service
    {

         DepartmentMaster_Data  Deptmstdataobj =new DepartmentMaster_Data();

         //read the data
         public List<DepartmentMaster_Model> DepatMstDtl(DepartmentMaster_Model Deptmodel)
         {
             return Deptmstdataobj.deptmstdetail( Deptmodel);
         }

         public List<DepartmentMaster_Model> GetMasterType()
         {
             return Deptmstdataobj.GetMasterType();
         }
         //insert the data
         public DepartmentMaster_Model DeptMstDtlSave(DepartmentMaster_Model Deptmodel)
         {
             return Deptmstdataobj.DptMstSave(Deptmodel);
         }


         //update the data
         public DepartmentMaster_Model DeptMstDtlUpdate(DepartmentMaster_Model Deptmodel)
         {
             return Deptmstdataobj.DptMstUpdate(Deptmodel);
         }

         public DataTable DeletingDepartment(string ID, string MasterTypeId)
         {
             return Deptmstdataobj.DeletingDepartment(ID, MasterTypeId);
         }
    }
}
