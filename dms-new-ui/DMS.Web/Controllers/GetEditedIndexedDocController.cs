using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class GetEditedIndexedDocController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EditIndexedDocumentController));  //Declaring Log4Net 
        EditIndexedDocument_Service serviceobj = new EditIndexedDocument_Service();
        // GET: ApproveEditedIndexedDoc
        public ActionResult GetEditedIndexedDoc()
        {
            return View();
        }
        public ActionResult GetEditedIndexedDocuments()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceobj.Getindexedrecordsforapproval();
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