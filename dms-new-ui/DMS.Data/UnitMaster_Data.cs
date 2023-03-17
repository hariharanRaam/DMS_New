using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Model;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace DMS.Data
{
    public class UnitMaster_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public List<UnitMaster_Model> getallunits()
        {
            try
            {
                List<UnitMaster_Model> UnitList = new List<UnitMaster_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetAllUnits", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    UnitList.Add(new UnitMaster_Model
                    {
                        UnitID = Convert.ToInt32(dr["Unit_Id"].ToString()),
                        UnitCode = dr["Unit_Code"].ToString(),
                        UnitName = dr["Unit_Name"].ToString(),
                        Dept_Id = Convert.ToInt32(dr["Dept_Id"].ToString()),
                        Dept_Name = dr["Dept_Name"].ToString()

                    });
                }
                return UnitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SaveUnit(UnitMaster_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_UnitSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
                cmd.Parameters.Add("In_UnitID", MySqlDbType.VarChar).Value = "0";
                cmd.Parameters.Add("In_UnitCode", MySqlDbType.VarChar).Value = ModelObj.UnitCode;
                cmd.Parameters.Add("In_UnitName", MySqlDbType.VarChar).Value = ModelObj.UnitName;
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

        public object UpdateUnit(UnitMaster_Model ModelObj)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_UnitSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Update";
                cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = ModelObj.UnitID;
                cmd.Parameters.Add("In_UnitCode", MySqlDbType.VarChar).Value = ModelObj.UnitCode;
                cmd.Parameters.Add("In_UnitName", MySqlDbType.VarChar).Value = ModelObj.UnitName;
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

        public DataTable GetDepartments(string CommonVal)
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

        public DataTable DeletingUnit(int? UnitID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_UnitSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Delete";
            cmd.Parameters.Add("In_UnitID", MySqlDbType.Int32).Value = UnitID;
            cmd.Parameters.Add("In_UnitCode", MySqlDbType.VarChar).Value = '0';
            cmd.Parameters.Add("In_UnitName", MySqlDbType.VarChar).Value = '0';
            cmd.Parameters.Add("In_DeptID", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = '0';
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return dt;  
        }
    }
}
