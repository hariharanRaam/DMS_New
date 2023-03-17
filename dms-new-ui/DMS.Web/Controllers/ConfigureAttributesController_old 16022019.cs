using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using DMS.Service;
using System.Data;
using DMS.Model;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class ConfigureAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigureAttributesController));  //Declaring Log4Net 
        ConfigureAttributes_Service ServiceObj = new ConfigureAttributes_Service();
        // GET: ConfigureAttributes
        public ActionResult ConfigureAttributes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(int DeptID, int UnitID, int DgroupID, int DNameID, string[] attributes1, string[] attributes2, string[] attributes3, string[] attributes4, string[] attributes5)
        {
            int Result = 0;
            try
            {
                string[] attrnameval = attributes1[0].ToString().Split(',');
                string[] attrtypeval = attributes2[0].ToString().Split(',');
                string[] attrlenval = attributes3[0].ToString().Split(',');
                string[] attrmandatoryval = attributes4[0].ToString().Split(',');
                string[] attrlovname = attributes5[0].ToString().Split(',');
                ConfigureAttributes_Service objSer = new ConfigureAttributes_Service();
                DataSet ds = new DataSet();
                for (int i = 0; i < attrnameval.Length; i++)
                {
                    string Len = attrlenval[i].ToString();
                    if (Len == "")
                    {
                        Len = "0";
                    }
                    ds = objSer.SaveConfigAttri(attrnameval[i].ToString(), Convert.ToInt16(Len), attrtypeval[i].ToString(), attrmandatoryval[i].ToString(), Convert.ToInt16(attrlovname[i].ToString()), DeptID, UnitID, DgroupID, DNameID);

                }
                Result =Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (Result == 1)
                {
                    ViewBag.Message = "File uploaded successfully";

                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            //return View();
            return Json(new { success = Result, JsonRequestBehavior.AllowGet });
        }

        //chechconfigattrib
        [HttpPost]
        public JsonResult checkconfigattrib(int DeptID, int UnitID, int DgroupID, int DNameID)
        {
            int Result = 0;
            try
            {

                DataSet ds = new DataSet();
                ds = ServiceObj.checkSaveConfigAttri(DeptID, UnitID, DgroupID, DNameID);
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to all dropdownlist And passing list values to view through jsonresult.
        public JsonResult DepartmentEdit(string type, string actiontype)
        {

            List<Dep_union_dropdown> dept = new List<Dep_union_dropdown>();
            try
            {
                dept = ServiceObj.GeALL(type, actiontype);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(dept, JsonRequestBehavior.AllowGet);
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

        //Json method for fetching values in databsae to department,Unit and docgroup dropdownlist using DocnameId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocName(int DocNameId)
        {
            List<Dep_union_dropdown> Get_dept3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup3 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept3 = ServiceObj.GetDept3(DocNameId);
                Get_unit3 = ServiceObj.GetUnit3(DocNameId);
                Get_docgroup3 = ServiceObj.GetDocGroup3(DocNameId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept3, Get_unit3, Get_docgroup3 }, JsonRequestBehavior.AllowGet);
        }
    }
}