using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Data;
using DMS.Model;
using System.Data;
using System.Reflection;

namespace DMS.Service
{
    public class TreeSearch_services
    {
        TreeSearch_Data DataObj = new TreeSearch_Data();

        public List<HierarchyDetail> GetTreeData()
        {
            List<HierarchyDetail> TreeView = new List<HierarchyDetail>();
             try
             {

                 DataSet ds = new DataSet();
                 DataTable dt = new DataTable();
                 ds = DataObj.GetTreeData();
                 dt = ds.Tables[0];
                 TreeView = ConvertDataTable<HierarchyDetail>(dt);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
            return TreeView;
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

        public DataSet GetAttributes(Int64 DocgroupId, Int64 DocNameId, Int64 _empid)
        {
            List<Attributes> TreeView = new List<Attributes>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try { ds = DataObj.GetAttributes(DocgroupId,DocNameId, _empid); }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetDocuments(string Attributetypes1, string atrname1, string Docgroupid1, string Docnameid1, Int64 _empid, string Dyntextvalues1, string Conditions1, string operators1)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DataObj.GetDocuments(Attributetypes1, atrname1, Docgroupid1, Docnameid1, _empid, Dyntextvalues1, Conditions1, operators1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
    }
}
