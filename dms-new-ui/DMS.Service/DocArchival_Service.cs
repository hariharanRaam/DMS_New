using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.IO;
using DMS.Model;
using DMS.Data;
using System.Reflection;

namespace DMS.Service
{
    public class DocArchival_Service
    {
        //Data Object
        DocArchival_Data DataObj = new DocArchival_Data();

        /***** Multiple document archival/Uploading Service method - START *****/
        public int SaveDocInfo(DocArchival_Model ModelObj, List<HttpPostedFileBase> FileUpload1)
        {
            int result;
            try
            {
                result = DataObj.SaveMultipleDocDetails(ModelObj, FileUpload1);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /***** Multiple document archival/Uploading Service method - END *****/

        public int SaveSingleFile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            int Result;
            try
            {
                Result = DataObj.SaveSingleDocDetails(ModelObj, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

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

        public List<Dep_union_dropdown> GeALL(string type, string actiontype, Int64 empid)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt1, dt2, dt3, dt4 = new DataTable();
            try
            {
                ds = DataObj.GetDropdown(type, actiontype,empid);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
               // dt4 = ds.Tables[3];

                if (type == "CateGory")
                {
                    if (dt1.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt1);
                    }
                }

                if (type == "SubCateGory")
                {
                    if (dt2.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt2);
                    }
                }

                if (type == "Lov_Master")
                {
                    if (dt3.Rows.Count > 0)
                    {
                        dropdown = ConvertDataTable<Dep_union_dropdown>(dt3);
                    }
                }

                //if (type == "SubCateGory")
                //{
                //    if (dt4.Rows.Count > 0)
                //    {
                //        dropdown = ConvertDataTable<Dep_union_dropdown>(dt4);
                //    }
                //}

                return dropdown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //throw new NotImplementedException();
        }


        public List<Dep_union_dropdown> GetALLdropdown(string type, string condition_1, string condition_2,string condition_3,string condition_4, Int64 empid)
        {
            List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                dt = DataObj.GetDropdownNew(type, condition_1,condition_2, condition_3,condition_4, empid);
                dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
                return dropdown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //throw new NotImplementedException();
        }

        public List<Dep_union_dropdown> GetselectedItem(int? docid, string type)
        {
            List<Dep_union_dropdown> selectdropdown = new List<Dep_union_dropdown>();
            DataTable dt = new DataTable();
            dt = DataObj.GetselectDropdown(docid, type);
            if (dt.Rows.Count > 0)
            {
                selectdropdown = ConvertDataTable<Dep_union_dropdown>(dt);
            }
            return selectdropdown;
        }

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

        //public List<Dep_union_dropdown> GetDept2(Int64 DocGroup_Id)
        //{
        //    List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        ds = DataObj.GetDocGroupChange(DocGroup_Id);
        //        dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
        //        }
        //        return dropdown;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<Dep_union_dropdown> GetDept3(Int64 DocNameId)
        //{
        //    List<Dep_union_dropdown> dropdown = new List<Dep_union_dropdown>();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        ds = DataObj.GetDocNameChange(DocNameId);
        //        dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            dropdown = ConvertDataTable<Dep_union_dropdown>(dt);
        //        }
        //        return dropdown;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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

        //public List<Dep_union_dropdown> GetUnit2(Int64 DocGroup_Id)
        //{
        //    List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        ds = DataObj.GetDocGroupChange(DocGroup_Id);
        //        dt = ds.Tables[1];
        //        if (dt.Rows.Count > 0)
        //        {
        //            dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
        //        }
        //        return dropdown1;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<Dep_union_dropdown> GetUnit3(Int64 DocNameId)
        //{
        //    List<Dep_union_dropdown> dropdown1 = new List<Dep_union_dropdown>();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        ds = DataObj.GetDocNameChange(DocNameId);
        //        dt = ds.Tables[1];
        //        if (dt.Rows.Count > 0)
        //        {
        //            dropdown1 = ConvertDataTable<Dep_union_dropdown>(dt);
        //        }
        //        return dropdown1;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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

        public List<Dep_union_dropdown> GetDocName2(Int64 DocGroup_Id, Int64 empid)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocGroupChange(DocGroup_Id, empid);
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

        public List<Dep_union_dropdown> GetDocGroup3(Int64 DocNameId, Int64 empid)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocNameChange(DocNameId, empid);
                dt = ds.Tables[0];
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

        public DataTable DeleteScannedFile(Int64? id, string mode, DocArchival_Model ModelObj)
        {
            return DataObj.DeleteScannedFile(id, mode, ModelObj);
        }

        public int EditScannedDocwithfile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            return DataObj.EditScannedDocwithfile(ModelObj, file);
        }

        public int EditScannedDocwithoutfile(DocArchival_Model ModelObj)
        {
            return DataObj.EditScannedDocwithoutfile(ModelObj);
        }

        public string checkfile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            return DataObj.checkfile(ModelObj, file);
        }

        public string checkfilemultiple(DocArchival_Model ModelObj, List<HttpPostedFileBase> FileUpload1)
        {
            return DataObj.checkfilemultiple(ModelObj, FileUpload1);
        }
        public string checkfilemultiplecache(DocArchival_Model ModelObj, HttpPostedFileBase FileUpload1)
        {
            return DataObj.checkfilemultiplecache(ModelObj, FileUpload1);
        }
    }
}
