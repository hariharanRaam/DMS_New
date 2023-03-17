using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using DMS.Web.Filters;
using DMS.Service;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class UserGroupsMappingController : Controller
    {
        UserGroups_Service MasterBusiness = new UserGroups_Service();
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UserGroupsMappingController));
        // GET: UserGroups
        public ActionResult UserGroupsMapping()
        {
            UserGroups_Model ug = new UserGroups_Model();
            List<UserGroups_Model.menu> ml = new List<UserGroups_Model.menu>();
            UserGroups_Model.menu m = new UserGroups_Model.menu();
            try
            {
                return View("UserGroups");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        public ActionResult RoleMapping(int user_gid, string mode_)
        {
            ViewBag.usergrp_gid = user_gid;
            ViewBag.mode_flag = mode_;
            return View();
        }

        public JsonResult RoleMappingData_(int user_gid)
        {
            string Data1 = "";
            DataTable dt = new DataTable();
            try
            {
                dt = UserGroups_Service.GetRoleMappings(user_gid);
                Data1 = JsonConvert.SerializeObject(dt);

            }
            catch (Exception er)
            {

            }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getScreenData(int screen_id, string screen_name) {
            string Data1 = "";
            string user_gcode = Session["Emp_Id"].ToString();
            DataTable dt = new DataTable();
            try
            {
                dt = UserGroups_Service.getScreenData_Srv(screen_id, screen_name,Convert.ToInt32(user_gcode));
                Data1 = JsonConvert.SerializeObject(dt);

            }
            catch (Exception er)
            {

            }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserGroups()
        {
            List<UserGroups_Model.UserEntities> usergroups = new List<UserGroups_Model.UserEntities>();
            try
            {
                //usergroups = MasterBusiness.GetUserGroups();
                usergroups = UserGroups_Service.GetUserGroups();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(usergroups, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TreeViewPartial(string usergroup_gid)
        {
            UserGroups_Model.UserEntities user = new UserGroups_Model.UserEntities();
            UserGroups_Model.UserGroups ug = new UserGroups_Model.UserGroups();
            List<UserGroups_Model.menu> ml = new List<UserGroups_Model.menu>();
            UserGroups_Model.menu m = new UserGroups_Model.menu();
            int usergroup_id = 0;
            try
            {
                if (usergroup_gid != "" && usergroup_gid != null)
                {
                    usergroup_id = Convert.ToInt32(usergroup_gid);
                }
                else
                {
                    usergroup_id = 0;
                }
                ug = UserGroups_Service.GetTreeview(usergroup_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return PartialView("TreeviewPartial", ug);
        }

        //Bug_Id-1 Undefined error fixed.
        [HttpPost]
        public JsonResult CreateUserGroups(string usergroup_gid, string usergroup_name, string menuchecked, string menunotchecked)
        {
            string user_id = "";
            string user_gname = "";
            //string user_gcode = "";
            string user_gcode = Session["Emp_Id"].ToString();
            int usergroup_id = 0;
            if (usergroup_gid != "" && usergroup_gid != null)
            {
                usergroup_id = Convert.ToInt32(usergroup_gid);
            }
            else
            {
                usergroup_id = 0;
            }
            string result = "";
            string msg = "";
            string checkedmenu = "";
            string notcheckedmenu = "";
            int menu_gid = 0;
            UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
            try
            {
                userdata = UserGroups_Service.CreateUserGroups(usergroup_id, usergroup_name, user_gcode,"");
                result = userdata.result.ToString();
                msg = userdata.msg;
                int usergrp_id = userdata.usergroup_gid;
                if (userdata.result == 1)
                {
                    string[] checkmenu = { };
                    string[] uncheckmenu = { };
                    if (menuchecked != null)
                    {
                        if (!String.IsNullOrEmpty(menuchecked.ToString()))
                        {
                            if (menuchecked.Contains(","))
                            {
                                checkmenu = menuchecked.Split(',');
                                int len1 = checkmenu.Length;
                                for (int i = 0; i < len1; i++)
                                {
                                    checkedmenu = checkmenu[i];
                                    menu_gid = Convert.ToInt32(checkedmenu);
                                    userdata = UserGroups_Service.SetRightsFlag(menu_gid, usergrp_id, 1);
                                }
                            }
                            else
                            {
                                checkedmenu = checkmenu[0];
                                menu_gid = Convert.ToInt32(checkedmenu);
                                userdata = UserGroups_Service.SetRightsFlag(menu_gid, usergrp_id, 1);
                            }
                        }
                    }
                    if (menunotchecked != null)
                    {
                        if (!String.IsNullOrEmpty(menunotchecked.ToString()))
                        {
                            if (menunotchecked.Contains(","))
                            {
                                uncheckmenu = menunotchecked.Split(',');
                                int len1 = uncheckmenu.Length;
                                for (int i = 0; i < len1; i++)
                                {
                                    notcheckedmenu = uncheckmenu[i];
                                    menu_gid = Convert.ToInt32(notcheckedmenu);
                                    userdata = UserGroups_Service.SetRightsFlag(menu_gid, usergrp_id, 0);
                                }
                            }
                            else
                            {
                                notcheckedmenu = uncheckmenu[0];
                                menu_gid = Convert.ToInt32(notcheckedmenu);
                                userdata = UserGroups_Service.SetRightsFlag(menu_gid, usergrp_id, 0);
                            }
                        }
                    }
                }

                return Json(new { result, msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                result = "0";
                msg = "Creation process Failed";
                return Json(result, msg, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CreateUserGroupsNew(string usergroup_gid, string usergroup_name, string usergroup_code)
        {
            string user_gcode = Session["Emp_Id"].ToString();
            int usergroup_id = 0;
            if (usergroup_gid != "" && usergroup_gid != null)
            {
                usergroup_id = Convert.ToInt32(usergroup_gid);
            }
            else
            {
                usergroup_id = 0;
            }
            string result = "";
            string msg = "";
            UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
            try
            {
                userdata = UserGroups_Service.CreateUserGroups(usergroup_id, usergroup_name, user_gcode, usergroup_code);
                result = userdata.result.ToString();
                msg = "Role Created Successfully";
                return Json(new { result, msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                result = "0";
                msg = "Creation process Failed";
                return Json(result, msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteUserGroups(int? usergroup_gid)
        {
            string result;
            try
            {
                UserGroups_Model.UserEntities userdata = new UserGroups_Model.UserEntities();
                userdata = UserGroups_Service.DeleteUserGroups(usergroup_gid);
                result = userdata.msg;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                result = "Delete process Failed";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SaveUserRoleMap(List<UserGroups_Model.RoleMappings> roleMappings)
        {
            int result = 0;
            List<UserGroups_Model.RoleMappings> roleMappings1 = new List<UserGroups_Model.RoleMappings>();
            //roleMappings = new JavaScriptSerializer().Deserialize<List<UserGroups_Model.RoleMappings>>(jsondata_);
            roleMappings1 = roleMappings;
            foreach (UserGroups_Model.RoleMappings rolmap in roleMappings)
            {
                result = UserGroups_Service.SaveUserRoleMapSrv(rolmap);
            }

            return Json(result);
        }
    }
}