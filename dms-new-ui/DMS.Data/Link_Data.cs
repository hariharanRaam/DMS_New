using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DMS.Data
{
    public class Link_Data
    {
        //creating connection string for accessing and modifying database tables datas. 
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

        public DataSet getdocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, string form)
        {
            //creating dataset for storing collection of tables.
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GetDocLinkGrid1Datas", con);
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = DeptID1;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = Unit1;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = Dgroup1;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = Dname1;
                cmd.Parameters.Add("In_Dynamicfields", MySqlDbType.VarChar).Value = form;
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

        public int AttachFiles(string attachid1, string attachid2)
        {
            int Result;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_GroupingDocuments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_GroupId1", MySqlDbType.VarChar).Value = attachid1;
                cmd.Parameters.Add("In_GroupId2", MySqlDbType.VarChar).Value = attachid2;
                Result = cmd.ExecuteNonQuery();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeAttachFiles(string attachid1, string attachid2)
        {
            int Result;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SP_DeGroupingDocuments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_GroupId1", MySqlDbType.VarChar).Value = attachid1;
                cmd.Parameters.Add("In_GroupId2", MySqlDbType.VarChar).Value = attachid2;
                Result = cmd.ExecuteNonQuery();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        //public string validate(string retval, string retval1)
        //{
        //    string Result;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        con.Open();
        //        MySqlCommand cmd = new MySqlCommand("SP_GroupingDocValidation", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("In_GroupId1", MySqlDbType.VarChar).Value = retval;
        //        cmd.Parameters.Add("In_GroupId2", MySqlDbType.VarChar).Value = retval1;
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da.Fill(dt);
        //        con.Close();
        //        Result = dt.Rows[0]["result"].ToString();
        //        return Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
