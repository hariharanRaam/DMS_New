using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DMS.Service;
using System.Data;
using DMS.Web.Filters;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DocGroupMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 
        public List<DocGroupMaster_Model >  DocGroup = new List<DocGroupMaster_Model>();
        DocGroupMaster_Model Docgrpmdlobj = new DocGroupMaster_Model();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        // GET: DocGroupMaster
        public ActionResult DocGroupMaster()
        {
            return View();
        }

        public ActionResult DocGroupGrid_Read([DataSourceRequest]DataSourceRequest request)//read the docgroupmaster data
        {
            try
            {
                return Json(Docgrpsrv.DocgrpMstDtl().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult DocGroupGrid_Save([DataSourceRequest]DataSourceRequest request, DocGroupMaster_Model Docgrpmdlobj)//insert new docgroupmaster data
        {
            try
            {
                  Docgrpmdlobj.DgroupCreatedBy= (Session["Emp_Id"].ToString());
                return Json(Docgrpsrv.DocGroupMstDtlSave(Docgrpmdlobj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult DocGroupGrid_Update([DataSourceRequest] DataSourceRequest request, DocGroupMaster_Model Docgrpmdlobj)//update the existing docgroupmaster data
        {
            try
            {
                Docgrpmdlobj.DgroupCreatedBy = (Session["Emp_Id"].ToString());
                return Json(Docgrpsrv.DocGroupMstDtlUpdate(Docgrpmdlobj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult DeletingDocGroup(int? DGroupID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = Docgrpsrv.DeletingDocGroup(DGroupID);
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

        //binding popup dropdown
        public JsonResult DocGroupNames(string CommonVal)
        {
            List<DocGroupMaster_Model> Get_Dept = new List<DocGroupMaster_Model>();
            try
            {
                Get_Dept = Docgrpsrv.GetDocGroup(CommonVal);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnitDropDown(int? masteritemid, string master)   //Method for populating other dropdown values based on docgroupid
        {
            DocNameMaster_Service serviceObj = new DocNameMaster_Service();
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

    }
}