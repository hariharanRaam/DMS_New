using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Model;
using DMS.Data;
using System.Web.SessionState;

namespace DMS.Service
{
    public class UserGroups_Service
    {
        
        public static List<UserGroups_Model.UserEntities> GetUserGroups()
        {
            DataTable tab = new DataTable();
            List<UserGroups_Model.UserEntities> ls_usergroups = new List<UserGroups_Model.UserEntities>();
            try
            {
                UserGroups_Data objproduct = new UserGroups_Data();
                tab = objproduct.GetUserGroups();
                if (tab.Rows.Count > 0)
                {
                    foreach (DataRow dr in tab.Rows)
                    {
                        UserGroups_Model.UserEntities usergrp = new UserGroups_Model.UserEntities();
                        usergrp.usergroup_code = dr["usergroup_code"].ToString();
                        usergrp.usergroup_name = dr["usergroup_name"].ToString();
                        usergrp.usergroup_gid = Convert.ToInt32(dr["usergroup_gid"].ToString());
                        ls_usergroups.Add(usergrp);
                    }
                }
                return ls_usergroups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static UserGroups_Model.UserGroups GetTreeview(int usergroup_gid)
        {
            DataTable tab = new DataTable();
            List<UserGroups_Model.menu> ls_menu = new List<UserGroups_Model.menu>();
            UserGroups_Model.UserGroups usergroups = new UserGroups_Model.UserGroups();
            try
            {
                UserGroups_Data objproduct = new UserGroups_Data();
                tab = objproduct.GetTreeview(usergroup_gid);
                if (tab.Rows.Count > 0)
                {
                    foreach (DataRow dr in tab.Rows)
                    {
                        int menu_access = 0;
                        UserGroups_Model.menu menu = new UserGroups_Model.menu();
                        menu.menu_gid = Convert.ToInt32(dr["menu_gid"].ToString());
                        menu.parent_menu_gid = Convert.ToInt32(dr["parent_menu_gid"].ToString());
                        menu.menu_name = dr["menu_name"].ToString();
                        menu.menu_order = Convert.ToInt32(dr["menu_order"].ToString());
                        if (!String.IsNullOrEmpty(dr["rights_flag"].ToString()))
                        {
                            menu_access = Convert.ToInt32(dr["rights_flag"].ToString());
                        }
                        else
                        {
                            menu_access = 0;
                        }
                        if (menu_access == 0)
                        {
                            menu.rights_flag = false;
                        }
                        else
                        {
                            menu.rights_flag = true;
                        }
                        ls_menu.Add(menu);
                    }
                }
                usergroups.menu = ls_menu;
                return usergroups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserGroups_Model.UserEntities CreateUserGroups(int usergroup_gid, string usergroup_name, string action_by, string usergroup_code)
        {
            string[] result = { };
            DataTable dt = new DataTable();
            try
            {
                UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
                UserGroups_Data objproduct = new UserGroups_Data();
                result = objproduct.CreateUserGroups(usergroup_gid, usergroup_name, action_by, usergroup_code);
                if (result.Length == 3)
                {
                    userdata.usergroup_gid = Convert.ToInt32(result[2]);
                    userdata.result = Convert.ToInt32(result[1]);
                    userdata.msg = result[0];
                }
                else
                {
                    userdata.usergroup_gid = 0;
                    userdata.result = 0;
                    userdata.msg = "Process Failed";
                }
                return userdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserGroups_Model.UserEntities DeleteUserGroups(int? usergroup_gid)
        {
            string[] result = { };
            UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
            UserGroups_Data objproduct = new UserGroups_Data();
            result = objproduct.DeleteUserGroups(usergroup_gid);
            if (result.Length == 1)
            {
                userdata.msg = result[0];
            }
            else
            {
                userdata.msg = "Process Failed";
            }
            return userdata;
        }
        public static UserGroups_Model.UserEntities SetRightsFlag(int menu_gid, int usergroup_gid, int rights_flag)
        {
            string[] result = { };
            DataTable dt = new DataTable();
            try
            {
                UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
                UserGroups_Data objproduct = new UserGroups_Data();
                result = objproduct.SetRightsFlag(menu_gid, usergroup_gid, rights_flag);
                if (result.Length == 2)
                {
                    userdata.result = Convert.ToInt32(result[1]);
                    userdata.msg = result[0];
                }
                else
                {
                    userdata.result = 0;
                    userdata.msg = "Process Failed";
                }
                return userdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<UserGroups_Model.menu> GetMenu(string user,string menutype, int menu_gid)
        {
            DataTable tab = new DataTable();
            List<UserGroups_Model.menu> ls_menu = new List<UserGroups_Model.menu>();
            try
            {
                int UserID = 0;

                Int32.TryParse(user, out UserID);
                UserGroups_Data objproduct = new UserGroups_Data();
                tab = objproduct.GetMenu(UserID,menutype,menu_gid);
                if (tab.Rows.Count > 0)
                {
                    foreach (DataRow dr in tab.Rows)
                    {
                        int menu_access = 0;
                        UserGroups_Model.menu menu = new UserGroups_Model.menu();
                        menu.menu_gid = Convert.ToInt32(dr["menu_gid"].ToString());
                        menu.parent_menu_gid = Convert.ToInt32(dr["parent_menu_gid"].ToString());
                        menu.menu_name = dr["menu_name"].ToString();
                        menu.menu_order = Convert.ToInt32(dr["menu_order"].ToString());
                        if (!String.IsNullOrEmpty(dr["rights_flag"].ToString()))
                        {
                            menu_access = Convert.ToInt32(dr["rights_flag"].ToString());
                        }
                        else
                        {
                            menu_access = 0;
                        }
                        if (menu_access == 0)
                        {
                            menu.rights_flag = false;
                        }
                        else
                        {
                            menu.rights_flag = true;
                        }
                        menu.menu_url = dr["menu_url"].ToString();
                        menu.icon_path = dr["icon_path"].ToString();
                        ls_menu.Add(menu);
                    }
                }

                return ls_menu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      public static DataTable GetRoleMappings(int user_gid){
        DataTable dt = new DataTable();
              UserGroups_Data objproduct = new UserGroups_Data();
              dt = objproduct.GetRoleMappingData(user_gid);

            return dt;
        }


      public static DataTable getScreenData_Srv(int screen_id, string screen_name, int user_gcode)
      {
          DataTable dt = new DataTable();
          UserGroups_Data objproduct = new UserGroups_Data();
          dt = objproduct.getScreenData_Data(screen_id, screen_name,user_gcode);

          return dt;
      }

   public static int SaveUserRoleMapSrv(UserGroups_Model.RoleMappings rolemap){
        int res = 0;
        UserGroups_Data objproduct = new UserGroups_Data();
        res = objproduct.SaveUserRoeMapData(rolemap);
        return res;
    } 
   }

 
}
