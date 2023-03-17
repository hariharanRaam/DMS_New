using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DMS.Web.Filters;
using DMS.Model;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class EmployeeMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 
        EmployeeMaster_Model EDtlObj = new EmployeeMaster_Model(); //Creating model object.
        EmployeeMaster__Service EmpSrvObj = new EmployeeMaster__Service();//Creating service object.

        public ActionResult EmployeeMaster(string mode,string userid)// GET: EmployeeMaster.
        {
            ViewBag.mode = mode;
            ViewBag.user_id = userid;
            if (mode != null && mode != "" && mode != "undefined" && mode != "I") {
                if (userid != null && userid != "" && userid != "undefined") {
                    int _userid = Convert.ToInt32(userid);
                    EDtlObj = EmpSrvObj.EmployeeMstDtl(_userid);
                }
            }
            return View(EDtlObj);
        }
        //Employee Master read.
        public JsonResult EmployeeGrid_Read(EmployeeMaster_Model EDtlObj) 
        {
            List<EmployeeMaster_Model> emplist = new List<EmployeeMaster_Model>();
            try
            {
                emplist = EmpSrvObj.EmployeeMstDtllist(EDtlObj);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(emplist, JsonRequestBehavior.AllowGet);
        }
        //Save employee details. Bug_Id-7 Dateformat issue fixed.
        public JsonResult SaveEmployee( EmployeeMaster_Model EDtlObj) 
        {
            try
            {
                EDtlObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
              //  EDtlObj.OrgLevelMax = orglvlmax;

                return Json(EmpSrvObj.SaveEmployee(EDtlObj), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
              //  return View();
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        private DateTime GetUtcTime(DateTime dateTime, decimal dateTimeOffsetMinutes)
        {
            return DateTime.SpecifyKind(dateTime.AddMinutes((double)dateTimeOffsetMinutes), DateTimeKind.Utc);
        }
        //Update employee details.
        public JsonResult EmployeeGrid_Update( EmployeeMaster_Model EDtlObj)
        {
            try
            {
                DataTable dt = new DataTable();
                EDtlObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
             //   EDtlObj.OrgLevelMax = orglvlmax;
                return Json(EmpSrvObj.EmployeeMstDtlUpdate(EDtlObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            //    return View();
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }      
        }

        //delete the data.
        public ActionResult DeletingEmployee(int? EmployeeID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = EmpSrvObj.DeletingDepartment(EmployeeID);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }

        }
        //binding popup dropdown.
        public JsonResult GetEmployeeNames(string CommonVal) 
        {
            List<EmployeeMaster_Model> Get_Dept = new List<EmployeeMaster_Model>();
            try
            {
                Get_Dept = EmpSrvObj.GetDepartment(CommonVal);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpOrglvls(string empid)
        {
            EmployeeMaster_Model EDtlObj2 = new EmployeeMaster_Model();
            try
            {
                EDtlObj2 = EmpSrvObj.GetEmpOrglvls(empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(EDtlObj2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeMaster()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
           // DocGroupMaster_Service serviceobj = new DocGroupMaster_Service();
            ds = EmpSrvObj.EmployeeMasterSrv();
            if (ds.Tables.Count > 0)
            {
                // var UniqueRows = ds.Tables[0].AsEnumerable().Distinct(DataRowComparer.Default);
                //  dt = UniqueRows.CopyToDataTable();
                dt = ds.Tables[0];
                if (ds.Tables.Count > 1)
                {
                    dt1 = ds.Tables[1];
                }
                else
                {
                    dt1 = ds.Tables[0];
                }
            }
            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt);
            Data2 = JsonConvert.SerializeObject(dt1);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
    }
}