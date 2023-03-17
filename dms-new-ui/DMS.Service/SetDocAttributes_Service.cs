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
    public class SetDocAttributes_Service
    {
        SetDocAttributes_Data DataObj = new SetDocAttributes_Data();
        public DataSet getalldepartements(int RefId, string RefType)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DataObj.GetDeptCatgLst(RefId, RefType);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet InitValues(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DataObj.InitValues(id);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveProperties(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            int Result;
            try
            {
                Result = DataObj.SaveDetails(deptsModel, ModelObjList1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public int UpdateProperties(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            int Result;
            try
            {
                Result = DataObj.UpdateProperties(deptsModel, ModelObjList1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public DataTable getmasterval(int LovId, string type)
        {
            return DataObj.getmastervalue(LovId, type);
        }

        public DataSet InitEditValues(int id)
        {
            return DataObj.Initeditvalues(id);
        }
        public int Deletefile(SetDocAttributes_Model deptsModel)
        {
            return DataObj.Deletefile(deptsModel);
        }

        //public DataSet storagedata(int? attribgid)
        //{
        //    return DataObj.storagedetail(attribgid);
        //}


        public DataTable ValidateAttributes(SetDocAttributes_Model deptsModel, List<SetDocAttributes_Model> ModelObjList1)
        {
            return DataObj.ValidateAttributes(deptsModel, ModelObjList1);
        }

        public DataTable Check_Document_Under_Modification(string Doc_GID)
        {
            try
            {
                return DataObj.Check_Document_Under_Modification(Doc_GID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
