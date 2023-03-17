using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Model;

namespace DMS.Data
{
    public class DocNameMaster_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataSet GetAllDocNames(int dnameid_)
        {
            try
            {
                List<DocNameMaster_Model> DocGroupList = new List<DocNameMaster_Model>();
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("sp_getDocNameDetails", Con);
                cmd.Parameters.Add("In_DnameId", MySqlDbType.VarChar).Value = dnameid_;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                Con.Close();
                 
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetmasterDorpdowns(string CommonVal)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_Get_DocGrDropDown", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = CommonVal;
                cmd.Parameters.Add("In_master", MySqlDbType.VarChar).Value = "all";
                cmd.Parameters.Add("In_masteritemid", MySqlDbType.VarChar).Value = "0";
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

        public object SaveDocName(DocNameMaster_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("Sp_DocNameSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = ModelObj.mode;
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = ModelObj.DNameID;
                cmd.Parameters.Add("In_DnameName", MySqlDbType.VarChar).Value = ModelObj.DocName;
                cmd.Parameters.Add("In_Dname_Shortname", MySqlDbType.VarChar).Value = ModelObj.Dname_Shortname;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = ModelObj.DgroupID;
                cmd.Parameters.Add("In_DocPeriod", MySqlDbType.VarChar).Value = ModelObj.DocPeriodAviavablity;
            //    cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitID;
            //    cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = ModelObj.Dept_Id;
                cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = ModelObj.UserID;
                cmd.Parameters.Add("In_Active_Period", MySqlDbType.Int32).Value = ModelObj.AP;
                cmd.Parameters.Add("In_Passive_Period", MySqlDbType.Int32).Value = ModelObj.PP;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                return ModelObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateDocName(DocNameMaster_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("Sp_DocNameSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Update";
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = ModelObj.DNameID;
                cmd.Parameters.Add("In_DnameName", MySqlDbType.VarChar).Value = ModelObj.DocName;
                cmd.Parameters.Add("In_Dname_Shortname", MySqlDbType.VarChar).Value = ModelObj.Dname_Shortname;
                cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = ModelObj.DgroupID;
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitID;
                cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = ModelObj.Dept_Id;
                cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = ModelObj.UserID;               
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                return ModelObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DeletingDocName(int? DNameID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("Sp_DocNameSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Delete";
            cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = DNameID;
            cmd.Parameters.Add("In_DnameName", MySqlDbType.VarChar).Value = '0';
            cmd.Parameters.Add("In_Dname_Shortname", MySqlDbType.VarChar).Value ='0';
            cmd.Parameters.Add("In_DgroupID", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = '0';
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return dt;
        }

        public DataSet unit(int? masteritemid, string master)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("SP_GetMasterDropDowns", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_master", MySqlDbType.VarChar).Value = master;
                cmd.Parameters.Add("In_masteritemid", MySqlDbType.VarChar).Value = masteritemid;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                Con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
