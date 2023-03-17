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
    public class DocNameMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net. 
        DocNameMaster_Service serviceObj = new DocNameMaster_Service();  //Creating service class object.
        DocNameMaster_Model modelObj = new DocNameMaster_Model();       //Creating model class object.

        public ActionResult DocNameMaster()   // GET: DocNameMaster.
        {
            return View();
        }
        
        public ActionResult GetAllDocNames([DataSourceRequest]DataSourceRequest request)  //Method for fetching all DocName Records
        {
            try
            {
                return Json(serviceObj.GetAllDocNames().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public JsonResult GetmasterDorpdowns(string CommonVal)   //Method for fetching all dropdown values
        {
            List<DocNameMaster_Model> Dropdowns = new List<DocNameMaster_Model>();
            try
            {
                Dropdowns = serviceObj.GetmasterDorpdowns(CommonVal);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Dropdowns, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DocGroupDropDown(int? masteritemid, string master)   //Method for populating other dropdown values based on docgroupid
        {
            List<DocNameMaster_Model> unitdropdownlist = new List<DocNameMaster_Model>();
            List<DocNameMaster_Model> deptdropdownlist = new List<DocNameMaster_Model>();
            try
            {
                unitdropdownlist = serviceObj.unit(masteritemid, master);
                deptdropdownlist = serviceObj.deptarment(masteritemid, master);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            //return Json(Dropdowns, JsonRequestBehavior.AllowGet);
            return Json(new { unitdropdownlist, deptdropdownlist }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UnitDropDown(int? masteritemid, string master)   //Method for populating other dropdown values based on docgroupid
        {
            List<DocNameMaster_Model> deptdropdownlist = new List<DocNameMaster_Model>();
            try
            {
                deptdropdownlist = serviceObj.deptarment(masteritemid, master);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(deptdropdownlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDocName([DataSourceRequest] DataSourceRequest request, DocNameMaster_Model ModelObj)  //Method for Save new DocName
        {
            try
            {
                ModelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                return Json(serviceObj.SaveDocName(ModelObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult UpdateDocName([DataSourceRequest] DataSourceRequest request, DocNameMaster_Model ModelObj) //Method for Update existing DocName
        {
            try
            {
                ModelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                return Json(serviceObj.UpdateDocName(ModelObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        //delete the data
        public ActionResult DeletingDocName(int? DNameID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = serviceObj.DeletingDocName(DNameID);
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

    }
}