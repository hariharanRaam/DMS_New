using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using DMS.Service;
using System.Data;
using DMS.Model;
using Newtonsoft.Json;

//vadivu code
namespace DMS.Web.Controllers
{

    [UserAuntheication]
    public class StorageAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigureAttributesController));  //Declaring Log4Net
        StorageAttributes_Service Storageobjsrv = new StorageAttributes_Service();
        public List<DocGroupMaster_Model> DocGroup = new List<DocGroupMaster_Model>();
        DocGroupMaster_Model Docgrpmdlobj = new DocGroupMaster_Model();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        //StorageAttributes_Service Strobjsrv = new StorageAttributes_Service();
        // GET: StorageAttributes
        public ActionResult StorageAttributes()
        {
            return View();
        }

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

        //Save storage attribute. 
        [HttpPost]
        public ActionResult Save( int DgroupID, int DNameID, string[] attributes1, string[] attributes2, string[] attributes3, string[] attributes4, string[] attributes5)
        {
            int Result = 0;
            try
            {
                int UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                string[] attrnameval = attributes1[0].ToString().Split(',');
                string[] attrtypeval = attributes2[0].ToString().Split(',');
                string[] attrlenval = attributes3[0].ToString().Split(',');
                string[] attrmandatoryval = attributes4[0].ToString().Split(',');
                string[] Storage_orderid = attributes5[0].ToString().Split(',');
                DataSet ds = new DataSet();
                for (int i = 0; i < attrnameval.Length; i++)
                {
                    string Len = attrlenval[i].ToString();
                    if (Len == "")
                    {
                        Len = "0";
                    }
                    ds = Storageobjsrv.SaveStorageAttri(attrnameval[i].ToString(), Convert.ToInt16(Len), attrtypeval[i].ToString(), attrmandatoryval[i].ToString(), Convert.ToInt16(Storage_orderid[i].ToString()),DgroupID, DNameID, UserID);

                }
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (Result == 1)
                {
                    ds = Storageobjsrv.saveStorageAtt( DgroupID, DNameID, UserID);
                    ViewBag.Message = "File uploaded successfully";

                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            // return View();
            return Json(new { success = Result, JsonRequestBehavior.AllowGet });
        }

        //chexksavestorageattributes
        [HttpPost]
        public JsonResult CheckStorageattribute( int? DgroupID, int? DNameID)
        {
            int Result = 0;
            try
            {

                DataSet ds = new DataSet();
                ds = Storageobjsrv.CheckSaveStorageattrib( DgroupID, DNameID);
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        //04-04-2019 Viewpartialviewedit

        //#region View and Addsameas

        public ActionResult ViewPartialViewEdit(int sameasdocnameid)
        {
            ViewBag.samednameid = sameasdocnameid;
            return PartialView("StorageAttributePartialView"); // for StorageAttributes
        }

        public JsonResult GetSameasattributesdata(int sameasdocnameid)
        {
            int count = 0;
            int Docnameid = Convert.ToInt32(sameasdocnameid);
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = Storageobjsrv.GSameasAttributeValues(Docnameid);
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


        public JsonResult GetSameasAttributeplusdata(int sameasdocnameid)
        {
            int count = 0;
            int Docnameid = Convert.ToInt32(sameasdocnameid);
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = Storageobjsrv.GetSameasAttributeplusdata(Docnameid);
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

        public ActionResult DeleteAttribute(int strid)
        {
            int Result = 0;
            try
            {
                int UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                DataSet ds = new DataSet();
                ds = Storageobjsrv.DeleteStorageAttri(strid);
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (Result == 1)
                {
                    //ds = serviceObj.saveStorageAtt(DeptID, UnitID, DgroupID, DNameID, UserID);
                    ViewBag.Message = "Deleted successfully";

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }


            return PartialView("PartialViewEdit");
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

        //#endregion

    }
}