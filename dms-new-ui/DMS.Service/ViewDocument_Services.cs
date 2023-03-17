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
    public class ViewDocument_Services
    {
        //ViewDocument_Data objData = new ViewDocument_Data();
        ViewDocument_Data DataObj = new ViewDocument_Data();

        public DataSet GetDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, string form)
        {
            return DataObj.GetDocuments(DeptID1, Unit1, Dgroup1, Dname1, form);
        }

        public DataSet GetDocumentsmulti(string Dgroup1, string Dname1, string form)
        {
            return DataObj.GetDocumentsmulti(Dgroup1, Dname1, form);
        }

        public DataSet GetMultifile(string Attribid)
        {
            return DataObj.GetMultifile(Attribid);
        }

        public DataSet GetLinkedDocuments( int? Dgroup1, int? Dname1, Int64 Attribid)
        {
            return DataObj.GetLinkedDocuments( Dgroup1, Dname1, Attribid);
        }

        public DataSet GetRetrievalaudit(Int64 Attribid)
        {
            return DataObj.GetRetrievalaudit(Attribid);
        }

        public DataSet GetInterFilingDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            return DataObj.GetInterlingDocuments(DeptID1, Unit1, Dgroup1, Dname1);
        }

        public DataSet GetInterFilingLinkedDocuments( int? Dgroup1, int? Dname1, Int64 Attribid)
        {
            return DataObj.GetInterlingLinkDocuments( Dgroup1, Dname1, Attribid);
        }

        public DataTable GetDynamicFields( int? Dgroup1, int? Dname1)
        {
            return DataObj.GetDynamicFields(Dgroup1, Dname1);
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


        public List<Dep_union_dropdown> GetDocGroups(string DocNameId)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocNameChanges(DocNameId);
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

        public List<Dep_union_dropdown> GetDocNames(string DocGroup_Id)
        {
            List<Dep_union_dropdown> dropdown2 = new List<Dep_union_dropdown>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = DataObj.GetDocGroupChanges(DocGroup_Id);
                dt = ds.Tables[1];
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

        public string SingleFileDownload(int? viewid,string mode)
        {
            string Result;
            try
            {
                Result = DataObj.SingleDownload(viewid, mode);
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

        public DataTable showdynamicattributes(int group_gid, string group_type)
        {
            try
            {
                return DataObj.showdynamicattributes(group_gid, group_type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public DataSet ShowAttributes(int? Dgroup1, int? Dname1, int? Attribid)
        {
            try
            {
                return DataObj.ShowAttributes(Dgroup1, Dname1, Attribid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         public DataSet ShowAttributesMultiple(int? Dgroup1, int? Dname1, string Attribid)
         {
             try
             {
                 return DataObj.ShowAttributesMultiple(Dgroup1, Dname1, Attribid);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataSet getmultiplemaildetails(int _empid, string attributegids)
         {
             try
             {
                 return DataObj.getmultiplemaildetails(_empid, attributegids);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         // Insert Mastertable
         public int SaveRequestRetrival(string Requestdate, string Requestno, int noofdcoc, int Employeeid, string Requireddate, string Description, string status, Int64 RequestType)
         {
             int Result;
             try
             {
                 Result = DataObj.SaveRequestRetrival(Requestdate, Requestno, noofdcoc, Employeeid, Requireddate, Description, status, RequestType);

             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return Result;
         }

         //Insert Child Table

         public int ChildSaveRequestRetrival(int Requestgid, int attributeid, int Employeeid)
         {
             int Result;
             try
             {
                 Result = DataObj.ChildSaveRequestRetrival(Requestgid, attributeid,Employeeid);

             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return Result;
         }


         public static List<ViewDocuments_Model.RequestRerival> GetRequestRerival(int Employeeid)
         {
             DataTable tab = new DataTable();
             List<ViewDocuments_Model.RequestRerival> ls_requestretrival = new List<ViewDocuments_Model.RequestRerival>();
             try
             {
                 ViewDocument_Data objproduct = new ViewDocument_Data();
                 tab = objproduct.GetRequestretrival(Employeeid);
                 if (tab.Rows.Count > 0)
                 {
                     foreach (DataRow dr in tab.Rows)
                     {
                         ViewDocuments_Model.RequestRerival requestretrival = new ViewDocuments_Model.RequestRerival();
                         requestretrival.RequestDate = dr["Request_Date"].ToString();
                         requestretrival.NOofDoc = Convert.ToInt32(dr["Noof_Doc"].ToString());
                         requestretrival.Request_No = dr["Request_No"].ToString();
                         requestretrival.Request_From = (dr["Emp_Name"].ToString());
                         requestretrival.RequiredDate =dr["Date_Required"].ToString();
                         requestretrival.Request_Note = dr["Request_Note"].ToString();
                         ls_requestretrival.Add(requestretrival);
                     }
                 }
                 return ls_requestretrival;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public DataSet GetFileTaken(Int64 EmpId, String AttribId)
         {
             DataSet ds = new DataSet();
             ds = DataObj.GetFileTaken(EmpId, AttribId);
             return ds;
         }

         public DataTable getDocnameDocgroup(string Attribid1) {
             DataTable dt = new DataTable();
             try
             {
                 dt = DataObj.getDocnameDocgroupData(Attribid1);
             }
             catch (Exception e)
             {
             }
             return dt;
         }
    }
}
