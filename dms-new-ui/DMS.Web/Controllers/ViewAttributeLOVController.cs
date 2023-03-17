using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DMS.Web.Filters;
using DMS.Service;
using Newtonsoft.Json;
using DMS.Model;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class ViewAttributeLOVController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(_LayoutController));  //Declaring Log4Net 
        ViewAttributeLOV_Service serviceObj = new ViewAttributeLOV_Service();
        ViewAttributeLOV_Model modelObj = new ViewAttributeLOV_Model();
        ViewAttributeLOV_Model depts;
        List<ViewAttributeLOV_Model> lstobj = new List<ViewAttributeLOV_Model>();
        public ActionResult ViewAttributeLOV()// GET: ViewAttributeLOV
        {
            return View();
        }

        public ActionResult GetListView()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.GetListView();
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

        public ActionResult PartialView(int? lovid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = serviceObj.getlovvalues(lovid);
                foreach (DataRow dr in dt.Rows)
                {
                    depts = new ViewAttributeLOV_Model();
                    depts.lovtext = dr["Lovexl_Name"].ToString();
                    modelObj.dept.Add(depts);
                }
                return PartialView("LovPartialView", modelObj);
            }


            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("LovPartialView", modelObj);
            }
        }

        [HttpPost]
        public ActionResult UpdateLOV(string[] form, ViewAttributeLOV_Model modelObj)
        {
            int Result = 0;
            try
            {
                int AtrCount = modelObj.slno;
                modelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                for (int i = 0; i < AtrCount; i++)
                {
                    ViewAttributeLOV_Model model = new ViewAttributeLOV_Model();
                    model.lovtext = form[i];
                    model.masterid = modelObj.masterid;
                    model.slno = i+1;
                    lstobj.Add(model);
                }
                //passing model object and model list to service class
                Result = serviceObj.Updatelov(modelObj, lstobj);
                return Json(Result,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}