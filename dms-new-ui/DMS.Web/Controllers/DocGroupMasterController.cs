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
using Newtonsoft.Json;

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
        public ActionResult DocGroupMaster(string modeflag, string dgroupId)
        {
            ViewBag.mode = modeflag;
            ViewBag.dgroupId = dgroupId;
            if (modeflag == "U" || modeflag == "V")
            {
                if (dgroupId != null && dgroupId != "undefined" && dgroupId != "")
                {
                    int DgroupId = Convert.ToInt32(dgroupId);
                    Docgrpmdlobj = Docgrpsrv.DocgrpMstDtl(DgroupId);
                }
            }
            return View(Docgrpmdlobj);
        }

        public ActionResult DocGroupGrid_Read([DataSourceRequest]DataSourceRequest request)//read the docgroupmaster data
        {
            try
            {
                //return Json(Docgrpsrv.DocgrpMstDtl().ToDataSourceResult(request));
                return Json(0);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult DocGroupGrid_Save(string DocGrpID,string DocGrpNm, string ShortName,string Org_Level1val, string Org_Level2val, string Org_Level3val, string Org_Level4val,string mode)//insert new docgroupmaster data
        {
            try
            {
                Docgrpmdlobj.DgrpId = DocGrpID;
                Docgrpmdlobj.DgroupName = DocGrpNm;
                Docgrpmdlobj.Dgroup_shortname = ShortName;
                Docgrpmdlobj.Org_Level1 = Org_Level1val;
                Docgrpmdlobj.Org_Level2 = Org_Level2val;
                Docgrpmdlobj.Org_Level3 = Org_Level3val;
                Docgrpmdlobj.Org_Level4 = Org_Level4val;
                Docgrpmdlobj.mode = mode;
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
        public JsonResult DocGroupNames(string parentcode,string dependcode)
        {
            List<DocGroupMaster_Model.DocGroupDynamicLop> Get_Dept = new List<DocGroupMaster_Model.DocGroupDynamicLop>();
            try
            {
                Get_Dept = Docgrpsrv.GetDocGroup(parentcode,dependcode);
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

        public JsonResult getlabellist() {
            List<DMS.Model.DocGroupMaster_Model.DocGroupDynamicLabel> labellist = new List<DMS.Model.DocGroupMaster_Model.DocGroupDynamicLabel>();
            DocGroupMaster_Service serviceobj = new DocGroupMaster_Service();
            try
            {
                labellist = serviceobj.GetDoclabelsrv();

            }
            catch (Exception er) {
                throw er;
            }
            return Json(labellist);
        }

        public ActionResult DocGroupGridread() {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            DocGroupMaster_Service serviceobj = new DocGroupMaster_Service();
            ds = serviceobj.DocGroupGridread();
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