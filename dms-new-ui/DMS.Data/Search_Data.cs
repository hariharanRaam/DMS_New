using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DMS.Data
{
    public class Search_Data
    {
        public MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public DataSet GetDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, string form)
        {
            DataSet ds = new DataSet();
            try
            {
                //MySqlConnection con = new MySqlConnection();
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDocAttributes", con);
                // MySqlCommand cmd = new MySqlCommand("SP_GetDocLinkGrid1Datas", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = form;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;

                // return dt;
            }
            return ds;
        }

        public DataTable getmastervalue(int LovId, string LovType)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Getlovmastervalues", con);
                cmd.Parameters.Add("In_LovId", MySqlDbType.Int32).Value = LovId;
                cmd.Parameters.Add("In_LovType", MySqlDbType.VarChar).Value = LovType;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataSet GetLinkedDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetLinkedFiles_Search", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int64).Value = Attribid;
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

        public DataSet GetInterlingDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_ShowInterFilingFiles", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
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

        public DataSet GetInterlingLinkDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_ShowInterFilingFiles1_Search", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_AttribID", MySqlDbType.Int32).Value = Attribid;
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

        public DataTable GetDynamicFields(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            DataTable dt = new DataTable();
            try
            {
                //MySqlConnection con = new MySqlConnection();
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDynamicFileds", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataSet GetDropdown(string type, string actiontype)
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

        public DataSet GetDocNameChange(Int64 DocNameId)
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
                ds = RunProcedure(values);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SingleDownload(int attachid)
        {
            string Result = "";
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("ViewDocumentsDownload", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Attachid", MySqlDbType.Int32).Value = attachid;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "main";
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Result = dt.Rows[0][0].ToString();
                con.Close();
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Getmaildetails(int? Indexed_gid, int EmpID, string type)
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Getmaildetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_indexgid", MySqlDbType.Int32).Value = Indexed_gid;
                cmd.Parameters.Add("In_empgid", MySqlDbType.Int32).Value = EmpID;
                cmd.Parameters.Add("In_type", MySqlDbType.VarChar).Value = type;
                con.Open();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
