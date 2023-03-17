using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DMS.Service;
using DMS.Web.Filters;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class ViewStorageAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GetAllDocumentsController));  //Declaring Log4Net. 
        ViewStorageAttributes_Service serviceObj = new ViewStorageAttributes_Service();
        StorageAttributes_Service Storageobjsrv = new StorageAttributes_Service();
        // GET: ViewStorageAttributes
        public ActionResult ViewStorageAttributes()
        {
            return View();
        }

        public ActionResult GetDynamicStorageAttributes()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.GetDynamicStorageAttributes();
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

        //bind value to dynamice textbox
        public JsonResult GetStorageAttributeValues(string group_gid)
        {
            int count = 0;
            int GroupAtrID = Convert.ToInt32(group_gid);
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = serviceObj.GetStorageAttributeValues(GroupAtrID);
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
        public ActionResult PartialView()
        {
            //return PartialView("DocAttributePartial", modelObj);
            return PartialView("StorageAttributePartialView");
        }

        //   30-03-2019 UpdateStoragettribute,Delete

        #region UpdateStoragettribute
        public ActionResult PartialViewEdit()
        {
            return PartialView("PartialViewEdit");
        }
        ////Bug_Id-4 Update issue fixed
        [HttpPost]
        public ActionResult Update(string[] attributes0,int DgroupID, int DNameID, string[] attributes1, string[] attributes2, string[] attributes3, string[] attributes4, string[] attributes5)
        {
            int Result = 0;
            try
            {
                int UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                string[] attrgid = attributes0[0].ToString().Split(',');
                string[] attrnameval = attributes1[0].ToString().Split(',');
                string[] attrtypeval = attributes2[0].ToString().Split(',');
                string[] attrlenval = attributes3[0].ToString().Split(',');
                string[] attrmandatoryval = attributes4[0].ToString().Split(',');
                string[] attrorderid = attributes5[0].ToString().Split(',');
                DataSet ds = new DataSet();
                for (int i = 0; i < attrnameval.Length; i++)
                {
                    string Len = attrlenval[i].ToString();
                    if (Len == "")
                    {
                        Len = "0";
                    }

                    ds = Storageobjsrv.UpdateStorageAttri(Convert.ToInt64(attrgid[i].ToString()), attrnameval[i].ToString(), Convert.ToInt16(Len), attrtypeval[i].ToString(), attrmandatoryval[i].ToString(), Convert.ToInt32(attrorderid[i].ToString()), DgroupID, DNameID, UserID);

                }
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (Result == 1)
                {
                    //ds = serviceObj.saveStorageAtt(DeptID, UnitID, DgroupID, DNameID, UserID);
                    ViewBag.Message = "Update successfully";

                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            // return View();
            return Json(new { success = Result, JsonRequestBehavior.AllowGet });
        }
        #endregion

        //docname,docgroup,unit,dep display in partialviewedit
        public JsonResult GetDisplayDocumentValues(string group_gid)
        {
            int count = 0;
            int GroupAtrID = Convert.ToInt32(group_gid);
            string Data2 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = serviceObj.GetStorageAttributeValuesEdit(GroupAtrID);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    ViewBag.Documentname = dr["Dname_Id"].ToString();
                    ViewBag.DocumentGroup = dr["Dgroup_Id"].ToString();
                    ViewBag.orglvl1 = dr["orglvlcode1"].ToString();
                    ViewBag.orglvl2 = dr["orglvlcode2"].ToString();
                    ViewBag.orglvl3 = dr["orglvlcode3"].ToString();
                    ViewBag.orglvl4 = dr["orglvlcode4"].ToString();
                    //  var con = new { ViewBag.CountryNAME };

                    count = dt.Rows.Count;
                    Data2 = JsonConvert.SerializeObject(dt);
                }
                return Json(new { count, Data2 }, JsonRequestBehavior.AllowGet);
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

    }
}
