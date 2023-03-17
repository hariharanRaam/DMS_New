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
    public class UnitMaster_Service
    {
        UnitMaster_Data dataObj = new UnitMaster_Data();
        public List<UnitMaster_Model> Getallunits()
        {
            return dataObj.getallunits();
        }

        public object SaveUnit(UnitMaster_Model ModelObj)
        {
            return dataObj.SaveUnit(ModelObj);
        }

        public object UpdateUnit(UnitMaster_Model ModelObj)
        {
            return dataObj.UpdateUnit(ModelObj);
        }

        public List<UnitMaster_Model> GetDepartment(string CommonVal)
        {
            try
            {
                List<UnitMaster_Model> dropdown = new List<UnitMaster_Model>();
                DataTable dt1 = new DataTable();
                dt1 = dataObj.GetDepartments(CommonVal);
                dropdown = ConvertDataTable<UnitMaster_Model>(dt1);
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


        public DataTable deletingUnit(int? UnitID)
        {
            return dataObj.DeletingUnit(UnitID);
        }
    }
}
