using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;
using DMS.Model;
using System.Reflection;

namespace DMS.Service
{
    public class ViewDocumentAttributes_Service
    {
        ViewDocumentAttributes_Data dataObj = new ViewDocumentAttributes_Data();
        public DataSet getdynamicattributes()
        {
            return dataObj.getdynamicattributes();
        }

        public DataTable Getattributes(Int32 GroupAtrID)
        {
            return dataObj.Getattributes(GroupAtrID);
        }

        public DataTable Getattribute(Int64 Attrbid, Int64 Dognameid, Int64 _empid)
        {
            List<ViewDocumentAttributes_Model> modelObj = new List<ViewDocumentAttributes_Model>();
            DataTable dt = new DataTable();
            dt = dataObj.Getattribute(Attrbid, Dognameid, _empid);
            return dt;
        }

        public DataTable Setattribute( Int64 DgroupID, Int64 DNameID, string dynamictxt, string dynamictxtlov,Int64 _empid)
        {
            List<ViewDocumentAttributes_Model> modelObj = new List<ViewDocumentAttributes_Model>();
            DataTable dt = new DataTable();
            dt = dataObj.Setattribute(DgroupID, DNameID, dynamictxt, dynamictxtlov, _empid);
            return dt;
        }

        public DataTable Geteditattribute(Int64 docnameId, Int64 docgrpId, Int64 unitId, Int64 depId, Int64 docnameId1, Int64 _empid)
        {
            //List<ViewDocumentAttributes_Model> serviceobj = new List<ViewDocumentAttributes_Model>();
            DataTable dt = new DataTable();
            try
            {

                dt = dataObj.Geteditattribute(docnameId, docgrpId, unitId, depId, docnameId1, _empid);
               // serviceobj = ConvertDataTable<ViewDocumentAttributes_Model>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
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
    }
}
