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

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class ConfigureAttributesController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigureAttributesController));  //Declaring Log4Net 
        ConfigureAttributes_Service ServiceObj = new ConfigureAttributes_Service();//Creating service object
        public List<DocGroupMaster_Model> DocGroup = new List<DocGroupMaster_Model>();
        DocGroupMaster_Model Docgrpmdlobj = new DocGroupMaster_Model();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        // GET: ConfigureAttributes
        public ActionResult ConfigureAttributes()
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
        [HttpPost]
        public ActionResult Save(int DgroupID, int DNameID, string attributes1, string attributes2, string attributes3, string attributes4, string attributes5, string attributes6, string attributes7, string attributes8)
        {
            int Result = 0;
            try
            {
                string[] attrnameval = attributes1.ToString().Split(',');
                string[] attrtypeval = attributes2.ToString().Split(',');
                string[] attrlenval = attributes3.ToString().Split(',');
                string[] attrmandatoryval = attributes4.ToString().Split(',');
                string[] attrlovname = attributes5.ToString().Split(',');
                string[] Attrib_orderId = attributes6.ToString().Split(',');
                string[] attrautonumberid = attributes7.ToString().Split(',');
                string[] atr_keyval = attributes8.ToString().Split(',');
                ConfigureAttributes_Service objSer = new ConfigureAttributes_Service();//Creating service object
                DataSet ds = new DataSet();
                for (int i = 0; i < attrnameval.Length; i++)
                {
                    string Len = attrlenval[i].ToString();
                    if (Len == "")
                    {
                        Len = "0";
                    }
                    if (i == (attrnameval.Length - 1))
                    {
                        ds = objSer.SaveConfigAttri(attrnameval[i].ToString(), Convert.ToInt16(Len), attrtypeval[i].ToString(), attrmandatoryval[i].ToString(), Convert.ToInt16(attrlovname[i].ToString()), Convert.ToInt16(Attrib_orderId[i].ToString()), Convert.ToInt32(attrautonumberid[i].ToString()), atr_keyval[i].ToString(), DgroupID, DNameID, "END");
                    }
                    else {
                        ds = objSer.SaveConfigAttri(attrnameval[i].ToString(), Convert.ToInt16(Len), attrtypeval[i].ToString(), attrmandatoryval[i].ToString(), Convert.ToInt16(attrlovname[i].ToString()), Convert.ToInt16(Attrib_orderId[i].ToString()), Convert.ToInt32(attrautonumberid[i].ToString()), atr_keyval[i].ToString(), DgroupID, DNameID, "START");
                    }

                }
                Result = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

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
        public JsonResult checkconfigattrib( int DgroupID, int DNameID)
        {
            int Result = 0;
            try
            {

                DataSet ds = new DataSet();
                ds = ServiceObj.checkSaveConfigAttri( DgroupID, DNameID);
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
                string userId = (Session["Emp_Id"].ToString());
                dept = ServiceObj.GeALL(type, actiontype, userId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(dept, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to unit,docgroup and docname dropdownlist using DepartmentId based.And passing list values to view through jsonresult. 
        //public JsonResult GetDept(int DeptID)
        //{
        //    List<Dep_union_dropdown> Get_unit = new List<Dep_union_dropdown>();
        //    List<Dep_union_dropdown> Get_docgroup = new List<Dep_union_dropdown>();
        //    List<Dep_union_dropdown> Get_docname = new List<Dep_union_dropdown>();
        //    try
        //    {
        //        Get_unit = ServiceObj.GetUnit(DeptID);
        //        Get_docgroup = ServiceObj.GetDocGroup(DeptID);
        //        Get_docname = ServiceObj.GetDocName(DeptID);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //    }
        //    return Json(new { Get_unit, Get_docgroup, Get_docname }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetUnit(int UnitID)
        //{
        //    List<Dep_union_dropdown> Get_dept1 = new List<Dep_union_dropdown>();
        //    List<Dep_union_dropdown> Get_docgroup1 = new List<Dep_union_dropdown>();
        //    List<Dep_union_dropdown> Get_docname1 = new List<Dep_union_dropdown>();
        //    try
        //    {
        //        Get_dept1 = ServiceObj.GetDept1(UnitID);
        //        Get_docgroup1 = ServiceObj.GetDocGroup1(UnitID);
        //        Get_docname1 = ServiceObj.GetDocName1(UnitID);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //    }
        //    return Json(new { Get_dept1, Get_docgroup1, Get_docname1 }, JsonRequestBehavior.AllowGet);
        //}

        //Json method for fetching values in databsae to department,Unit and docname dropdownlist using DocgroupId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocGroup(int DocGroupID)
        {
            List<Dep_union_dropdown> Get_dept2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname2 = new List<Dep_union_dropdown>();
            try
            {
                //Get_dept2 = ServiceObj.GetDept2(DocGroupID);
                //Get_unit2 = ServiceObj.GetUnit2(DocGroupID);
                string userId = (Session["Emp_Id"].ToString());
                Get_unit2 = ServiceObj.GetOrgLevelbyDocgroup(DocGroupID, userId);
                Get_docname2 = ServiceObj.GetDocName2(DocGroupID, userId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept2, Get_unit2, Get_docname2 }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,Unit and docgroup dropdownlist using DocnameId based.And passing list values to view through jsonresult. 
        //Bug_ID-6 Master data refresh issue fixed.
        public JsonResult GetDocName(int DocNameId)
        {
            List<Dep_union_dropdown> Get_dept3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_Orglevel = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup3 = new List<Dep_union_dropdown>();
            try
            {
                //Get_dept3 = ServiceObj.GetDept3(DocNameId);
              
                string userId = (Session["Emp_Id"].ToString());
                Get_Orglevel = ServiceObj.GetOrgLevel(DocNameId, userId);
                Get_docgroup3 = ServiceObj.GetDocGroup3(DocNameId, userId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept3, Get_Orglevel, Get_docgroup3 }, JsonRequestBehavior.AllowGet);
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