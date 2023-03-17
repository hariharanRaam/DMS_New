using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GleamTech.Collections;
using GleamTech.DocumentUltimate;
using GleamTech.Data;
using DMS.Model;
using DMS.Service;
using System.Data;
using GleamTech.DocumentUltimate.AspNet.UI;
using System.IO;
using DMS.Web.Filters;
using System.Web.UI;
using System.Configuration;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class SetDocAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SetDocAttributesController));  //Declaring Log4Net 
        SetDocAttributes_Model deptsModel = new SetDocAttributes_Model();
        SetDocAttributes_Model depts;
        SetDocAttributes_Service ServiceObj = new SetDocAttributes_Service();
        List<SetDocAttributes_Model> ModelObjList1 = new List<SetDocAttributes_Model>();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();

        // GET: SetDocAttributes
        public ActionResult SetDocAttributes(int? id)
        {
            string Action = Request.QueryString["Action"].ToString();
            Session["Action"] = null;
            ViewBag.doc_id = id;
            Session["Action"] = Action;
            try
            {
                DataSet ds = new DataSet();
                if (id != null && Action != null)
                {
                    if (Action == "I" || Action == "V")
                    {
                        ds = ServiceObj.InitValues(id.Value);
                        ViewBag.SubmitValue = "Save";
                    }
                    if (Action == "E")
                    {
                        ds = ServiceObj.InitEditValues(id.Value);
                        ViewBag.SubmitValue = "Update";
                    }
                    if (Action == "D")
                    {
                        ds = ServiceObj.InitEditValues(id.Value);
                        ViewBag.SubmitValue = "Delete";
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        deptsModel.FileName = ds.Tables[0].Rows[0][1].ToString();
                        deptsModel.FileType = ds.Tables[0].Rows[0][2].ToString();
                        deptsModel.HideDocArchId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                        ViewBag.FilePath = ds.Tables[0].Rows[0][3].ToString();
                        deptsModel.Filepathfrom = ds.Tables[0].Rows[0][3].ToString();
                        deptsModel.FileSize = ds.Tables[0].Rows[0][4].ToString(); 
                        if (Action == "E" || Action == "D")
                        {
                            deptsModel.VersionDate = Convert.ToDateTime(ds.Tables[0].Rows[0][4]);
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                      //  deptsModel.DeptName = ds.Tables[1].Rows[0][3].ToString();
                      //  deptsModel.UnitName = ds.Tables[1].Rows[0][5].ToString();
                        deptsModel.CateName = ds.Tables[1].Rows[0][3].ToString();
                        deptsModel.SubCateName = ds.Tables[1].Rows[0][5].ToString();
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ViewBag.AttributeCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                    }
                    if (ds.Tables[4].Rows.Count > 0) {
                        ViewBag.Orglvlval4 = ds.Tables[4].Rows[0][8].ToString();
                        ViewBag.Orglvlval3 = ds.Tables[4].Rows[0][7].ToString();
                        ViewBag.Orglvlval2 = ds.Tables[4].Rows[0][6].ToString();
                        ViewBag.Orglvlval1 = ds.Tables[4].Rows[0][5].ToString();
                    }
                    DataTable dt = new DataTable();
                    dt = ds.Tables[3];
                    ViewBag.Attribute = dt;
                    Session["dt"] = dt;
                    int ctlval = 0;

                    string type;
                    int LovId;
                    DataTable dt1 = new DataTable();
                    foreach (DataRow dr in dt.Rows)
                    {
                        depts = new SetDocAttributes_Model();
                        depts.attrbname = dr["Atr_Name"].ToString();
                        depts.attrid = dr["Atr_Id"].ToString();
                        type = dr["Atr_Type"].ToString();
                        if (type == "Lov Name")
                        {
                            LovId = Convert.ToInt32(dr["Lov_Id"].ToString());
                            dt1 = ServiceObj.getmasterval(LovId, type);

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
                            depts.LovName = item8;
                        }

                        if (type == "AutoNumber") {
                            LovId = Convert.ToInt32(dr["Lov_Id"].ToString());
                            dt1 = ServiceObj.getmasterval(LovId, type);

                          //  List<SelectListItem> item8 = new List<SelectListItem>();
                            foreach (DataRow dr1 in dt1.Rows)
                            {

                                depts.autonumbervalue = dr1["master_name"].ToString();
                                    depts.autonumberrowid = dr1["master_code"].ToString();
                             
                            }
                            //ViewBag.Lovexl_Name = item8;
                          //  depts.LovName = item8;
                        }

                        if (Action == "E" || Action == "D")
                        {
                            depts.attrbdesc = dr["Attribdtl_Description"].ToString();
                        }
                        else
                        {
                            depts.attrbdesc = null;
                        }
                        depts.attrbtype = dr["Atr_Type"].ToString();
                        depts.attrblen = dr["Atr_Length"].ToString();
                        depts.attrbMand = dr["Atr_Mandotry"].ToString();
                        if (Action == "E")
                        {
                            depts.attrbmode = "E";
                            deptsModel.attrbmode = "E";
                        }
                        if (Action == "D")
                        {
                            depts.attrbmode = "D";
                            deptsModel.attrbmode = "D";
                        }
                        if (Action == "I")
                        {
                            depts.attrbmode = "I";
                            deptsModel.attrbmode = "I";
                        }
                        depts.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
                        deptsModel.dept.Add(depts);
                        ctlval++;
                    }
                    //ViewBag.RowCount = dt.Rows.Count;
                    Session["AtrCount"] = dt.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View(deptsModel);
        }

        [HttpPost]
        public ActionResult ValidateAttributes(string[] form, SetDocAttributes_Model deptsModel)
        {
            string Data1 = "";
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt = Session["dt"] as DataTable;
                for (int i = 0; i < form.Count(); i++)
                {
                    SetDocAttributes_Model model = new SetDocAttributes_Model();
                    model.AtrLabel1 = dt.Rows[i][1].ToString();
                    model.AtrType = dt.Rows[i][3].ToString();
                    model.AtrLen = dt.Rows[i][2].ToString();
                    model.AtrMand = dt.Rows[i][4].ToString();
                    model.AtrLovId = Convert.ToInt32(dt.Rows[i][5].ToString());
                    model.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                    model.AtrText1 = form[i];
                    ModelObjList1.Add(model);
                }
                dt1 = ServiceObj.ValidateAttributes(deptsModel, ModelObjList1);
                if (dt1.Rows.Count > 0)
                {
                    Data1 = dt1.Rows[0][0].ToString();
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveAttributes(string[] form, SetDocAttributes_Model deptsModel)
        {
            int Result = 0;
            string Action = Session["Action"].ToString();
            try
            {
                DataTable dt = Session["dt"] as DataTable;
                int AtrCount = Convert.ToInt32(Session["AtrCount"]);

                if (Action == "I")
                {
                    string category = deptsModel.CateName;
                    string subcategory = deptsModel.SubCateName;

                    //string Catename = "D:\\" + category;
                    string filePath = ConfigurationManager.AppSettings["Path"].ToString();

                    string Catename = filePath + category;
                    string SubCatename = subcategory;

                    string ExistingfilePath = deptsModel.Filepathfrom;
                    deptsModel.Filepathto = filePath + category + "\\" + SubCatename + "\\" + deptsModel.FileName;

                    bool loggingDirectoryExists = false;
                    DirectoryInfo ObjDirectoryInfo = new DirectoryInfo(Catename);

                    if (ObjDirectoryInfo.Exists)
                    {
                        loggingDirectoryExists = true;
                        DirectoryInfo ObjSubDirectoryInfo = new DirectoryInfo(SubCatename);
                        if (ObjSubDirectoryInfo.Exists)
                        {
                            loggingDirectoryExists = true;
                        }
                        else
                        {
                            ObjSubDirectoryInfo = ObjDirectoryInfo.CreateSubdirectory(SubCatename);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(Catename);
                        ObjDirectoryInfo = new DirectoryInfo(Catename);
                        DirectoryInfo ObjSubDirectoryInfo = ObjDirectoryInfo.CreateSubdirectory(SubCatename);
                        loggingDirectoryExists = true;
                    }

                    for (int i = 0; i < AtrCount; i++)
                    {
                        SetDocAttributes_Model model = new SetDocAttributes_Model();
                        model.AtrLabel1 = dt.Rows[i][1].ToString();
                        model.AtrType = dt.Rows[i][3].ToString();
                        model.AtrLen = dt.Rows[i][2].ToString();
                        model.AtrMand = dt.Rows[i][4].ToString();
                        model.AtrLovId = Convert.ToInt32(dt.Rows[i][5].ToString());
                        model.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                        model.AtrText1 = form[i];
                        ModelObjList1.Add(model);
                    }
                    //passing model object and model list to service class
                    Result = ServiceObj.SaveProperties(deptsModel, ModelObjList1);
                    if (Result == 1)
                    {
                        //ViewBag.Message = "File uploaded successfully";
                    }
                    string Source = ExistingfilePath;
                    FileInfo objfile = new FileInfo(Source);
                    {
                        string Destination = Path.Combine(Catename, SubCatename);
                        System.IO.File.Move(Source, Destination + "\\" + deptsModel.FileName);
                    }
                }
                if (Action == "E")
                {
                    for (int i = 0; i < AtrCount; i++)
                    {
                        SetDocAttributes_Model model = new SetDocAttributes_Model();
                        model.AtrLabel1 = dt.Rows[i][1].ToString();
                        model.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                        model.AtrText1 = form[i];
                        ModelObjList1.Add(model);
                    }
                    //passing model object and model list to service class
                    Result = ServiceObj.UpdateProperties(deptsModel, ModelObjList1);

                    if (Result == 1)
                    {
                        Result = 2;
                        //ViewBag.Message = "File Edited successfully";
                    }
                }
                if (Action == "D")
                {
                    //passing model object and model list to service class
                    Result = ServiceObj.Deletefile(deptsModel);

                    if (Result == 1)
                    {
                        Result = 3;
                        //ViewBag.Message = "File Deleted successfully";
                    }
                }

           //     int currentdocid = ViewBag.doc_id;
                //int currentdocid = deptsModel.HideDocArchId;
                //DataTable griddatalist = Session["docgridlist"] as DataTable;
                //int rownumber_ = 0;
                //for (int y = 0; y < griddatalist.Rows.Count; y++)
                //{
                //    int tempdocid = Convert.ToInt32(griddatalist.Rows[y]["Sl No"].ToString());
                //    if (tempdocid == currentdocid)
                //    {
                //        rownumber_ = y;
                //        break;
                //    }
                //}
                //griddatalist.Rows[rownumber_].Delete();
                //griddatalist.AcceptChanges();
                //Session["docgridlist"] = null;
                //Session["docgridlist"] = griddatalist;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            var urlBuilder = new System.Web.Mvc.UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("GetAllDocuments", "GetAllDocuments");
            var urlforedit = urlBuilder.Action("EditIndexedDocument", "EditIndexedDocument");
            return Json(new { status = Result, redirectUrl = url, redirectUrl1 = urlforedit });
        }


        public ActionResult PreviousNextgridfetch(int current_docid, string grid_mode) 
        {
            int docid_ = 0;
            DataTable gridresult = new DataTable();
            gridresult = Session["docgridlist"] as DataTable;

            for (int k = 0; k < gridresult.Rows.Count; k++) {
                int _doc = Convert.ToInt32(gridresult.Rows[k]["SL No"].ToString());
                if (_doc == current_docid) {
                    if (grid_mode == "P") {
                        if ((k - 1) < 0)
                        {
                            docid_ = Convert.ToInt32(gridresult.Rows[gridresult.Rows.Count - 1]["SL No"].ToString());
                            break;
                        }
                        else {
                            docid_ = Convert.ToInt32(gridresult.Rows[k - 1]["SL No"].ToString());
                            break;
                        }
                    }
                    else if (grid_mode == "N") {
                        if ((k + 1) >= gridresult.Rows.Count)
                        {
                            docid_ = Convert.ToInt32(gridresult.Rows[0]["SL No"].ToString());
                            break;
                        }
                        else {
                            docid_ = Convert.ToInt32(gridresult.Rows[k + 1]["SL No"].ToString());
                            break;
                        }
                    }
                }
            }

            //return RedirectToAction("SetDocAttributes", "SetDocAttributes", new { id = docid_, @Action = "I" });
            string queryString = "?id=" + docid_ + "&Action="+"I";
            return Redirect(Url.Action("SetDocAttributes") + queryString);
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