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
using System.IO;
using Newtonsoft.Json;
using DMS.Web.Filters;


namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class InterFilingController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(InterFilingController));  //Declaring Log4Net 
        InterFiling_Model ModelObj = new InterFiling_Model();
        //Service Object
        InterFiling_Service ServiceObj = new InterFiling_Service();
        BasicReport_Service serviceObj1 = new BasicReport_Service();

        [HttpGet]
        public ActionResult InterFiling(string name)
        {
            return View();
        }


        [HttpGet]
        public ActionResult InterFilingNew(string name)
        {
            return View();
        }


        //Json method for fetching values in databsae to all dropdownlist And passing list values to view through jsonresult.
        #region  All Dropdownload in PageLoad
        public JsonResult DepartmentEdit(string type, string actiontype)
        {
            string userId = (Session["Emp_Id"].ToString());
            List<Dep_union_dropdown> dept = new List<Dep_union_dropdown>();
            try
            {
                dept = ServiceObj.GeALL(type, actiontype, userId);
            }
            catch (Exception ex)
            {
               logger.Error(ex.ToString());
            }
            return Json(dept, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Json method for fetching values in databsae to unit,docgroup and docname dropdownlist using DepartmentId based.And passing list values to view through jsonresult. 

        #region BasedonDropdown Load
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
            List<Dep_union_dropdown> Get_dept2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname2 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept2 = ServiceObj.GetDept2(DocGroupID);
                Get_unit2 = ServiceObj.GetUnit2(DocGroupID);
                Get_docname2 = ServiceObj.GetDocName2(DocGroupID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept2, Get_unit2, Get_docname2 }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDocName(int DocNameId)
        {
            List<Dep_union_dropdown> Get_dept3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_Orglevel = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup3 = new List<Dep_union_dropdown>();
            try
            {
               // Get_dept3 = ServiceObj.GetDept3(DocNameId);
                string userId = (Session["Emp_Id"].ToString());
                Get_Orglevel = ServiceObj.GetOrgLevel(DocNameId,userId);
                Get_docgroup3 = ServiceObj.GetDocGroup3(DocNameId,userId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept3, Get_Orglevel, Get_docgroup3 }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Dynamic Kendogrid Bind Interfiling 
        public ActionResult GetDocuments(string form, int? Dgroup1, int? Dname1, string activeflag)
        {
            try
            {
                /*
                string[] attrnameval = form[0].ToString().Split(',');
                string retval = string.Empty;
                int i = 1;
                retval = "0";
                foreach (string item in attrnameval)
                {
                    if (item == "''" && retval == "0")
                    {
                        retval = "0";
                    }
                    else
                    {
                        if (retval == "0")
                        {
                            retval = "";
                        }
                        if (i == attrnameval.Length)
                        {
                            if (item.ToString() != string.Empty)
                            {
                                retval = retval + item.ToString();
                            }
                        }
                        else
                        {
                            if (item.ToString() != string.Empty)
                            {
                                retval = retval + item.ToString() + ",";
                            }
                        }
                    }
                    i++;
                }

                if (retval == "")
                {
                    retval = "0";
                }
                */

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = ServiceObj.GetDocuments(Dgroup1, Dname1, form, activeflag);

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
              
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        #endregion
   
        #region SingleFileUploadforInterfiling
        [HttpPost]
        public ActionResult InterFiling()
        {
            try
            {
                string Result = "";
                HttpFileCollectionBase File = Request.Files;
                int Attachid = Convert.ToInt32(Request.Form["attacheid"].ToString());
                string remarks = Request.Form["remarks"].ToString();
                string activeflag = Request.Form["activeflag"].ToString();
                for (int i = 0; i < File.Count; i++)
                {


                    if (File != null && File[i].ContentLength > 0)
                    {
                        string filePath = ConfigurationManager.AppSettings["Path"].ToString();
                        string filepath = Path.Combine(filePath, Path.GetFileName(File[i].FileName));
                        string fileExtension = System.IO.Path.GetExtension(File[i].FileName);
                        ModelObj.FileExtension = fileExtension;
                        ModelObj.FilePath = filepath;
                        ModelObj.FileName = System.IO.Path.GetFileName(File[i].FileName); ;
                        ModelObj.FileType = fileExtension;
                        ModelObj.remarks = remarks;
                        ModelObj.activeflag = activeflag;
                        int result = ServiceObj.SaveSingleFile(ModelObj, File[i], Attachid);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
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
                ds = serviceObj1.onChangeDropdowns(Master, MasterID, UserID);
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
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { lst_dept, lst_docgroup, lst_docname }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}