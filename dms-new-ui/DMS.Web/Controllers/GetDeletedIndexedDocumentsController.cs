using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DMS.Web.Filters;
using DMS.Service;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class GetDeletedIndexedDocumentsController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EditIndexedDocumentController));  //Declaring Log4Net 
        GetDeletedIndexedDocuments_Service serviceobj = new GetDeletedIndexedDocuments_Service();  //Generating service object
        // GET: GetDeletedIndexedDocuments
        public ActionResult GetDeletedIndexedDocuments()
        {
            return View();
        }
        public ActionResult GetDeletedIndexedDocs()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceobj.getdeletednidexedrecords();
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
            }
            return View();
        }
    }
}