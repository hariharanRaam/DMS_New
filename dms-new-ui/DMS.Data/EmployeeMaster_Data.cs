using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using DMS.Model;


namespace DMS.Data
{
    public class EmployeeMaster_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public EmployeeMaster_Model EmpMstDtl(int _userid)
        {
            try
            {
                EmployeeMaster_Model EmployeeList = new EmployeeMaster_Model();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetAllEmployeeDetails", Con);
                cmd.Parameters.Add("In_user_id",MySqlDbType.Int32).Value = _userid;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                //    EmployeeList.Add(new EmployeeMaster_Model
                 //       {
                            EmployeeList.EmployeeID = Convert.ToInt32(dr["Emp_ID"].ToString());
                            EmployeeList.EmployeeCode = dr["Emp_Code"].ToString();
                            EmployeeList.EmployeeName = dr["Emp_Name"].ToString();
                            EmployeeList.Grade = dr["Grade_Name"].ToString();
                            EmployeeList.GradeID = Convert.ToInt32(dr["Grade_ID"].ToString());
                            EmployeeList.TitleID = Convert.ToInt32(dr["Title_Id"].ToString());
                            EmployeeList.Title = dr["Title_Name"].ToString();
                          //  Dept_Id = Convert.ToInt32(dr["Dept_ID"].ToString()),
                          //  Dept_Name = dr["Dept_Name"].ToString(),
                          //  UnitID = Convert.ToInt32(dr["Unit_ID"].ToString()),
                         //   Unit = dr["Unit_Name"].ToString(),
                            //DOJ = Convert.ToDateTime(dr["Emp_DOJ"].ToString()),
                            EmployeeList.DOJ = dr["Emp_DOJ"].ToString();
                           EmployeeList.Address = dr["Emp_Address"].ToString();
                            EmployeeList.City = dr["City_Name"].ToString();
                            EmployeeList.CityID = Convert.ToInt32(dr["City_ID"].ToString());
                            EmployeeList.Pin = dr["Pin_Code"].ToString();
                            EmployeeList.StateID = Convert.ToInt32(dr["State_ID"].ToString());
                            EmployeeList.State = dr["State_Name"].ToString();
                            EmployeeList.RegionID = Convert.ToInt32(dr["Region_ID"].ToString());
                            EmployeeList.Region = dr["Region_Name"].ToString();
                            EmployeeList.MobileNo = dr["Emp_MobileNo"].ToString();
                            EmployeeList.EmailID = dr["Emp_EmailId"].ToString();
                            EmployeeList.LanNo = dr["Emp_LanNo"].ToString();
                            EmployeeList.Password = dr["Emp_Password"].ToString();
                            EmployeeList.TypeID = Convert.ToInt32(dr["EmpType_ID"].ToString());
                            EmployeeList.TypeName = dr["EmpType_Name"].ToString();
                            EmployeeList.UserGroupID = Convert.ToInt32(dr["Usergroup_Gid"].ToString());
                            EmployeeList.UserGroup = dr["Usergroup_Name"].ToString();
                            EmployeeList.Status = dr["Active"].ToString();
                            EmployeeList.OrgLevelMax = dr["OrgLevelMax"].ToString();
                   //     });
                }
                return EmployeeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<EmployeeMaster_Model> EmpMstDtllist(EmployeeMaster_Model EDtlObj)
        {
            try
            {
                List<EmployeeMaster_Model> EmployeeList = new List<EmployeeMaster_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetAllEmployeeDetailsList", Con);
                cmd.Parameters.Add("In_Employee_Name", MySqlDbType.VarChar).Value = EDtlObj.EmployeeName;
                cmd.Parameters.Add("In_Employee_Mobile", MySqlDbType.VarChar).Value = EDtlObj.MobileNo;
                cmd.Parameters.Add("In_Employee_UGI", MySqlDbType.Int32).Value = EDtlObj.UserGroupID;
                cmd.Parameters.Add("In_MaxOrglvl", MySqlDbType.Text).Value = EDtlObj.OrgLevelMax;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    EmployeeList.Add(new EmployeeMaster_Model
                    {
                        EmployeeID = Convert.ToInt32(dr["Emp_ID"].ToString()),
                        EmployeeCode = dr["Emp_Code"].ToString(),
                        EmployeeName = dr["Emp_Name"].ToString(),
                        Grade = dr["Grade_Name"].ToString(),
                        GradeID = Convert.ToInt32(dr["Grade_ID"].ToString()),
                        TitleID = Convert.ToInt32(dr["Title_Id"].ToString()),
                        Title = dr["Title_Name"].ToString(),
                        //  Dept_Id = Convert.ToInt32(dr["Dept_ID"].ToString()),
                        //  Dept_Name = dr["Dept_Name"].ToString(),
                        //  UnitID = Convert.ToInt32(dr["Unit_ID"].ToString()),
                        //   Unit = dr["Unit_Name"].ToString(),
                        //DOJ = Convert.ToDateTime(dr["Emp_DOJ"].ToString()),
                        DOJ = dr["Emp_DOJ"].ToString(),
                        Address = dr["Emp_Address"].ToString(),
                        City = dr["City_Name"].ToString(),
                        CityID = Convert.ToInt32(dr["City_ID"].ToString()),
                        Pin = dr["Pin_Code"].ToString(),
                        StateID = Convert.ToInt32(dr["State_ID"].ToString()),
                        State = dr["State_Name"].ToString(),
                        RegionID = Convert.ToInt32(dr["Region_ID"].ToString()),
                        Region = dr["Region_Name"].ToString(),
                        MobileNo = dr["Emp_MobileNo"].ToString(),
                        EmailID = dr["Emp_EmailId"].ToString(),
                        LanNo = dr["Emp_LanNo"].ToString(),
                        Password = dr["Emp_Password"].ToString(),
                        TypeID = Convert.ToInt32(dr["EmpType_ID"].ToString()),
                        TypeName = dr["EmpType_Name"].ToString(),
                        UserGroupID = Convert.ToInt32(dr["Usergroup_Gid"].ToString()),
                        UserGroup = dr["Usergroup_Name"].ToString(),
                        Status = dr["Active"].ToString(),
                        OrgLevelMax = dr["OrgLevelMax"].ToString()
                    });
                }

                return EmployeeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeMaster_Model EmpMstUpdate(EmployeeMaster_Model EDtlObj)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_EmployeeSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Update";
                cmd.Parameters.Add("In_EmpID", MySqlDbType.VarChar).Value = EDtlObj.EmployeeID;
                cmd.Parameters.Add("In_EmpCode", MySqlDbType.VarChar).Value = EDtlObj.EmployeeCode;
                cmd.Parameters.Add("In_EmpName", MySqlDbType.VarChar).Value = EDtlObj.EmployeeName;
                cmd.Parameters.Add("In_EmpTitle", MySqlDbType.VarChar).Value = EDtlObj.TitleID;
                cmd.Parameters.Add("In_GradeID", MySqlDbType.Int32).Value = EDtlObj.GradeID;
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = EDtlObj.Dept_Id;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = EDtlObj.UnitID;
               // string dt = System.DateTime.Now.ToString("dd/MM/yyyy");
                //string strdt = "";
                //int strdtCount = EDtlObj.DOJ.Length;
                if (EDtlObj.DOJ == null || EDtlObj.DOJ == "")
                {
                    cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = DateTime.Now.ToString("dd/MM/yyyy"); ;
                }
                else
                {
                   // string strdt = EDtlObj.DOJ.Substring(4, 12);
                    cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = EDtlObj.DOJ;
                }
                if (EDtlObj.Address == null)
                {
                    EDtlObj.Address = "";
                }
                if (EDtlObj.Pin == null)
                {
                    EDtlObj.Pin = "0";
                }
                if (EDtlObj.LanNo == null)
                {
                    EDtlObj.LanNo = "";
                }
              //  cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = Convert.ToDateTime(strdt).ToString("dd/MM/yyyy");
                cmd.Parameters.Add("In_Address", MySqlDbType.VarChar).Value = EDtlObj.Address;
                cmd.Parameters.Add("In_CityID", MySqlDbType.Int32).Value = EDtlObj.CityID;
                cmd.Parameters.Add("In_PinID", MySqlDbType.Int32).Value = EDtlObj.Pin;
                cmd.Parameters.Add("In_StateID", MySqlDbType.Int32).Value = EDtlObj.StateID;
                cmd.Parameters.Add("In_RegionID", MySqlDbType.Int32).Value = EDtlObj.RegionID;
                cmd.Parameters.Add("In_MobileNo", MySqlDbType.VarChar).Value = EDtlObj.MobileNo;
                cmd.Parameters.Add("In_LanNo", MySqlDbType.VarChar).Value = EDtlObj.LanNo;
                cmd.Parameters.Add("In_EmailID", MySqlDbType.VarChar).Value = EDtlObj.EmailID;
                cmd.Parameters.Add("In_CreatedBy", MySqlDbType.Int32).Value = EDtlObj.UserID;
                cmd.Parameters.Add("In_EmpType", MySqlDbType.VarChar).Value = EDtlObj.TypeID;
                cmd.Parameters.Add("In_Password", MySqlDbType.VarChar).Value = EDtlObj.Password;
                cmd.Parameters.Add("In_UserGroupID", MySqlDbType.Int32).Value = EDtlObj.UserGroupID;
                cmd.Parameters.Add("In_status", MySqlDbType.VarChar).Value = EDtlObj.Status;
                cmd.Parameters.Add("In_OrgLevelMax", MySqlDbType.VarChar).Value = EDtlObj.OrgLevelMax;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    EDtlObj.EmployeeID = Convert.ToInt32(dr["id"].ToString());
                }
                return EDtlObj;
            }
            catch (Exception ex)
            {
                throw ex;               
            }
        }

        public DataTable GetDepartments(string CommonVal)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetEmpDropdowns", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = CommonVal;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SaveEmployee(EmployeeMaster_Model EDtlObj)
        {
            try
            {
                Con.Close();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_EmployeeSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
                cmd.Parameters.Add("In_EmpID", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_EmpCode", MySqlDbType.VarChar).Value = EDtlObj.EmployeeCode;
                cmd.Parameters.Add("In_EmpName", MySqlDbType.VarChar).Value = EDtlObj.EmployeeName;
                cmd.Parameters.Add("In_EmpTitle", MySqlDbType.VarChar).Value = EDtlObj.TitleID;
                cmd.Parameters.Add("In_GradeID", MySqlDbType.Int32).Value = EDtlObj.GradeID;
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = EDtlObj.Dept_Id;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = EDtlObj.UnitID;
                //cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = EDtlObj.DOJ;
                if (EDtlObj.DOJ == null || EDtlObj.DOJ == "")
                {
                    cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = DateTime.Now.ToString("dd/MM/yyyy"); ;
                }
                else {
                    string strdt = EDtlObj.DOJ.Substring(4, 12);
                    cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = Convert.ToDateTime(strdt).ToString("dd/MM/yyyy");
                }
                if (EDtlObj.Address == null) {
                    EDtlObj.Address = "";
                }
                if (EDtlObj.Pin == null) {
                    EDtlObj.Pin = "0";
                }
                if (EDtlObj.LanNo == null)
                {
                    EDtlObj.LanNo = "";
                }
                cmd.Parameters.Add("In_Address", MySqlDbType.VarChar).Value = EDtlObj.Address;
                cmd.Parameters.Add("In_CityID", MySqlDbType.Int32).Value = EDtlObj.CityID;
                cmd.Parameters.Add("In_PinID", MySqlDbType.Int32).Value = EDtlObj.Pin;
                cmd.Parameters.Add("In_StateID", MySqlDbType.Int32).Value = EDtlObj.StateID;
                cmd.Parameters.Add("In_RegionID", MySqlDbType.Int32).Value = EDtlObj.RegionID;
                cmd.Parameters.Add("In_MobileNo", MySqlDbType.VarChar).Value = EDtlObj.MobileNo;
                cmd.Parameters.Add("In_LanNo", MySqlDbType.VarChar).Value = EDtlObj.LanNo;
                cmd.Parameters.Add("In_EmailID", MySqlDbType.VarChar).Value = EDtlObj.EmailID;
                cmd.Parameters.Add("In_CreatedBy", MySqlDbType.Int32).Value = EDtlObj.UserID;
                cmd.Parameters.Add("In_EmpType", MySqlDbType.VarChar).Value = EDtlObj.TypeID;
                cmd.Parameters.Add("In_Password", MySqlDbType.VarChar).Value = EDtlObj.Password;
                cmd.Parameters.Add("In_UserGroupID", MySqlDbType.Int32).Value = EDtlObj.UserGroupID;
                cmd.Parameters.Add("In_status", MySqlDbType.VarChar).Value = EDtlObj.Status;
                cmd.Parameters.Add("In_OrgLevelMax", MySqlDbType.VarChar).Value = EDtlObj.OrgLevelMax;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    EDtlObj.EmployeeID = Convert.ToInt32(dr["id"].ToString());
                }
                return EDtlObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DeletingDepartment(int? EmployeeID)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_EmployeeSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Delete";
                cmd.Parameters.Add("In_EmpID", MySqlDbType.VarChar).Value = EmployeeID;
                cmd.Parameters.Add("In_EmpCode", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_EmpName", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_EmpTitle", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_GradeID", MySqlDbType.Int32).Value = "0";
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = "0";
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_DOJ", MySqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("In_Address", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_CityID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_PinID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_StateID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_RegionID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_MobileNo", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_LanNo", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_EmailID", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_CreatedBy", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_EmpType", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_Password", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_UserGroupID", MySqlDbType.Int32).Value = "0";
                cmd.Parameters.Add("In_status", MySqlDbType.VarChar).Value = "I";
                cmd.Parameters.Add("In_OrgLevelMax", MySqlDbType.VarChar).Value = "0";
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeMaster_Model GetEmporglvl(string empid) {
            EmployeeMaster_Model outobj = new EmployeeMaster_Model();
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetEmpOrglvls", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Emp_ID", MySqlDbType.Int64).Value = Int64.Parse(empid);
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    outobj.OrgLevelMax = dr["OrgLevelMax"].ToString();
                }
            }catch(Exception e){
                throw e;
            }
            return outobj;
        }

        public DataSet EmployeeMasterRead()
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                Con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetAllEmployeeDetails", Con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                Con.Close();
            }
            catch (Exception ex)
            {
                //ex;
            }
            finally
            {
                Con.Close();
            }
            return ds;
        }

    }
}

