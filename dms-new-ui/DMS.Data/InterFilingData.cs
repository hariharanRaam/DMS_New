using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using DMS.Model;
using System.Data.SqlClient;

namespace DMS.Data
{
    public class InterFilingData
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet GetDropdown(string type, string actiontype, string userId)
        {
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", actiontype);
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                values.Add("p_userid", userId);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet RunProcedure(Dictionary<string, Object> values = null)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand("Pr_get_SingFileddl", con);
                DataSet ds = new DataSet();

                cmd.CommandType = CommandType.StoredProcedure;
                if (values != null)
                {
                    foreach (string key in values.Keys)
                    {
                        cmd.Parameters.AddWithValue(key, values[key]);
                    }
                }
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                //con.Open();
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetDepartmentChange(Int64 Department_id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDept");
                values.Add("p_Department", Department_id);
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitChange(Int64 Unit_Id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetUnit");
                values.Add("p_Department", "0");
                values.Add("p_Unit", Unit_Id);
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", "0");
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocGroupChange(Int64 DocGroup_Id)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDocGroup");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", DocGroup_Id);
                values.Add("p_Docname", "0");
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocNameChange(Int64 DocNameId, string userId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("p_action", "GetDocName");
                values.Add("p_Department", "0");
                values.Add("p_Unit", "0");
                values.Add("p_Docgroup", "0");
                values.Add("p_Docname", DocNameId);
                values.Add("p_userid", userId);
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getdocuments(int? Dgroup1, int? Dname1, string form, string activeflag)
        {
            DataSet ds = new DataSet();
            try
            {
                // con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetInterFiling2", con);
                //MySqlCommand cmd = new MySqlCommand("SP_GetDocLinkGrid1Datas", con);
              //  cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
              //  cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = form;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = activeflag;
                //cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = 0;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveSingleDocDetails(InterFiling_Model ModelObj, HttpPostedFileBase file, int? attachid)
        {
            int Result = 0;
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_SingleDocumentInterfiling", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_FileName", MySqlDbType.VarChar).Value = ModelObj.FileName;
                cmd.Parameters.Add("In_FilePath", MySqlDbType.VarChar).Value = ModelObj.FilePath;
                cmd.Parameters.Add("In_FileExtension", MySqlDbType.VarChar).Value = ModelObj.FileExtension;
                cmd.Parameters.Add("In_FileType", MySqlDbType.VarChar).Value = ModelObj.FileType;
                cmd.Parameters.Add("In_Attributeid", MySqlDbType.VarChar).Value = attachid;
                cmd.Parameters.Add("In_Remarks", MySqlDbType.VarChar).Value = ModelObj.remarks;
                cmd.Parameters.Add("In_Active_flag", MySqlDbType.VarChar).Value = ModelObj.activeflag;
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Result = Convert.ToInt32(dt.Rows[0][0].ToString());

                con.Close();

                if (Result == 1)
                {
                    string filePath = ConfigurationManager.AppSettings["Path"].ToString();
                    string filepath1 = Path.Combine(filePath, Path.GetFileName(file.FileName));
                    file.SaveAs(filepath1);
                }
                return Result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
