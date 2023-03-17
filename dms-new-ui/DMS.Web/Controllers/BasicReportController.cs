using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using DMS.Web.Filters;
using DMS.Model;
using DMS.Service;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class BasicReportController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BasicReportController));  //Declaring Log4Net. 
        BasicReport_Service serviceObj = new BasicReport_Service();
        BasicReport_Model modelObj = new BasicReport_Model();
        SetDocAttributes_Model deptsModel1 = new SetDocAttributes_Model();
        SetDocAttributes_Model depts1;
        SetDocAttributes_Service ServiceObj1 = new SetDocAttributes_Service();
        Search_Service ServiceObj = new Search_Service();
        ViewDocument_Services ServiceObj2 = new ViewDocument_Services();
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

        // GET: BasicReport
        public ActionResult BasicReport()
        {
            try
            {
                modelObj.Lbl_Dept = Session["Dept_Label"].ToString();
                modelObj.Lbl_Unit = Session["Unit_Label"].ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View(modelObj);
        }


       // public ActionResult ViewIndexedDocDetails(int filegid)
        public ActionResult ShowIndexedDocDetailsPartialVw(int filegid, string modetype)
        {
            int id = filegid;
            ViewBag.filegid = filegid;
            ViewBag.mode = modetype;
            try
            {
                DataSet ds = new DataSet();
                ds = ServiceObj1.InitEditValues(id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    deptsModel1.FileName = ds.Tables[0].Rows[0][1].ToString();
                    deptsModel1.FileType = ds.Tables[0].Rows[0][2].ToString();
                    deptsModel1.HideDocArchId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    ViewBag.FilePath = ds.Tables[0].Rows[0][3].ToString();
                    //string url="https://localhost:44378/api/File/Download?subDirectory=" + ViewBag.FilePath;
                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri(url);
                        
                    //    HttpContent content = new StringContent(JsonConvert.SerializeObject(""),System.Text.UTF8Encoding.UTF8, "application/json");
                        
                    //    var response = client.GetByteArrayAsync(url);
                    //    int i = 0;
                    //    deptsModel1.filedata = response.Result;
                        
                    //}

                    deptsModel1.Filepathfrom = ds.Tables[0].Rows[0][3].ToString();
                    deptsModel1.FileSize = ds.Tables[0].Rows[0][5].ToString(); ;
                    deptsModel1.VersionDate = Convert.ToDateTime(ds.Tables[0].Rows[0][4]);

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    // deptsModel1.DeptName = ds.Tables[1].Rows[0][3].ToString();
                    // deptsModel1.UnitName = ds.Tables[1].Rows[0][5].ToString();
                    deptsModel1.CateName = ds.Tables[1].Rows[0][3].ToString();
                    deptsModel1.SubCateName = ds.Tables[1].Rows[0][5].ToString();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    ViewBag.Orglvlval4 = ds.Tables[5].Rows[0][8].ToString();
                    ViewBag.Orglvlval3 = ds.Tables[5].Rows[0][7].ToString();
                    ViewBag.Orglvlval2 = ds.Tables[5].Rows[0][6].ToString();
                    ViewBag.Orglvlval1 = ds.Tables[5].Rows[0][5].ToString();
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ViewBag.AttributeCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                }
                DataTable dt = new DataTable();
                dt = ds.Tables[3];
                //ViewBag.Attribute = dt;
                //Session["dt"] = dt;
                int ctlval = 0;

                string type;
                int LovId;
                DataTable dt1 = new DataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    depts1 = new SetDocAttributes_Model();
                    depts1.attrbname = dr["Atr_Name"].ToString();
                    depts1.attrid = dr["Atr_Id"].ToString();
                    type = dr["Atr_Type"].ToString();
                    if (type == "Lov Name")
                    {
                        LovId = Convert.ToInt32(dr["Lov_Id"].ToString());
                        dt1 = ServiceObj.getmasterval(LovId,type);

                        List<SelectListItem> item8 = new List<SelectListItem>();
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            item8.Add(new SelectListItem
                            {
                                Text = dr1["master_name"].ToString(),
                                Value = dr1["master_code"].ToString()
                            });
                        }
                        //ViewBag.Lovexl_Name = item8;
                        depts1.LovName = item8;
                    }
                    if (type == "Datetime")
                    {
                        depts1.attrbdesc = dr["Attribdtl_Description"].ToString().Substring(0, 10);
                    }
                    else
                    { depts1.attrbdesc = dr["Attribdtl_Description"].ToString(); }
                  //  depts1.attrbdesc = dr["Attribdtl_Description"].ToString();
                    depts1.attrbtype = dr["Atr_Type"].ToString();
                    depts1.attrblen = dr["Atr_Length"].ToString();
                    depts1.attrbMand = dr["Atr_Mandotry"].ToString();
                    depts1.attrbmode = "D";
                    depts1.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
                    deptsModel1.dept.Add(depts1);
                    ctlval++;
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ViewBag.AttributeCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                }
                DataTable dt6 = new DataTable();
                dt6 = ds.Tables[4];
                //int ctlval1= 0;

                int ctlval1 = 0;

                string type1;
                // 22-03-2019
                //  DataTable dt9 = new DataTable();
                List<SelectListItem> item9 = new List<SelectListItem>();
                foreach (DataRow dr2 in dt6.Rows)
                {
                    depts1 = new SetDocAttributes_Model();
                    depts1.Satr_Name = dr2["Satr_Name"].ToString();
                    depts1.Satr_Length = Convert.ToInt32(dr2["Satr_Length"].ToString());
                    depts1.Satr_Type = dr2["Satr_Type"].ToString();
                    depts1.Satr_Mandotry = dr2["Satr_Mandotry"].ToString();
                    depts1.Sattribdtl_Description = dr2["Sattribdtl_Description"].ToString();
                    deptsModel1.dept.Add(depts1);
                    ctlval++;
                }

              //  return PartialView("ShowIndexedDocDetailsPartialVw", deptsModel1);
                return View(deptsModel1);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
               // return PartialView("ShowIndexedDocDetailsPartialVw", deptsModel1);
                return View(deptsModel1);
            }
        }

        #region get dropdown records,
        public JsonResult GetBasicReportDetails(string Master = "", Int64 MasterID = 0)   //Method for fetching all dropdown values
        {
            List<BasicReport_Model> lst_ = new List<BasicReport_Model>();
            try
            {
                Int64 UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                lst_ = serviceObj.GetBasicReportDetails(Master, MasterID, UserID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(lst_, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region dropdown onchange methods,
        public JsonResult onChangeDropdowns(string Master = "", Int64 MasterID = 0)   //Method for fetching all dropdown values
        {
            List<BasicReport_Model> lst_dept = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_unit = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_docgroup = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_docname = new List<BasicReport_Model>();
            DataSet ds = new DataSet();
            try
            {
                Int64 UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                ds = serviceObj.onChangeDropdowns(Master, MasterID, UserID);
                if (ds.Tables.Count > 0)
                {
                    if (Master == "GetDocName")
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.CateID = Convert.ToInt64(row["Dgroup_Id"].ToString());
                            modelObj_.CateName = row["Dgroup_Name"].ToString();
                            lst_docgroup.Add(modelObj_);
                        }

                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.mastercode = row["master_code"].ToString();
                            modelObj_.mastername = row["master_name"].ToString();
                            lst_dept.Add(modelObj_);

                        }
                    }
                    if (Master == "GetDocGroup")
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.SubCateID = Convert.ToInt64(row["Dname_Id"].ToString());
                            modelObj_.SubCateName = row["Dname_Name"].ToString();
                            lst_docname.Add(modelObj_);
                        }
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.mastercode = row["master_code"].ToString();
                            modelObj_.mastername = row["master_name"].ToString();
                            lst_dept.Add(modelObj_);

                        }
                    }
                    //if (Master == "GetUnit")
                    //{
                    //    foreach (DataRow row in ds.Tables[0].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.DeptID = Convert.ToInt64(row["Dept_Id"].ToString());
                    //        modelObj_.DeptName = row["Dept_Name"].ToString();
                    //        lst_dept.Add(modelObj_);
                    //    }
                    //    foreach (DataRow row in ds.Tables[1].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.CateID = Convert.ToInt64(row["Dgroup_Id"].ToString());
                    //        modelObj_.CateName = row["Dgroup_Name"].ToString();
                    //        lst_docgroup.Add(modelObj_);
                    //    }
                    //    foreach (DataRow row in ds.Tables[2].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.SubCateID = Convert.ToInt64(row["Dname_Id"].ToString());
                    //        modelObj_.SubCateName = row["Dname_Name"].ToString();
                    //        lst_docname.Add(modelObj_);
                    //    }
                    //}
                    //if (Master == "GetDept")
                    //{
                    //    foreach (DataRow row in ds.Tables[0].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.UnitID = Convert.ToInt64(row["unit_id"].ToString());
                    //        modelObj_.UnitName = row["unit_name"].ToString();
                    //        lst_unit.Add(modelObj_);
                    //    }
                    //    foreach (DataRow row in ds.Tables[1].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.CateID = Convert.ToInt64(row["Dgroup_Id"].ToString());
                    //        modelObj_.CateName = row["Dgroup_Name"].ToString();
                    //        lst_docgroup.Add(modelObj_);
                    //    }
                    //    foreach (DataRow row in ds.Tables[2].Rows)
                    //    {
                    //        BasicReport_Model modelObj_ = new BasicReport_Model();
                    //        modelObj_.SubCateID = Convert.ToInt64(row["Dname_Id"].ToString());
                    //        modelObj_.SubCateName = row["Dname_Name"].ToString();
                    //        lst_docname.Add(modelObj_);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { lst_dept, lst_docgroup, lst_docname }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get uploaded list to grid,
        public ActionResult GetUploadedFiles(BasicReport_Model modelObj)  //Method for fetching all DocName Records
        {
            string Data1 = "", Data2 = "";
            DataSet dset = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                modelObj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                dset = serviceObj.GetUploadedFiles(modelObj);
                if (dset.Tables.Count > 1)
                {
                    dt = dset.Tables[0];
                    dt1 = dset.Tables[1];
                }
                else if (dset.Tables.Count > 0)
                {
                    dt = dset.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            Data1 = JsonConvert.SerializeObject(dt);
            Data2 = JsonConvert.SerializeObject(dt1);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Download file,
        public JsonResult CheckFileExists(string absolutePath)
        {
            string message = string.Empty;
            try
            {
                if (System.IO.File.Exists(absolutePath))
                {
                    message = "fileexists";
                }
                else
                {
                    message = "filenotexists";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                message = ex.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string filePath, string filename)
        {
            byte[] fileBytes = GetFile(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        #endregion

        #region Customizing Search Report
        public ActionResult CustomizingSearch()
        {
            try
            {
                modelObj.Lbl_Dept = Session["Dept_Label"].ToString();
                modelObj.Lbl_Unit = Session["Unit_Label"].ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View(modelObj);
        }

        public JsonResult GetAttributes(string inAction = "", Int64 Department = 0, Int64 Unit = 0, Int64 DGroup = 0, Int64 DName = 0)
        {
            List<BasicReport_Model> lst_ = new List<BasicReport_Model>();
            try
            {
                modelObj.inAction = inAction;
                modelObj.DeptID = Department;
                modelObj.UnitID = Unit;
                modelObj.CateID = DGroup;
                modelObj.SubCateID = DName;
                lst_ = serviceObj.GetAttributes(modelObj);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(lst_, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetIndexedFileDetails(Int64 DGroup = 0, Int64 DName = 0, string dbCondition = "", string action = "", string activeflag = "")
        {
            string Data1 = "", Data2 = "";
            DataSet dset = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                modelObj.Atr_Conditions = dbCondition;

                modelObj.CateID = DGroup;
                modelObj.SubCateID = DName;
                modelObj.Atr_load_action = action;
                modelObj.activeflag = activeflag;
                dset = serviceObj.GetIndexedFileDetails(modelObj);
                if (dset.Tables.Count > 1)
                {
                    dt = dset.Tables[0];
                    dt1 = dset.Tables[1];
                }
                else if (dset.Tables.Count > 0)
                {
                    dt = dset.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            Data1 = JsonConvert.SerializeObject(dt);
            Data2 = JsonConvert.SerializeObject(dt1);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult GetIndexedDocumentDetails(Int64 DGroup = 0, Int64 DName = 0, string dbCondition = "", string action = "", string activeflag = "")
        {
            string Data1 = "", Data2 = "";
            DataSet dset = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                modelObj.Atr_Conditions = dbCondition;

                modelObj.CateID = DGroup;
                modelObj.SubCateID = DName;
                modelObj.Atr_load_action = action;
                modelObj.activeflag = activeflag;
                dset = serviceObj.GetIndexedDocumentDetails(modelObj);
                if (dset.Tables.Count > 1)
                {
                    dt = dset.Tables[0];
                    dt1 = dset.Tables[1];
                }
                else if (dset.Tables.Count > 0)
                {
                    dt = dset.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            Data1 = JsonConvert.SerializeObject(dt);
            Data2 = JsonConvert.SerializeObject(dt1);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetIndexedFileExportDetails(Int64 DGroup = 0, Int64 DName = 0, string dbCondition = "", string action = "")
        {
            string Data1 = "", Data2 = "";
            DataSet dset = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                modelObj.Atr_Conditions = dbCondition;

                modelObj.CateID = DGroup;
                modelObj.SubCateID = DName;
                modelObj.Atr_load_action = action;
                modelObj.activeflag = "Y";
                dset = serviceObj.GetIndexedFileDetails(modelObj);
                if (dset.Tables.Count > 1)
                {
                    dt = dset.Tables[0];
                    dt1 = dset.Tables[1];
                }
                else if (dset.Tables.Count > 0)
                {
                    dt = dset.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
          //  Data1 = JsonConvert.SerializeObject(dt);
          //  Data2 = JsonConvert.SerializeObject(dt1);

            //DataTable dtnew = (DataTable)Session["Errdatatable"];
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DMS Reports.xls"));
            Response.ContentType = "application/vnd.ms-excel";
           // DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
            //return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowlinkandInterfileDocs(Int64 Attribid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();
                DataTable dt6 = new DataTable();
                ds1 = ServiceObj2.GetInterFilingLinkedDocuments(0, 0, Attribid);
                ds = ServiceObj2.GetLinkedDocuments(0, 0, Attribid);
                ds2 = ServiceObj2.GetRetrievalaudit(Attribid);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dt2 = ds.Tables[1];
                    }
                }

                if (ds1.Tables.Count > 1)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        dt3 = ds1.Tables[0];
                    }

                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        dt4 = ds1.Tables[1];
                    }
                }
                if (ds2.Tables.Count > 1)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        dt5 = ds2.Tables[0];
                    }

                    if (ds2.Tables[1].Rows.Count > 0)
                    {
                        dt6 = ds2.Tables[1];
                    }
                }
                string Data3, Data1, Data2, Data4, Data5, Data6;
                Data1 = JsonConvert.SerializeObject(dt1);
                Data2 = JsonConvert.SerializeObject(dt2);
                Data3 = JsonConvert.SerializeObject(dt3);
                Data4 = JsonConvert.SerializeObject(dt4);
                Data5 = JsonConvert.SerializeObject(dt5);
                Data6 = JsonConvert.SerializeObject(dt6);
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }
        }
    

    }
}