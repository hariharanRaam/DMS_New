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
      private List<DocGroupMaster_Model.DocGroupDynamicLabel> dropdownlabels;
      //read the data
      public DocGroupMaster_Model DocgrpMstDtl(int DgroupId)
      {
          return Docgrdatapobj.docgrpmstdetail(DgroupId);
      }

      public DocGroupMaster_Model DocGroupMstDtlSave(DocGroupMaster_Model Docgrpmdlobj)
      {
          return Docgrdatapobj.DocGroupMstSave(Docgrpmdlobj);
      }

      public DocGroupMaster_Model DocGroupMstDtlUpdate(DocGroupMaster_Model Docgrpmdlobj)
      {
          return Docgrdatapobj.DocGroupMstUpdate(Docgrpmdlobj);
      }
      public List<DocGroupMaster_Model.DocGroupDynamicLop> GetDocGroup(string parentcode, string dependcode)
      {
          try
          {
              List<DocGroupMaster_Model.DocGroupDynamicLop> dropdown = new List<DocGroupMaster_Model.DocGroupDynamicLop>();
              DataTable dt1 = new DataTable();
              dt1 = Docgrdatapobj.GetDocGroups(parentcode,dependcode);
              dropdown = ConvertDataTable<DocGroupMaster_Model.DocGroupDynamicLop>(dt1);
              return dropdown;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public List<DataTable> GetDocGroupsNew(string parentcode, string dependcode, int maxorg, int currentorg)
      {
          List<DataTable> data_ = new List<DataTable>();
          try
          {
              data_ = Docgrdatapobj.GetDocGroupsNew(parentcode, dependcode, maxorg, currentorg);
          }
          catch (Exception er) { 
          
          }
          return data_;
      }

      public List<DocGroupMaster_Model.DocGroupDynamicLabel> GetDoclabelsrv()
      {
          try
          {
              List<DocGroupMaster_Model.DocGroupDynamicLabel> dropdownlabels = new List<DocGroupMaster_Model.DocGroupDynamicLabel>();
              DataTable dt1 = new DataTable();
              dt1 = Docgrdatapobj.GetDoclabels();
              dropdownlabels = ConvertDataTable<DocGroupMaster_Model.DocGroupDynamicLabel>(dt1);
              return dropdownlabels;
          }
          catch (Exception ex)
          {
              return dropdownlabels;
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

      public DataSet DocGroupGridread() {
          DataSet ds = new DataSet();
          try
          {
              ds = Docgrdatapobj.DocGroupGridread();
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return ds;
      }
    }
}
