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
    public class UserGroups_Data
    {
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        public DataTable GetUserGroups()
        {
            DataTable tab = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetUserGroups", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("In_Action", MySqlDbType.VarChar).Value = CommonVal;
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

        public DataTable GetRoleMappingData(int usergrp_gid)
        {

            DataTable tab = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("pr_UserMapping", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_usergrp_gid", MySqlDbType.Int32).Value = usergrp_gid;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(tab);
                Con.Close();
                return tab;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable getScreenData_Data(int screen_id, string screen_name, int user_gcode)
        {

            DataTable tab = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("pr_GetScreenData", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("In_Screen_Id", MySqlDbType.Int32).Value = screen_id;
                cmd.Parameters.Add("In_Screen_Name", MySqlDbType.VarChar).Value = screen_name;
                cmd.Parameters.Add("In_Emp_Gid", MySqlDbType.Int32).Value = user_gcode;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(tab);
                Con.Close();
                return tab;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetTreeview(int usergroup_gid)
        {
            DataTable tab = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("SP_GetTreeView", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_usergroup_gid", MySqlDbType.VarChar).Value = usergroup_gid;
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


        public string[] CreateUserGroups(int usergroup_gid, string usergroup_name, string action_by,string usergroup_code)
        {
            string[] result = { };
            DataTable tab = new DataTable();
            //DataConnection con = new DataConnection();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("in_usergroup_gid", usergroup_gid);
                values.Add("in_usergroup_name", usergroup_name);
                values.Add("in_usergroup_code", usergroup_code);
                values.Add("in_action_by", action_by);
                values.Add("out_usergroup_gid", "out");
                values.Add("out_msg", "out");
                values.Add("out_result", "out");
                result = RunDmlProcArrayList("SP_SetUserGroup", values);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] RunDmlProcArrayList(string command, Dictionary<string, Object> values = null)
        {
            int rowsChanged = 0;
            MySqlCommand cmd = new MySqlCommand(command,Con);
           // cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            string retmsg = string.Empty;
            string retresult = string.Empty;
            string retid = string.Empty;
            try
            {
                if (values != null)
                {
                    foreach (string key in values.Keys)
                    {
                        if (values[key] == "out")
                        {
                            cmd.Parameters.Add(key, MySqlDbType.VarChar);
                            cmd.Parameters[key].Direction = ParameterDirection.Output;

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(key, values[key]);
                        }
                    }
                }
                Con.Open();
                rowsChanged = cmd.ExecuteNonQuery();
                retmsg = (string)cmd.Parameters["out_msg"].Value;
                retresult = (string)cmd.Parameters["out_result"].Value;
                if (cmd.Parameters["out_usergroup_gid"].Value.ToString() != "")
                {
                    retid = (string)cmd.Parameters["out_usergroup_gid"].Value;
                }
                else
                {
                    retid = "0";
                }
                string[] returnvalues = { retmsg, retresult, retid };
                Con.Close();
                return returnvalues;
            }
            catch (Exception ex)
            {
                Con.Close();
                string[] returnvalues = { };
                return returnvalues;

            }
        }
        public string[] SetRightsFlag(int menu_gid, int usergroup_gid, int rights_flag)
        {
            string[] result = { };
            DataTable tab = new DataTable();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("in_menu_gid", menu_gid);
                values.Add("in_usergroup_gid", usergroup_gid);
                values.Add("in_rights_flag", rights_flag);
                values.Add("out_msg", "out");
                values.Add("out_result", "out");
                result = RunDmlProcArray("SP_InsertRights", values);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] DeleteUserGroups(int? usergroup_gid)
        {
            string[] result = { };
            DataTable tab = new DataTable();
            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();
                values.Add("in_usergroup_gid", usergroup_gid);
                values.Add("out_msg", "out");
                result = RunDelProcArray("SP_Deleteusergroup", values);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] RunDelProcArray(string command, Dictionary<string, Object> values = null)
        {
            int rowsChanged = 0;
            MySqlCommand cmd = new MySqlCommand(command, Con);
            cmd.CommandType = CommandType.StoredProcedure;
            string retmsg = string.Empty;
            try
            {
                if (values != null)
                {
                    foreach (string key in values.Keys)
                    {
                        if (values[key] == "out")
                        {
                            cmd.Parameters.Add(key, MySqlDbType.VarChar);
                            cmd.Parameters[key].Direction = ParameterDirection.Output;

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(key, values[key]);
                        }
                    }
                }
                Con.Open();
                rowsChanged = cmd.ExecuteNonQuery();
                retmsg = (string)cmd.Parameters["out_msg"].Value;
                string[] returnvalues = { retmsg};
                Con.Close();
                return returnvalues;
            }
            catch (Exception ex)
            {
                string[] returnvalues = { };
                return returnvalues;
            }
        }


        public string[] RunDmlProcArray(string command, Dictionary<string, Object> values = null)
        {
            int rowsChanged = 0;
            MySqlCommand cmd = new MySqlCommand(command,Con);
            cmd.CommandType = CommandType.StoredProcedure;
            string retmsg = string.Empty;
            string retresult = string.Empty;
            try
            {
                if (values != null)
                {
                    foreach (string key in values.Keys)
                    {
                        if (values[key] == "out")
                        {
                            cmd.Parameters.Add(key, MySqlDbType.VarChar);
                            cmd.Parameters[key].Direction = ParameterDirection.Output;

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(key, values[key]);
                        }
                    }
                }
                Con.Open();
                rowsChanged = cmd.ExecuteNonQuery();
                retmsg = (string)cmd.Parameters["out_msg"].Value;
                retresult = (string)cmd.Parameters["out_result"].Value;
                string[] returnvalues = { retmsg, retresult };
                Con.Close();
                return returnvalues;
            }
            catch (Exception ex)
            {
                string[] returnvalues = { };
                return returnvalues;
            }
        }

        public DataTable GetMenu(int userID, string menutype, int menu_gid)
        {
            DataTable tab = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SP_Getmenu", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_usergroup_gid", MySqlDbType.Int32).Value = userID;
                cmd.Parameters.Add("menu_type", MySqlDbType.VarChar).Value = menutype;
                cmd.Parameters.Add("menu_gid", MySqlDbType.Int32).Value = menu_gid;
                Con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(tab);
                Con.Close();
                return tab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveUserRoeMapData(UserGroups_Model.RoleMappings rolemap)
        {
            int res = 0;
            try
            {
                int rowsChanged = 0;
                MySqlCommand cmd = new MySqlCommand("SP_Save_UserRoles", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_role_code", MySqlDbType.Int32).Value = rolemap.role_code;
                cmd.Parameters.Add("in_activity_code", MySqlDbType.Int32).Value = rolemap.menu_gid;
                cmd.Parameters.Add("in_rolemenu_rowid", MySqlDbType.Int32).Value = rolemap.rolemenu_rowid;
                cmd.Parameters.Add("in_add_perm", MySqlDbType.Int32).Value = rolemap.add_perm;
                cmd.Parameters.Add("in_mod_perm", MySqlDbType.Int32).Value = rolemap.mod_perm;
                cmd.Parameters.Add("in_view_perm", MySqlDbType.Int32).Value = rolemap.view_perm;
                cmd.Parameters.Add("in_auth_perm", MySqlDbType.Int32).Value = rolemap.auth_perm;
                cmd.Parameters.Add("in_inactivate_perm", MySqlDbType.Int32).Value = rolemap.inactivate_perm;
                cmd.Parameters.Add("in_print_perm", MySqlDbType.Int32).Value = rolemap.print_perm;
                cmd.Parameters.Add("in_deny_perm", MySqlDbType.Int32).Value = rolemap.deny_flag;
                if (rolemap.rolemenu_rowid > 0)
                {
                    cmd.Parameters.Add("in_mode_flag", MySqlDbType.VarChar).Value = "U";
                }
                else {
                    cmd.Parameters.Add("in_mode_flag", MySqlDbType.VarChar).Value = "I";
                }
                Con.Open();
                res = cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception er) { 
            
            }
            return res;
        }
    }
}
