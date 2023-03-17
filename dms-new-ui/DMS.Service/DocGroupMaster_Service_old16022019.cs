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
  public   class DocGroupMaster_Service
    {
      DocgroupMaster_Data Docgrdatapobj = new DocgroupMaster_Data();
      //read the data
      public List<DocGroupMaster_Model> DocgrpMstDtl()
      {
          return Docgrdatapobj.docgrpmstdetail();
      }

      public DocGroupMaster_Model DocGroupMstDtlSave(DocGroupMaster_Model Docgrpmdlobj)
      {
          return Docgrdatapobj.DocGroupMstSave(Docgrpmdlobj);
      }

      public DocGroupMaster_Model DocGroupMstDtlUpdate(DocGroupMaster_Model Docgrpmdlobj)
      {
          return Docgrdatapobj.DocGroupMstUpdate(Docgrpmdlobj);
      }
      public List<DocGroupMaster_Model> GetDocGroup(string CommonVal)
      {
          try
          {
              List<DocGroupMaster_Model> dropdown = new List<DocGroupMaster_Model>();
              DataTable dt1 = new DataTable();
              dt1 = Docgrdatapobj.GetDocGroups(CommonVal);
              dropdown = ConvertDataTable<DocGroupMaster_Model>(dt1);
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


      public DataTable DeletingDocGroup(int? DGroupID)
      {
          return Docgrdatapobj.DeletingDocGroup(DGroupID);
      }
    }
}
