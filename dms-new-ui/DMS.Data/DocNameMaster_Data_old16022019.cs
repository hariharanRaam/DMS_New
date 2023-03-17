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
        public List<DocNameMaster_Model> GetAllDocNames()
        {
            try
            {
                List<DocNameMaster_Model> DocGroupList = new List<DocNameMaster_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetAllDocNames", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    DocGroupList.Add(new DocNameMaster_Model
                    {
                        DNameID = Convert.ToInt32(dr["Dname_Id"].ToString()),
                        DocName = dr["Dname_Name"].ToString(),
                        Dept_Id = Convert.ToInt32(dr["Dept_Id"].ToString()),
                        Dept_Name = dr["Dept_Name"].ToString(),
                        UnitID = Convert.ToInt32(dr["Unit_Id"].ToString()),
                        Unit = dr["Unit_Name"].ToString(),
                        DgroupID = Convert.ToInt32(dr["Dgroup_Id"].ToString()),
                        DgroupName = dr["Dgroup_Name"].ToString(),
                    });
                }
                return DocGroupList;
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
                MySqlCommand cmd = new MySqlCommand("SP_GetMasterDropDowns", Con);
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
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
                cmd.Parameters.Add("In_DnameID", MySqlDbType.Int32).Value = '0';
                cmd.Parameters.Add("In_DnameName", MySqlDbType.VarChar).Value = ModelObj.DocName;
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
