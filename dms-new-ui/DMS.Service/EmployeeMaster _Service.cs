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
    public class EmployeeMaster__Service
    {
        EmployeeMaster_Data Edataobj = new EmployeeMaster_Data();

        public EmployeeMaster_Model EmployeeMstDtl(int _userid)
        {
            return Edataobj.EmpMstDtl(_userid);
        }

        public List<EmployeeMaster_Model> EmployeeMstDtllist(EmployeeMaster_Model EDtlObj)
        {
            return Edataobj.EmpMstDtllist(EDtlObj);
        }

        public EmployeeMaster_Model EmployeeMstDtlUpdate(EmployeeMaster_Model EDtlObj)
        {
            return Edataobj.EmpMstUpdate(EDtlObj);
        }
        //public DataTable EmployeeMstDtlUpdate(EmployeeMaster_Model EDtlObj)
        //{
        //    return Edataobj.EmpMstUpdate(EDtlObj);
        //}

        public List<EmployeeMaster_Model> GetDepartment(string CommonVal)
        {
            try
            {
                List<EmployeeMaster_Model> dropdown = new List<EmployeeMaster_Model>();
                DataTable dt1 = new DataTable();
                dt1 = Edataobj.GetDepartments(CommonVal);
                dropdown = ConvertDataTable<EmployeeMaster_Model>(dt1);
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

        public object SaveEmployee(EmployeeMaster_Model EDtlObj)
        {
            return Edataobj.SaveEmployee(EDtlObj);
        }

        public DataTable DeletingDepartment(int? EmployeeID)
        {
            return Edataobj.DeletingDepartment(EmployeeID);
        }

        public EmployeeMaster_Model GetEmpOrglvls(string empid) {
            return Edataobj.GetEmporglvl(empid);
        }

         public DataSet EmployeeMasterSrv() {
          DataSet ds = new DataSet();
          try
          {
              ds = Edataobj.EmployeeMasterRead();
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return ds;
      }
    
    }
}
