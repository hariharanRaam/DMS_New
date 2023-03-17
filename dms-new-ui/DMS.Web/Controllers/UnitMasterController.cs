using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using Kendo.Mvc.UI;
using DMS.Service;
using DMS.Model;
using Kendo.Mvc.Extensions;
using System.Data;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class UnitMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 
        UnitMaster_Service serviceObj = new UnitMaster_Service();
        UnitMaster_Model modelObj = new UnitMaster_Model();
        // GET: UnitMaster
        public ActionResult UnitMaster()
        {
            return View();
        }
        public ActionResult GetAllUnits([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return Json(serviceObj.Getallunits().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult SaveUnit([DataSourceRequest] DataSourceRequest request, UnitMaster_Model ModelObj)
        {
            try
            {
                ModelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                return Json(serviceObj.SaveUnit(ModelObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult UpdateUnit([DataSourceRequest] DataSourceRequest request, UnitMaster_Model ModelObj)
        {
            try
            {
                ModelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                return Json(serviceObj.UpdateUnit(ModelObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            } 
            return View();
        }

        public ActionResult DeletingUnit(int? UnitID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = serviceObj.deletingUnit(UnitID);
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

        public JsonResult GetDepartment(string CommonVal)
        {
            List<UnitMaster_Model> Get_Dept = new List<UnitMaster_Model>();
            try
            {
                Get_Dept = serviceObj.GetDepartment(CommonVal);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }

    }
}