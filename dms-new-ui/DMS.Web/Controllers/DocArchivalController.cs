using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using DMS.Model;
using DMS.Service;
using DMS.Web.Filters;
using System.Configuration;
using System.Data.OleDb;
using Newtonsoft.Json;


namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DocArchivalController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocArchivalController));  //Declaring Log4Net       
        ConfigureAttributes_Service ServiceObj1 = new ConfigureAttributes_Service();//Creating service object
        DocArchival_Model ModelObj = new DocArchival_Model();  //Model Object
        DocArchival_Model.DocArchivalFileCache modelcache = new DocArchival_Model.DocArchivalFileCache();
        DocArchival_Service ServiceObj = new DocArchival_Service();  //Service Object

        public List<DocGroupMaster_Model> DocGroup = new List<DocGroupMaster_Model>();
        DocGroupMaster_Model Docgrpmdlobj = new DocGroupMaster_Model();
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

        public JsonResult getScreenData(int screen_id, string screen_name)
        {
            string Data1 = "";
            string user_gcode = Session["Emp_Id"].ToString();
            DataTable dt = new DataTable();
            try
            {
                dt = UserGroups_Service.getScreenData_Srv(screen_id, screen_name, Convert.ToInt32(user_gcode));
                Data1 = JsonConvert.SerializeObject(dt);

            }
            catch (Exception er)
            {

            }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
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

        // GET: DocArchival
        [HttpGet]
        public ActionResult DocArchival_Multiple()
        {
            Dept_Union_Cat_SubCat_Model deptsModel = new Dept_Union_Cat_SubCat_Model();
            Session["fileid"] = 0;
            Session["Multiple"] = null;
            return View(deptsModel);
        }
        public ActionResult Default()
        {
            Dept_Union_Cat_SubCat_Model deptsModel = new Dept_Union_Cat_SubCat_Model();
          //  Session["fileid"] = 0;
            return View();
        }

        /****** Multiple Document Archival/Uploading Controller Method - START ******/
        [HttpPost]
        //public ActionResult DocArchival_Multiple(HttpPostedFileBase[] FileUpload1, Dept_Union_Cat_SubCat_Model DCObj)
        //{
        public ActionResult Save_Multiple_File(int docgroup, int docname, string useracceptance, string activeflag)
        {
            string message = "";
            Int64 nooffiles = 0;
            string checkfile = "";
            int Result;
            try
            {
                List<HttpPostedFileBase> FileUpload1 = Session["Multiple"] as List<HttpPostedFileBase>;
                if (FileUpload1 != null)
                {
                 //  ModelObj.DeptId = department;
                 //   ModelObj.UnitId = unit;
                    ModelObj.CatgId = docgroup;
                    ModelObj.SubCatgId = docname;
                    ModelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                    ModelObj.activeflag = activeflag;
                    if (useracceptance == "allow")
                    {
                        //Passing all values to Service class through Model object list.
                        Result = ServiceObj.SaveDocInfo(ModelObj, FileUpload1);
                        if (Result == 1)
                        {
                            message = "inserted";
                            nooffiles = FileUpload1.Count();
                            //ViewBag.Message = FileUpload1.Count() + " " + "Files Uploaded Scuccessfully!....";
                        }
                    }
                    else
                    {
                        checkfile = ServiceObj.checkfilemultiple(ModelObj, FileUpload1);
                        if (checkfile == "exist")
                        {
                            message = "exist";
                        }
                        else
                        {
                            //Passing all values to Service class through Model object list.
                            Result = ServiceObj.SaveDocInfo(ModelObj, FileUpload1);
                            if (Result == 1)
                            {
                                message = "inserted";
                                nooffiles = FileUpload1.Count();
                            }
                        }
                    }
                }
                return Json(new {message,nooffiles}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            //Dept_Union_Cat_SubCat_Model deptsModel = new Dept_Union_Cat_SubCat_Model();
            //return View(deptsModel);
        }
        /****** Multiple Document Archival/Uploading Controller Method - END ******/

        /****** Single Document Archival/Uploading Controller Method - START ******/
        [HttpPost]
        public ActionResult Save_Single_File(int docgroup, int docname, string action, int fileid, string useracceptance,string activeflag)
        {
            string message = "";
            string checkfile = "";
            int result;
            try
            {
                string ActionMode = action;
                Int64? FileId = fileid;
                ModelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
             //   ModelObj.DeptId = department;
              //  ModelObj.UnitId = unit;
                ModelObj.CatgId = docgroup;
                ModelObj.SubCatgId = docname;
                ModelObj.FileID = fileid;
                ModelObj.activeflag = activeflag;
                HttpPostedFileBase file = Session["Single"] as HttpPostedFileBase;
                if (ActionMode == "I")
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        if (useracceptance == "allow")
                        {
                            //Passing all values to Service class through Model object list.
                            result = ServiceObj.SaveSingleFile(ModelObj, file);
                            if (result == 1)
                            {
                                message = "inserted";
                                //ViewBag.Message = "File uploaded successfully..!!";
                            }
                        }
                        else
                        {
                            checkfile = ServiceObj.checkfile(ModelObj, file);
                        }
                        if (checkfile == "exist")
                        {
                            message = "exist";
                        }
                        else
                        {
                            //Passing all values to Service class through Model object list.
                            result = ServiceObj.SaveSingleFile(ModelObj, file);
                            if (result == 1)
                            {
                                message = "inserted";
                            }
                        }
                    }
                }

                if (ActionMode == "E")
                {
                     result = 0;
                    if (file != null && file.ContentLength > 0)
                    {
                        //Passing all values to Service class through Model object list.
                        result = ServiceObj.EditScannedDocwithfile(ModelObj, file);
                    }
                    else
                    {
                        result = ServiceObj.EditScannedDocwithoutfile(ModelObj);
                    }
                    if (result == 1)
                    {
                        message = "edited";
                    }
                }

                if (ActionMode == "D")
                {
                    if (FileId != null)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = ServiceObj.DeleteScannedFile(FileId, ActionMode, ModelObj);
                        if (dt1.Rows.Count > 0)
                        {
                            string fullPath = dt1.Rows[0]["DOC_Arch_FilePath"].ToString();
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                                //ViewBag.Message = "File Deleted successfully..!!";
                            }
                            message = "deleted";

                        }
                    }
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            //return View(deptsModel);
        }
        /****** Single Document Archival/Uploading Controller Method - END ******/

        [HttpGet]
        public ActionResult DocArchival_Single(int? id, string mode, string filename,string activeflag)
        {
            Dept_Union_Cat_SubCat_Model deptsModel = new Dept_Union_Cat_SubCat_Model();
            Session["fileid"] = null;
            if (id != null)
            {
                Session["fileid"] = id;
                deptsModel.ActionMode = mode;
                deptsModel.FileId = id;
                ViewBag.FileName = filename;
                ViewBag.activeflag = activeflag;
            }
            else
            {
                deptsModel.ActionMode = "I";
                Session["fileid"] = 0;
                deptsModel.FileId = 0;
            }
            return View(deptsModel);
        }

        [HttpPost]
        public void SingleFile_onchange(HttpPostedFileBase file)
        {
            Session["Single"] = null;
            Session["Single"] = file;
        }

        public ActionResult deleteSessionfile(string GridID)
        {
            int removefileno = Convert.ToInt32(GridID);
            List<HttpPostedFileBase> multiplefiles = new List<HttpPostedFileBase>();
            List<HttpPostedFileBase> UploadedFiles = Session["Multiple"] as List<HttpPostedFileBase>;
            for (int i = 0; i < UploadedFiles.Count; i++)
            {
                if (i != removefileno)
                {
                    multiplefiles.Add(UploadedFiles[i]);
                }
            }
            Session["Multiple"] = null;
            Session["Multiple"] = multiplefiles;
            return View();
        }

        [HttpPost]
        public ActionResult MultipleFile_onchange()
        {
            string Data1 = "", Data2 = "", Data3 = "";
            string checkfile = "";
            HttpFileCollectionBase files = Request.Files;
            List<HttpPostedFileBase> multiplefiles = new List<HttpPostedFileBase> ();
            string docname = Request["DocNameid"];
            string docgroup = Request["Docgroupid"];
            ModelObj.SubCatgId = Convert.ToInt32(docname);
            ModelObj.CatgId = Convert.ToInt32(docgroup);
            List<HttpPostedFileBase> UploadedFiles = Session["Multiple"] as List<HttpPostedFileBase>;
            if (UploadedFiles != null)
            {
                multiplefiles = UploadedFiles;
            }
            else {
                multiplefiles = new List<HttpPostedFileBase>();
            }
            for (int i = 0; i < files.Count; i++)
            {
                var isavl = false;
                if (UploadedFiles != null)
                {
                    for (int h = 0; h < UploadedFiles.Count; h++)
                    {
                        if (UploadedFiles[h].FileName == files[i].FileName)
                        {
                            isavl = true;
                            break;
                        }
                    }
                }
                if (!isavl) {
                    multiplefiles.Add(files[i]);
                }
            }
            Session["Multiple"] = null;
            Session["Multiple"] = multiplefiles;
            List<DocArchival_Model.DocArchivalFileCache> cachelist = new List<DocArchival_Model.DocArchivalFileCache>();
            for (int l = 0; l < multiplefiles.Count; l++) {
                modelcache = new DocArchival_Model.DocArchivalFileCache();
                FileInfo fi = new FileInfo(multiplefiles[l].FileName);
                modelcache.Fileno = l.ToString();
                modelcache.FileName = multiplefiles[l].FileName;
                modelcache.FileSize = FileSizeFormatter.FormatSize(multiplefiles[l].ContentLength);
                checkfile = ServiceObj.checkfilemultiplecache(ModelObj, multiplefiles[l]);
                if (checkfile != "exist")
                {
                    modelcache.FileIsExists = "Not Exist";
                }
                else {
                    modelcache.FileIsExists = "Exist";
                }
                cachelist.Add(modelcache);
            }
            Data1 = JsonConvert.SerializeObject(cachelist);
            DocArchival_Model.DocArchivalFileCache modelcache1 = new DocArchival_Model.DocArchivalFileCache();
            modelcache1.FileName = "String";
            modelcache1.Fileno = "Int";
            modelcache1.FilePath = "String";
            modelcache1.FileIsExists = "String";
            modelcache1.FileSize = "String";
            modelcache1.totalnooffiles = cachelist.Count;
            modelcache1.totalnoofsize = "0";
            Data2 = JsonConvert.SerializeObject(modelcache1);
//            Data3 = JsonConvert.SerializeObject(totalnooffiles);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
        public static class FileSizeFormatter
        {
            // Load all suffixes in an array  
            static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            public static string FormatSize(Int64 bytes)
            {
                int counter = 0;
                decimal number = (decimal)bytes;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1}{1}", number, suffixes[counter]);
            }
        }  
        //Json method for fetching values in databsae to all dropdownlist And passing list values to view through jsonresult.
        public JsonResult DepartmentEdit(string type, string actiontype)
        {
            Int64 empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            int? DocID = Convert.ToInt32(Session["fileid"].ToString());
            List<Dep_union_dropdown> dept = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Select = new List<Dep_union_dropdown>();
            try
            {
                dept = ServiceObj.GeALL(type, actiontype,empid);
                if (DocID != null && DocID != 0)
                {
                    Select = ServiceObj.GetselectedItem(DocID, type);
                }
                else
                {
                    Select = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { dept, Select }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepartmentEditNew(string type,string condition_1, string condition_2,string condition_3,string condition_4)
        {
            Int64 empid = Convert.ToInt64(Session["Emp_Id"].ToString());
          //  int? DocID = Convert.ToInt32(Session["fileid"].ToString());
            List<Dep_union_dropdown> dept = new List<Dep_union_dropdown>();
         //   List<Dep_union_dropdown> Select = new List<Dep_union_dropdown>();
            try
            {
                dept = ServiceObj.GetALLdropdown(type, condition_1, condition_2, condition_3,condition_4, empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { dept }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to unit,docgroup and docname dropdownlist using DepartmentId based.And passing list values to view through jsonresult. 
        public JsonResult GetDept(int DeptID)
        {
            List<Dep_union_dropdown> Get_unit = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname = new List<Dep_union_dropdown>();
            try
            {
                Get_unit = ServiceObj.GetUnit(DeptID);
                Get_docgroup = ServiceObj.GetDocGroup(DeptID);
                Get_docname = ServiceObj.GetDocName(DeptID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_unit, Get_docgroup, Get_docname }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,docgroup and docname dropdownlist using UnitId based.And passing list values to view through jsonresult. 
        public JsonResult GetUnit(int UnitID)
        {
            List<Dep_union_dropdown> Get_dept1 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup1 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname1 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept1 = ServiceObj.GetDept1(UnitID);
                Get_docgroup1 = ServiceObj.GetDocGroup1(UnitID);
                Get_docname1 = ServiceObj.GetDocName1(UnitID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept1, Get_docgroup1, Get_docname1 }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,Unit and docname dropdownlist using DocgroupId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocGroup(int DocGroupID)
        {
            Int64 empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            List<Dep_union_dropdown> Get_dept2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname2 = new List<Dep_union_dropdown>();
            try
            {
            //    Get_dept2 = ServiceObj.GetDept2(DocGroupID);
           //     Get_unit2 = ServiceObj.GetUnit2(DocGroupID);
                Get_docname2 = ServiceObj.GetDocName2(DocGroupID, empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_docname2 }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,Unit and docgroup dropdownlist using DocnameId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocName(int DocNameId)
        {
            Int64 empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            string userId = Session["Emp_Id"].ToString();
          //  List<Dep_union_dropdown> Get_dept3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_Orglevel = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup3 = new List<Dep_union_dropdown>();
            try
            {
    //            Get_dept3 = ServiceObj.GetDept3(DocNameId);
   //             Get_unit3 = ServiceObj.GetUnit3(DocNameId);
                Get_Orglevel = ServiceObj1.GetOrgLevel(DocNameId, userId);
                Get_docgroup3 = ServiceObj.GetDocGroup3(DocNameId, empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_Orglevel,Get_docgroup3 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Headerfetch(string orglvl4val, string orglvl3val, string orglvl2val, string orglvl1val) 
        {
            Headers_Model HeaderObj = new Headers_Model();
            List<Headers_Model> Orglvls = new List<Headers_Model>();
            HeaderObj.Orglvl1code = orglvl1val;
            HeaderObj.Orglvl2code = orglvl2val;
            HeaderObj.Orglvl3code = orglvl3val;
            HeaderObj.Orglvl4code = orglvl4val;

            try
            {
                Orglvls = ServiceObj1.Headerfetch_Srv(HeaderObj);
            }
            catch (Exception ex) {
                logger.Error(ex.ToString());
            }
            return Json(new { Orglvls }, JsonRequestBehavior.AllowGet);
        }


    }
}