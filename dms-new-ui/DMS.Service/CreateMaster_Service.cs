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
     public class CreatetMaster_Service
    {

         CreateMaster_Data Deptmstdataobj = new CreateMaster_Data();

         //read the data
         public List<CreateMaster_Model> DepatMstDtl(CreateMaster_Model Deptmodel)
         {
             return Deptmstdataobj.deptmstdetail( Deptmodel);
         }

         public List<CreateMaster_Model> GetMasterType()
         {
             return Deptmstdataobj.GetMasterType();
         }
         public List<CreateMaster_Model> GetQcdMaster(string sParentCode)
         {
             return Deptmstdataobj.GetQcdMaster( sParentCode);
         }
         //insert the data
         public CreateMaster_Model DeptMstDtlSave(CreateMaster_Model Deptmodel)
         {
             return Deptmstdataobj.DptMstSave(Deptmodel);
         }


         //update the data
         public CreateMaster_Model DeptMstDtlUpdate(CreateMaster_Model Deptmodel)
         {
             return Deptmstdataobj.DptMstUpdate(Deptmodel);
         }

         public DataTable DeletingDepartment(string ID, string MasterTypeId)
         {
             return Deptmstdataobj.DeletingDepartment(ID, MasterTypeId);
         }
    }
}
