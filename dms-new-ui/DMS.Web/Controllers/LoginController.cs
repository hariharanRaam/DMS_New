using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using DMS.Service;
using System.Data;

namespace DMS.Web.Controllers
{
    public class LoginController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LoginController));  //Declaring Log4Net 
        Login_Model Objmodel = new Login_Model();
        Login_Service Objservice = new Login_Service();
        ForgotPwd_Service observice = new ForgotPwd_Service();//forgotpswd service obj
        change_password Obmodel = new change_password();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //POST:Login
        [HttpPost]
        public ActionResult Login(string txtusername, string txtpwd)
        {
            try
            {
                DateTime expired_date =Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ExpiredDate"].ToString());
                DateTime dateTime = DateTime.UtcNow.Date;
                DateTime current_date = Convert.ToDateTime(dateTime.ToString("dd-MM-yyyy"));
                //condition for validating application expired date.
                if (expired_date > current_date)
                {
                    Session.Remove("Employee_Name");
                    Session.Remove("Employee_Type");
                    Session.Remove("EmployeeRole_Id");
                    Session.Remove("Emp_Id");
                    Session.Remove("UserGroup_ID");
                    DataTable dt = new DataTable();
                    Objmodel.UserName = txtusername;
                    Objmodel.Password = txtpwd;
                    dt = Objservice.Getusername(Objmodel);
                    if (dt.Rows.Count > 0)
                    {
                        Session["Emp_Id"] = dt.Rows[0]["Emp_Id"].ToString();
                        Session["Employee_Name"] = dt.Rows[0]["Emp_Name"].ToString();
                        Session["Employee_Type"] = dt.Rows[0]["EmpType_Id"].ToString();
                        Session["EmployeeRole_Id"] = dt.Rows[0]["Role_Id"].ToString();
                        Session["UserGroup_ID"] = dt.Rows[0]["usergroup_gid"].ToString();
                        //return RedirectToAction("Dashboard", "Dashboard");
                        if (dt.Rows[0]["Emp_Fpwd_Flag"].ToString() == "Y")
                        {
                            return RedirectToAction("ChangePassword", "Login");
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Dashboard");
                          //  return RedirectToAction("CreateMaster", "CreateMaster");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Invalid username or password";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Your trail period is expired please contact admin.!!";
                    return View();
                }
               
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }   
        }
        [HttpPost]
        public ActionResult Signout()
        {
            try
            {
                Session.Remove("Emp_Id");
                return this.RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        //Change Password
        public ActionResult ChangePassword()
        {
            try
            {
                Session.Remove("dismessage");
                Obmodel.Emp_Id = Convert.ToInt32(Session["Emp_Id"].ToString());
                return View(Obmodel);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View(Obmodel);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(change_password Obmodel)
        {
            string Result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable dt = new DataTable();
                    dt = Objservice.Cpwd(Obmodel);
                    Result = dt.Rows[0][0].ToString();
                    Session["dismessage"] = Result;
                    if (Result == "Password Changed Succesfully!")
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else{
                        ViewBag.Message = Result;
                        return View(Obmodel);
                    }
                    
                }
                return View();
                //return Content("<script language='javascript' type='text/javascript'> alert('Thanks for Feedback!');</script>");
               
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        //Forgot Password
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string Emp_Code, string Useremailid)
        {
            string Result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    Result = Objservice.EmailManager(Emp_Code, Useremailid);
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}