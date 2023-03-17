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
    public class LOVMaster_Service
    {
        LOVMaster_Data DataObj = new LOVMaster_Data();
        LOVMaster_Model modelObj = new LOVMaster_Model();

        public Int32 SaveLovMaster(string lovname, int UserID)
        {
            //DataSet ds = new DataSet();
            //textbox value to save in the lovhdr table
            try
            {
                int ret = DataObj.SaveLMaster(lovname, UserID);
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int32 SaveLovdtl(List<LOVMaster_Model> lstmodel, int UserID)
        {
            //DataSet ds = new DataSet();
            //excel file  data to save in the lovdtl table
            try
            {
                int ret = DataObj.SaveLodtl(lstmodel, UserID);
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFile(string Filename)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DataObj.GetFile(Filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }



    }
}
