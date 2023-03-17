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
    public class OrgHierarchy_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        //read the data

        public List<OrgHierarchy_Model> Orgmstdetail()
        {
            try
            {

                List<OrgHierarchy_Model> OrgMastertList = new List<OrgHierarchy_Model>();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetOrgHeirarchy", Con);
                cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "ORG";
                cmd.CommandType = CommandType.StoredProcedure;
               
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                Con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    OrgMastertList.Add
                        (
                        new OrgHierarchy_Model
                        {
                            OrgGId =Convert.ToInt32( dr["orghierarchy_gid"].ToString ()),
                            OrgCode = dr["orghierarchy_code"].ToString(),
                           // CreatedDate = Convert.ToDateTime(dr["Created_Datetime"]),
                            //Grade = Convert.ToInt32(dr["Grade"].ToString()),
                            OrgName = dr["orghierarchy_name"].ToString(),
                           
                        });
                }
                return OrgMastertList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
                Con.Close();
            }
        }

        public List<CreateMaster_Model> GetMasterType()
        {
            try
            {
                List<CreateMaster_Model> DepartmentList = new List<CreateMaster_Model>();
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
                        new CreateMaster_Model
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
        public string SaveOrgHierarchy(List<OrgHierarchy_Model> objModel,Int32 UserId)
        {
            try
            {

                for (int i = 0; i < objModel.Count; i++)
                {
                    DataTable dt = new DataTable();
                    MySqlCommand cmd = new MySqlCommand("SP_OrGMasterSaveUpdate", Con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
                    cmd.Parameters.Add("OrgGId", MySqlDbType.Int32).Value = objModel[i].OrgGId;
                    cmd.Parameters.Add("OrgCode", MySqlDbType.VarChar).Value = objModel[i].OrgCode;
                    cmd.Parameters.Add("OrgName", MySqlDbType.VarChar).Value = objModel[i].OrgName;

                    cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = UserId;
                    Con.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    Con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Con.Close();
            }
            return "Success";
        }

        //update the data
        public CreateMaster_Model DptMstUpdate(CreateMaster_Model Deptmodel)
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
