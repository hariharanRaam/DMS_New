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
        public DocGroupMaster_Model docgrpmstdetail(int DgroupId)
        {
            DocGroupMaster_Model DocgrpList = new DocGroupMaster_Model();

            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetDocGroupMaster", Con);
            cmd.Parameters.Add("In_DgroupId", MySqlDbType.Int32).Value = DgroupId;
            cmd.CommandType = CommandType.StoredProcedure;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            foreach (DataRow dr in dt.Rows)
            {
               
                        DocgrpList.DgroupId = Convert.ToInt32(dr["Dgroup_Id"].ToString());
                        DocgrpList.DgroupName = dr["Dgroup_Name"].ToString();
                        DocgrpList.Dgroup_shortname = dr["Docgroup_shortname"].ToString();
                       // DocgrpList.Org_Level1 = dr["Dept_Name"].ToString();
                        DocgrpList.Org_Level1code = dr["Org_Level1"].ToString();
                        DocgrpList.Org_Level2code = dr["Org_Level2"].ToString();
                        DocgrpList.Org_Level3code = dr["Org_Level3"].ToString();
                        DocgrpList.Org_Level4code = dr["Org_Level4"].ToString();
            }

            return DocgrpList;

        }

        public DocGroupMaster_Model DocGroupMstSave(DocGroupMaster_Model Docgrpmdlobj)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_DocGroupSaveUpdateDelete", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = Docgrpmdlobj.mode;
            cmd.Parameters.Add("In_Dgroup_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.DgrpId;
            cmd.Parameters.Add("In_Dgroup_Name", MySqlDbType.VarChar).Value = Docgrpmdlobj.DgroupName;
            cmd.Parameters.Add("In_Docgroup_shortname", MySqlDbType.VarChar).Value = Docgrpmdlobj.Dgroup_shortname;
            cmd.Parameters.Add("In_OrgLevel1_code", MySqlDbType.VarChar).Value = Docgrpmdlobj.Org_Level1;
            cmd.Parameters.Add("In_OrgLevel2_code", MySqlDbType.VarChar).Value = Docgrpmdlobj.Org_Level2;
            cmd.Parameters.Add("In_OrgLevel3_code", MySqlDbType.VarChar).Value = Docgrpmdlobj.Org_Level3;
            cmd.Parameters.Add("In_OrgLevel4_code", MySqlDbType.VarChar).Value = Docgrpmdlobj.Org_Level4;
            cmd.Parameters.Add("In_UserID", MySqlDbType.VarChar).Value = Docgrpmdlobj.DgroupCreatedBy;
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
            cmd.Parameters.Add("In_Docgroup_shortname", MySqlDbType.VarChar).Value = Docgrpmdlobj.Dgroup_shortname;
            cmd.Parameters.Add("In_Dept_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.Dept_Id;
            cmd.Parameters.Add("In_Unit_Id", MySqlDbType.Int32).Value = Docgrpmdlobj.UnitID;
            cmd.Parameters.Add("In_UserID", MySqlDbType.Int32).Value = Docgrpmdlobj.DgroupCreatedBy;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return Docgrpmdlobj;
        }

        public DataTable GetDocGroups(string parentcode,string dependcode)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetMasterDropDowns", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("parentcode", MySqlDbType.VarChar).Value = parentcode;
            cmd.Parameters.Add("dependcode", MySqlDbType.VarChar).Value = dependcode;
            //cmd.Parameters.Add("In_masteritemid", MySqlDbType.VarChar).Value = "0";
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return dt;
        }

        public List<DataTable> GetDocGroupsNew(string parentcode, string dependcode, int maxorg, int currentorgnumber)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            List<DataTable> resulttable = new List<DataTable>();
            if (maxorg == 4) {
                if (currentorgnumber == 4)
                {
                    dt3 = getresult(parentcode, dependcode, 2);
                    dt2 = getresult(parentcode, getconcatdependcode(dt3), 2);
                    dt1 = getresult(parentcode, getconcatdependcode(dt2), 2);
                   // dt4 = getresult(parentcode, getconcatdependcode(dt2), 2);
                }
                else if (currentorgnumber == 3) {
                    dt4 = getresult(parentcode, dependcode, 1);
                    dt3 = getresult(parentcode, getconcatdependcode(dt4), 2);
                    dt2 = getresult(parentcode, getconcatdependcode(dt3), 2);
                    dt1 = getresult(parentcode, getconcatdependcode(dt2), 2);
                }
                else if (currentorgnumber == 2) {
                    dt3 = getresult(parentcode, dependcode, 1);
                    dt4 = getresult(parentcode, getconcatdependcode(dt3), 1);
                    dt2 = getresult(parentcode, getconcatdependcode(dt3), 2);
                    dt1 = getresult(parentcode, getconcatdependcode(dt2), 2);
                }
                else
                {
                    dt2 = getresult(parentcode, dependcode, 1);
                    dt3 = getresult(parentcode, getconcatdependcode(dt2), 1);
                    dt4 = getresult(parentcode, getconcatdependcode(dt3), 1);
                    //dt4 = getresult(parentcode, getconcatdependcode(dt3), 2);
                }
            }
            else if (maxorg == 3)
            {
                if (currentorgnumber == 3)
                {
                    dt2 = getresult(parentcode, dependcode, 2);
                    dt1 = getresult(parentcode, getconcatdependcode(dt2), 2);
                    //dt3 = getresult(parentcode, getconcatdependcode(dt2), 2);
                }
                else if (currentorgnumber == 2)
                {
                    dt3 = getresult(parentcode, dependcode, 1);
                    dt2 = getresult(parentcode, getconcatdependcode(dt3), 2);
                    dt1 = getresult(parentcode, getconcatdependcode(dt2), 2);
                }
                else
                {
                    dt2 = getresult(parentcode, dependcode, 1);
                    dt3 = getresult(parentcode, getconcatdependcode(dt2), 1);
                    //dt4 = getresult(parentcode, getconcatdependcode(dt3), 2);
                }
            }
            else {
                if (currentorgnumber == 2)
                {
                    dt1 = getresult(parentcode, dependcode, 2);
                }
                else
                {
                    dt2 = getresult(parentcode, dependcode, 1);
                    //dt4 = getresult(parentcode, getconcatdependcode(dt3), 2);
                }
            }
            resulttable.Add(dt1);
            resulttable.Add(dt2);
            resulttable.Add(dt3);
            resulttable.Add(dt4);
            return resulttable;
        }

        public string getconcatdependcode(DataTable dt) {
            string dependcode = "";
            for (int u = 0; u < dt.Rows.Count; u++) {
                if (dependcode.ToString().Length > 0)
                {
                    dependcode = dependcode + " , '" + dt.Rows[u]["mastercode"].ToString() + "' ";
                }
                else {
                    dependcode =  "'" + dt.Rows[u]["mastercode"].ToString() + "' ";
                } 
            }
            return dependcode;
        }

        public DataTable getresult(string parentcode, string dependcode, int qrymode_)
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_GetMasterDropDownsNew", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("parentcode", MySqlDbType.VarChar).Value = parentcode;
            cmd.Parameters.Add("dependcode", MySqlDbType.Text).Value = dependcode;
            cmd.Parameters.Add("qrymode", MySqlDbType.Int32).Value = qrymode_;
            Con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Con.Close();
            return dt;
        }

        public DataTable GetDoclabels()
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SP_Get_DynamicLabels", Con);
            cmd.CommandType = CommandType.StoredProcedure;           
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
            cmd.Parameters.Add("In_Docgroup_shortname", MySqlDbType.VarChar).Value ='0';
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

        public DataSet DocGroupGridread() {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                Con.Open();
                 MySqlCommand cmd = new MySqlCommand("SP_GetDocGroupMaster", Con);
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
     
  
       
