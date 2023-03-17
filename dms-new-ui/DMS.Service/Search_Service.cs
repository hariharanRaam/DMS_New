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
    public class Search_Service
    {
        Search_Data DataObj = new Search_Data();

        public DataSet GetDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, string form)
        {
            return DataObj.GetDocuments(DeptID1, Unit1, Dgroup1, Dname1, form);
        }

        public DataSet GetLinkedDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            return DataObj.GetLinkedDocuments(DeptID1, Unit1, Dgroup1, Dname1, Attribid);
        }

        public DataSet GetInterFilingDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            return DataObj.GetInterlingDocuments(DeptID1, Unit1, Dgroup1, Dname1);
        }

        public DataSet GetInterFilingLinkedDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            return DataObj.GetInterlingLinkDocuments(DeptID1, Unit1, Dgroup1, Dname1, Attribid);
        }

        public DataTable GetDynamicFields(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            return DataObj.GetDynamicFields(DeptID1, Unit1, Dgroup1, Dname1);
        }
        public DataTable getmasterval(int LovId,string Lovtype)
        {
            return DataObj.getmastervalue(LovId,Lovtype);
        }
        public List<Dep_union_dropdown> GeALL(string type, string actiontype)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt1, dt2, dt3, dt4 = new DataTable();
            try
            {
                ds = DataObj.GetDropdown(type, actiontype);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];

                if (type == "department")
                {
                    if (dt1.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt1);
                    }
                }

                if (type == "unit")
                {
                    if (dt2.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt2);
                    }
                }

                if (type == "CateGory")
                {
                    if (dt3.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt3);
                    }
                }

                if (type == "SubCateGory")
                {
                    if (dt4.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt4);
                    }
                }

                return dropdown;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            throw new NotImplementedException();
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

        public List<Dep_union_dropdown> GetUnit(Int64 Department_id)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDepartmentChange(Department_id);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocGroup(Int64 Department_id)
        {
            List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDepartmentChange(Department_id);
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown1;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocName(Int64 Department_id)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDepartmentChange(Department_id);
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    dropdown2 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown2;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDept1(Int64 Unit_Id)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetUnitChange(Unit_Id);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocGroup1(Int64 Unit_Id)
        {
            List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetUnitChange(Unit_Id);
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown1;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocName1(Int64 Unit_Id)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetUnitChange(Unit_Id);
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    dropdown2 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown2;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDept2(Int64 DocGroup_Id)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocGroupChange(DocGroup_Id);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetUnit2(Int64 DocGroup_Id)
        {
            List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocGroupChange(DocGroup_Id);
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown1;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocName2(Int64 DocGroup_Id)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocGroupChange(DocGroup_Id);
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    dropdown2 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown2;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDept3(Int64 DocNameId)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocNameChange(DocNameId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetUnit3(Int64 DocNameId)
        {
            List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocNameChange(DocNameId);
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown1;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dep_union_dropdown> GetDocGroup3(Int64 DocNameId)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocNameChange(DocNameId);
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    dropdown2 = ConvertDataTable<Dep_union_dropdown>(dt);
                }
                return dropdown2;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SingleFileDownload(int attachid)
        {
            string Result;
            try
            {
                Result = DataObj.SingleDownload(attachid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        public DataSet Getmaildetails(int? Indexed_gid, int EmpID, string type)
        {
            try
            {
                return DataObj.Getmaildetails(Indexed_gid, EmpID, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
