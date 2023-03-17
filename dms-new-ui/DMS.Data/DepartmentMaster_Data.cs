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
    public class DepartmentMaster_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        //read the data
        public List<DepartmentMaster_Model> deptmstdetail(DepartmentMaster_Model Deptmodel)
        {
            try
            {
                
                List<DepartmentMaster_Model> DepartmentList = new List<DepartmentMaster_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetOrgHeirarchy", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("Org_code", MySqlDbType.VarChar).Value =Deptmodel.MasterTypeId;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    DepartmentList.Add
                        (
                        new DepartmentMaster_Model
                        {
                            Id = dr["master_code"].ToString(),
                            Name = dr["master_name"].ToString(),
                           // CreatedDate = Convert.ToDateTime(dr["Created_Datetime"]),
                            //Grade = Convert.ToInt32(dr["Grade"].ToString()),
                            Createdby = dr["Created_By"].ToString(),
                            MasterTypeId=Deptmodel.MasterTypeId.ToString ()
                        });
                }
                return DepartmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DepartmentMaster_Model> GetMasterType()
        {
            try
            {
                List<DepartmentMaster_Model> DepartmentList = new List<DepartmentMaster_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("select orghierarchy_code,orghierarchy_name from dms_mst_torghirerachy", Con);
                cmd.CommandType = CommandType.Text;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    DepartmentList.Add
                        (
                        new DepartmentMaster_Model
                        {
                            MasterTypeId  = dr["orghierarchy_code"].ToString(),
                            MasterTypeName = dr["orghierarchy_name"].ToString(),
                           // CreatedDate = Convert.ToDateTime(dr["orghierarchy_name"].ToString()),
                            //Grade = Convert.ToInt32(dr["Grade"].ToString()),
                            //Createdby = dr["Dept_Created_By"].ToString(),

                        });
                }
                return DepartmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //insert the data
        public DepartmentMaster_Model DptMstSave(DepartmentMaster_Model Deptmodel)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_MasterSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
                cmd.Parameters.Add("Master_Id", MySqlDbType.VarChar).Value = Deptmodel.Id ;
                cmd.Parameters.Add("Master_Name", MySqlDbType.VarChar).Value = Deptmodel.Name;
                cmd.Parameters.Add("ParentCode", MySqlDbType.VarChar).Value = Deptmodel.MasterTypeId ;
                cmd.Parameters.Add("Depend_code", MySqlDbType.VarChar).Value = Deptmodel.DependId ;
                cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = Deptmodel.Createdby;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deptmodel;
        }

        //update the data
        public DepartmentMaster_Model DptMstUpdate(DepartmentMaster_Model Deptmodel)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_MasterSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Update";
                cmd.Parameters.Add("Master_Id", MySqlDbType.VarChar).Value = Deptmodel.Id;
                cmd.Parameters.Add("Master_Name", MySqlDbType.VarChar).Value = Deptmodel.Name;
                cmd.Parameters.Add("ParentCode", MySqlDbType.VarChar).Value = Deptmodel.MasterTypeId;
                cmd.Parameters.Add("Depend_code", MySqlDbType.VarChar).Value = Deptmodel.DependId;
                cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = Deptmodel.Createdby;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deptmodel;
        }

        //delete the data
        public DataTable DeletingDepartment(string ID, string MasterTypeId)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_MasterSaveUpdateDelete", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Delete";
                cmd.Parameters.Add("Master_Id", MySqlDbType.VarChar).Value = ID;
                cmd.Parameters.Add("ParentCode", MySqlDbType.VarChar).Value = MasterTypeId;
                cmd.Parameters.Add("Master_Name", MySqlDbType.VarChar).Value = MasterTypeId;
                cmd.Parameters.Add("Depend_code", MySqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = "0";
                
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
