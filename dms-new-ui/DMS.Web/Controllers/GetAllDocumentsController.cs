using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Data;
using DMS.Service;
using DMS.Model;
using System.IO;
using DMS.Web.Filters;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class GetAllDocumentsController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GetAllDocumentsController));  //Declaring Log4Net 
        GetAllDocuments_Service Objservice = new GetAllDocuments_Service();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        // GET: GetAllDocuments
        public ActionResult GetAllDocuments()
        {
            Session["docgridlist"] = null;
            return View();
        }
        //Method for fetching all scanned documents
        public ActionResult GetScannedDocuments(string activeflag,string docname = "0", string docgroup = "0", string submission = "", string status = "1")
        {
            try
            {
                Int64 empid = Convert.ToInt64(Session["Emp_Id"].ToString());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = Objservice.GetDocuments(empid, activeflag,docname,docgroup ,submission,status);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    Session["docgridlist"] = dt;
                }
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                return Json(new { Data1,Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
           
        }
        public JsonResult DocGroupNamesNew(string parentcode, string dependcode, int maxorg, int currentorg)
        {
            List<DocGroupMaster_Model.DocGroupDynamicLop> Get_Dept = new List<DocGroupMaster_Model.DocGroupDynamicLop>();
            List<DataTable> getdept_ = new List<DataTable>();
            try
            {
                getdept_ = Docgrpsrv.GetDocGroupsNew(parentcode, dependcode, maxorg, currentorg);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            string Data1 = JsonConvert.SerializeObject(getdept_[0]);
            string Data2 = JsonConvert.SerializeObject(getdept_[1]);
            string Data3 = JsonConvert.SerializeObject(getdept_[2]);
            string Data4 = JsonConvert.SerializeObject(getdept_[3]);
            return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
        }

    }
}