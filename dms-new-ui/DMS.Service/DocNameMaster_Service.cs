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
    public class DocNameMaster_Service
    {
        DocNameMaster_Data dataObj = new DocNameMaster_Data();
        public DataSet GetAllDocNames(int dnameid_)
        {
            return dataObj.GetAllDocNames(dnameid_);
        }

        public List<DocNameMaster_Model> GetmasterDorpdowns(string CommonVal)
        {
            try
            {
                List<DocNameMaster_Model> dropdown = new List<DocNameMaster_Model>();
                DataTable dt1 = new DataTable();
                dt1 = dataObj.GetmasterDorpdowns(CommonVal);
                dropdown = ConvertDataTable<DocNameMaster_Model>(dt1);
                return dropdown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Convert Datatable to List
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion


        public object SaveDocName(DocNameMaster_Model ModelObj)
        {
            return dataObj.SaveDocName(ModelObj);
        }

        public object UpdateDocName(DocNameMaster_Model ModelObj)
        {
            return dataObj.UpdateDocName(ModelObj);
        }

        public DataTable DeletingDocName(int? DNameID)
        {
            return dataObj.DeletingDocName(DNameID);
        }

        public List<DocNameMaster_Model> unit(int? masteritemid, string master)
        {
            try
            {
                List<DocNameMaster_Model> unitdropdownlist = new List<DocNameMaster_Model>();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = dataObj.unit(masteritemid, master);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                unitdropdownlist = ConvertDataTable<DocNameMaster_Model>(dt);
                return unitdropdownlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DocNameMaster_Model> deptarment(int? masteritemid, string master)
        {
            try
            {
                List<DocNameMaster_Model> deptdropdownlist = new List<DocNameMaster_Model>();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = dataObj.unit(masteritemid, master);
                if (ds.Tables.Count > 0)
                {
                    if (master == "Unit")
                    {
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        dt = ds.Tables[1];
                    }
                   
                }
                deptdropdownlist = ConvertDataTable<DocNameMaster_Model>(dt);
                return deptdropdownlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
