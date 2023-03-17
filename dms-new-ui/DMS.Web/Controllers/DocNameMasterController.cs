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
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DocNameMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net. 
        DocNameMaster_Service serviceObj = new DocNameMaster_Service();  //Creating service class object.
        DocNameMaster_Model modelObj = new DocNameMaster_Model();       //Creating model class object. 
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        public JsonResult DocGroupNames(string parentcode, string dependcode)
        {
            List<DocGroupMaster_Model.DocGroupDynamicLop> Get_Dept = new List<DocGroupMaster_Model.DocGroupDynamicLop>();
            try
            {
                Get_Dept = Docgrpsrv.GetDocGroup(parentcode, dependcode);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DocNameMaster(string modeflag, string dnameid)   // GET: DocNameMaster.
        {
            if (modeflag == "U" || modeflag == "V") {
                if (dnameid != null && dnameid != "undefined") {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    int dnameid_ = Convert.ToInt32(dnameid);
                    ds = serviceObj.GetAllDocNames(dnameid_);

                    if (ds.Tables.Count > 0)
                    {
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
                    string Data1, Data2;
                    Data1 = JsonConvert.SerializeObject(dt);
                    Data2 = JsonConvert.SerializeObject(dt1);
                }
            }
            return View(modelObj);
        }

        public ActionResult DocNameMasterPartialPopUp(string modeflag , string dnameid)   // GET: DocNameMaster.
        {
            ViewBag.mode = modeflag;
            ViewBag.dnameid = dnameid;
            if (modeflag == "U" || modeflag == "V")
            {
                if (dnameid != null && dnameid != "undefined")
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    int dnameid_ = Convert.ToInt32(dnameid);
                    ds = serviceObj.GetAllDocNames(dnameid_);

                    if (ds.Tables.Count > 0)
                    {
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
                    foreach (DataRow row in dt.Rows)
                    {
                        modelObj.DNameID = Convert.ToInt32(row["Dname_Id"].ToString());
                        modelObj.DgroupID = Convert.ToInt32(row["Dgroup_Id"].ToString());
                        modelObj.DgroupName = row["Doc Group"].ToString();
                        modelObj.DocName = row["Doc Name"].ToString();
                        modelObj.DocPeriodAviavablity = row["DocPeriodAvailablity"].ToString();
                        modelObj.AP = row["Active_Peroid"].ToString();
                        modelObj.Dname_Shortname = row["Short Name"].ToString();
                    }

                   // string Data1, Data2;
                  //  Data1 = JsonConvert.SerializeObject(dt);
                   // Data2 = JsonConvert.SerializeObject(dt1);
                }
            }
            return View(modelObj);
        }

        public ActionResult GetAllDocNames()  //Method for fetching all DocName Records
        {
            try
            {
                //return Json(serviceObj.GetAllDocNames().ToDataSourceResult(request));
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.GetAllDocNames(0);

                if (ds.Tables.Count > 0)
                {
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
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                //Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);

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

        public ActionResult SaveDocName(string DnameId, string DocGrpID, string DocName, string ShortName, string mode, string dpval, string AP, string PP)  //Method for Save new DocName
        {
            DocNameMaster_Model ModelObj = new DocNameMaster_Model();
            try
            {
                ModelObj.DgroupID = Convert.ToInt64(DocGrpID);
                ModelObj.DNameID = Convert.ToInt32(DnameId);
                ModelObj.DocName = DocName;
                ModelObj.Dname_Shortname = ShortName;
                ModelObj.mode = mode;
                ModelObj.DocPeriodAviavablity = dpval;
                ModelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                ModelObj.AP = AP;
                ModelObj.PP = PP;
                return Json(serviceObj.SaveDocName(ModelObj));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult UpdateDocName(DocNameMaster_Model ModelObj) //Method for Update existing DocName
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

        public JsonResult Peroid(string parentcode, string dependcode)
        {
            List<DocGroupMaster_Model.DocGroupDynamicLop> dropdown = new List<DocGroupMaster_Model.DocGroupDynamicLop>();
            try
            {
                for (int i = 0; i <= 10; i++) {
                    DocGroupMaster_Model.DocGroupDynamicLop dt1 = new DocGroupMaster_Model.DocGroupDynamicLop();
                    if (i == 0)
                    {
                        dt1.master_code = i.ToString();
                        dt1.master_name = "Select";
                    }
                    else {
                        dt1.master_code = i.ToString();
                        dt1.master_name = i + " Year";
                    }
                    dropdown.Add(dt1);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(dropdown, JsonRequestBehavior.AllowGet);
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