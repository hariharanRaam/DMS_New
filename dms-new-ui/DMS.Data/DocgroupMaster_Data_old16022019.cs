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
    public class DocgroupMaster_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public List<DocGroupMaster_Model> docgrpmstdetail()
        {
            List<DocGroupMaster_Model> DocgrpList = new List<DocGroupMaster_Model>();

            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetDocGroupMaster", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                DocgrpList.Add
                    (
                    new DocGroupMaster_Model
                    {
                        DgroupId = Convert.ToInt32(dr["Dgroup_Id"].ToString()),
                        DgroupName = dr["Dgroup_Name"].ToString(),
                        Dept_Name = dr["Dept_Name"].ToString(),
                        Dept_Id=Convert.ToInt32(dr["Dept_Id"].ToString()),
                        Unit = dr["Unit"].ToString(),
                        UnitID = Convert.ToInt32(dr["Unit_Id"].ToString()),
                    });
            }

            return DocgrpList;

        }

        public DocGroupMaster_Model DocGroupMstSave(DocGroupMaster_Model Docgrpmdlobj)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_DocGroupSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Insert";
            cmd.Parameters.Add("In_Dgroup_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.DgroupId;
            cmd.Parameters.Add("In_Dgroup_Name", MySqlDbType.VarChar).Value = Docgrpmdlobj.DgroupName;
            cmd.Parameters.Add("In_Dept_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.Dept_Id;
            cmd.Parameters.Add("In_Unit_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.UnitID;
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = Docgrpmdlobj.DgroupCreatedBy;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return Docgrpmdlobj;
        }


        public DocGroupMaster_Model DocGroupMstUpdate(DocGroupMaster_Model Docgrpmdlobj)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_DocGroupSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Update";
            cmd.Parameters.Add("In_Dgroup_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.DgroupId;
            cmd.Parameters.Add("In_Dgroup_Name", MySqlDbType.VarChar).Value = Docgrpmdlobj.DgroupName;
            cmd.Parameters.Add("In_Dept_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.Dept_Id;
            cmd.Parameters.Add("In_Unit_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.UnitID;
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = Docgrpmdlobj.DgroupCreatedBy;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return Docgrpmdlobj;
        }

        public DataTable GetDocGroups(string CommonVal)
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


        public DataTable DeletingDocGroup(int? DGroupID)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_DocGroupSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Dgroup_Id", MySqlDbType.Int32).Value = DGroupID;
            cmd.Parameters.Add("In_Dgroup_Name", MySqlDbType.VarChar).Value = '0';
            cmd.Parameters.Add("In_Dept_Id", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_Unit_Id", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = '0';
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = "Delete";
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return dt;
        }
    }

}
     
  
       
