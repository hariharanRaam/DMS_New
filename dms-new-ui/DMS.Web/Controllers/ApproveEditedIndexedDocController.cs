using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using System.Data;
using DMS.Service;
using DMS.Model;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    public class ApproveEditedIndexedDocController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SetDocAttributesController));  //Declaring Log4Net 
        ApproveEditedIndexedDoc_Service ServiceObj = new ApproveEditedIndexedDoc_Service();
        ApproveEditedIndexedDoc_Model ModelObj = new ApproveEditedIndexedDoc_Model();
        string Mode="";
        // GET: ApproveEditedIndexedDoc
        public ActionResult ApproveEditedIndexedDoc(int? id)
        {
            try
            {
                Mode= Request.QueryString["Action"].ToString();
                Session["Action"] = null;
                Session["Action"] = Mode;
                DataSet ds = new DataSet();
                if (id != null)
                {
                    ds = ServiceObj.InitValues(id.Value, Mode);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ModelObj.FileName = ds.Tables[0].Rows[0][1].ToString();
                        ModelObj.FileType = ds.Tables[0].Rows[0][2].ToString();
                        ModelObj.HideDocArchId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                        ViewBag.FilePath = ds.Tables[0].Rows[0][3].ToString();
                        ModelObj.Filepathfrom = ds.Tables[0].Rows[0][3].ToString();
                        ModelObj.VersionDate = Convert.ToDateTime(ds.Tables[0].Rows[0][4].ToString());
                        ModelObj.VersionName = ds.Tables[0].Rows[0][5].ToString();
                        ModelObj.Signature = ds.Tables[0].Rows[0][6].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ModelObj.DeptName = ds.Tables[1].Rows[0][3].ToString();
                        ModelObj.UnitName = ds.Tables[1].Rows[0][5].ToString();
                        ModelObj.CateName = ds.Tables[1].Rows[0][7].ToString();
                        ModelObj.SubCateName = ds.Tables[1].Rows[0][9].ToString();
                    }
                    DataTable dt = new DataTable();
                    dt = ds.Tables[3];
                    foreach (DataRow dr in dt.Rows)
                    {
                        ApproveEditedIndexedDoc_Model depts = new ApproveEditedIndexedDoc_Model();
                        depts.attrbname = dr["Atr_Name"].ToString();
                        depts.attrid = dr["Atr_Id"].ToString();
                        depts.attrbdesc = dr["Attribdtl_Description"].ToString();
                        depts.attrbtype = dr["Atr_Type"].ToString();
                        depts.attrblen = dr["Atr_Length"].ToString();
                        depts.attrbMand = dr["Atr_Mandotry"].ToString();
                        ModelObj.dept.Add(depts);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View(ModelObj);
        }

        public ActionResult Approvetoupdatefile(int? DocID)
        {
            try
            {
                Mode = Session["Action"].ToString();
                Int32 Result;
                Result = ServiceObj.Approvetoupdatefile(DocID, Mode);
                var urlBuilder = new System.Web.Mvc.UrlHelper(Request.RequestContext);
                var url = urlBuilder.Action("GetEditedIndexedDoc", "GetEditedIndexedDoc");
                var url1 = urlBuilder.Action("GetDeletedIndexedDocuments", "GetDeletedIndexedDocuments");
                return Json(new { status = Result, redirectUrl = url, redirectUrl1 = url1 });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult Rejectneedtoupdatefile(int? DocID)
        {
            try
            {
                Mode = Session["Action"].ToString();
                Int32 Result;
                Result = ServiceObj.Reject(DocID, Mode);
                var urlBuilder = new System.Web.Mvc.UrlHelper(Request.RequestContext);
                var url = urlBuilder.Action("GetEditedIndexedDoc", "GetEditedIndexedDoc");
                var url1 = urlBuilder.Action("GetDeletedIndexedDocuments", "GetDeletedIndexedDocuments");
                return Json(new { status = Result, redirectUrl = url, redirectUrl1 = url1 });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

    }
}