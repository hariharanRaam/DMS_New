using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using DMS.Service;
using DMS.Model;
using DMS.Web.Filters;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class ViewDocumentAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GetAllDocumentsController));  //Declaring Log4Net. 
        ViewDocumentAttributes_Service serviceObj = new ViewDocumentAttributes_Service(); //Statement for creating service object.
        ViewDocumentAttributes_Model modelObj = new ViewDocumentAttributes_Model();//statement for creating model object.

        public ActionResult ViewDocumentAttributes() // GET: ViewDocumentAttributes
        {

            return View();
        }

        //Bug_Id-3 showing duplicate attribute fixed.
        public ActionResult GetDynamicAttributes()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.getdynamicattributes();
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        public JsonResult AttributePartialView(string group_gid)
        {
            int count = 0;
            int GroupAtrID = Convert.ToInt32(group_gid);
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = serviceObj.Getattributes(GroupAtrID);
                if (dt.Rows.Count > 0)
                {
                    count = dt.Rows.Count;
                    Data1 = JsonConvert.SerializeObject(dt);
                }
                return Json(new { count, Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult partialview()
        {
            return PartialView("DocAttributePartial", modelObj);
        }


        public ActionResult EditAttributepartialview(Int64 Attrbid, Int64 docnameid)  //, Int64 docname_id, string docname
        {
            ViewBag.Attrbid = Attrbid;
            ViewBag.docname_id = docnameid;
            //ViewBag.docname = docname;
            return PartialView("EditAttribute");
        }

        public ActionResult EditAttribute(Int64 Attrbid = 0, Int64 Dognameid = 0)
        {
            Int64 _empid = Convert.ToInt32(Session["Emp_Id"].ToString());
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = serviceObj.Getattribute(Attrbid, Dognameid,_empid);
                Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAttribut(Int64 docnameId = 0, Int64 docgrpId = 0, Int64 unitId = 0, Int64 depId = 0, Int64 docnameId1 = 0)
        {
            ViewBag.docnameId = docnameId;
            ViewBag.docgrpId = docgrpId;
            ViewBag.unitId = unitId;
            ViewBag.depId = depId;
            ViewBag.docnameId1 = docnameId1;
            return PartialView("GetAttribute");
        }
        //Bug_Id-8 Same as dropdown issue is fixed.
        public ActionResult GetAttribute(Int64 docnameId = 0, Int64 docgrpId = 0, Int64 unitId = 0, Int64 depId = 0, Int64 docnameId1 = 0)
        {
            Int64 _empid = Convert.ToInt32(Session["Emp_Id"].ToString());
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable ();
                dt = serviceObj.Geteditattribute(docnameId, docgrpId, unitId, depId, docnameId1, _empid);
                Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("GetAttribute", serviceObj);
            }
        }

        public ActionResult Save(Int64 DgroupID = 0, Int64 DNameID = 0, string dynamictxt = "0", string dynamictxtlov = "0")
        {
            Int64 _empid = Convert.ToInt32(Session["Emp_Id"].ToString());
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = serviceObj.Setattribute(DgroupID, DNameID, dynamictxt, dynamictxtlov, _empid);
                Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

    }
}