using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using System.Data;
using DMS.Service;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class EditIndexedDocumentController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EditIndexedDocumentController));  //Declaring Log4Net 

        EditIndexedDocument_Service serviceobj = new EditIndexedDocument_Service();

        // GET: EditIndexedDocument
        public ActionResult EditIndexedDocument()
        {
            return View();
        }
        public JsonResult GetIndexedDocuments()
        {
            string Data1 = "", Data2 = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceobj.Getindexedrecords();
                if(ds.Tables.Count>0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }     
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
    }
}