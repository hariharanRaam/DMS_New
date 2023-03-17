using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;
using DMS.Web.Filters;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class LinkController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LinkController));  //Declaring Log4Net 
        Link_Service ServiceObj = new Link_Service();
        ViewDocuments_Model deptsModel = new ViewDocuments_Model();
        // GET: Link
        public ActionResult Link()
        {
            return View();
        }

        //Method for fetching required grid1 datas into Database.
        public ActionResult GetGrid1Records(string[] form, int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {

            try
            {
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
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = ServiceObj.GetDocuments(DeptID1, Unit1, Dgroup1, Dname1, retval);

                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                string Data1;
                Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }


        public ActionResult LinkFiles(string[] Grid1, string[] Grid2)
        {
            string validation = "0";
            int Result = 0;
            try
            {
                string[] grid1val = Grid1[0].ToString().Split(',');
                string retval = string.Empty;
                //int i = 1;
                foreach (string item in grid1val)
                {
                    //if (i == grid1val.Length)
                    //{
                    //    if (item.ToString() != string.Empty)
                    //    {
                    //        retval = retval + item.ToString();
                    //    }
                    //}
                    //else
                    //{
                    if (item.ToString() != string.Empty)
                    {
                        retval = retval + item.ToString() + ",";
                    }
                    //}
                    //i++;
                }

                string[] grid2val = Grid2[0].ToString().Split(',');
                string retval1 = string.Empty;
                int j = 1;
                foreach (string item1 in grid2val)
                {
                    if (j == grid2val.Length)
                    {
                        if (item1.ToString() != string.Empty)
                        {
                            //if (j == 1)
                            //{
                            //    retval1 = retval1 + "," + item1.ToString();
                            //}
                            //else
                            //{
                            retval1 = retval1 + item1.ToString();
                            //}

                        }
                    }
                    else
                    {
                        if (item1.ToString() != string.Empty)
                        {
                            //retval1 = retval1 + "," + item1.ToString() + ",";
                            retval1 = retval1 + item1.ToString() + ",";
                        }
                    }
                    j++;
                }

                Result = ServiceObj.attachfiles(retval, retval1);

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult DeLinkFiles(string[] Grid1, string[] Grid2)
        {
            try
            {
                string[] grid1val = Grid1[0].ToString().Split(',');
                string retval = string.Empty;
                //int i = 1;
                foreach (string item in grid1val)
                {
                    //if (i == grid1val.Length)
                    //{
                    //    if (item.ToString() != string.Empty)
                    //    {
                    //        retval = retval + item.ToString();
                    //    }
                    //}
                    //else
                    //{
                    if (item.ToString() != string.Empty)
                    {
                        retval = retval + item.ToString() + ",";
                    }
                    //}
                    //i++;
                }

                string[] grid2val = Grid2[0].ToString().Split(',');
                string retval1 = string.Empty;
                int j = 1;
                foreach (string item1 in grid2val)
                {
                    if (j == grid2val.Length)
                    {
                        if (item1.ToString() != string.Empty)
                        {
                            //if (j == 1)
                            //{
                            //    retval1 = retval1 + "," + item1.ToString();
                            //}
                            //else
                            //{
                            retval1 = retval1 + item1.ToString();
                            //}

                        }
                    }
                    else
                    {
                        if (item1.ToString() != string.Empty)
                        {
                            //retval1 = retval1 + "," + item1.ToString() + ",";
                            retval1 = retval1 + item1.ToString() + ",";
                        }
                    }
                    j++;
                }

                //int Result = ServiceObj.attachfiles(attachid1, attachid2);
                int Result = ServiceObj.deattachfiles(retval, retval1);
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        //Post method for getting dynamic attributes.
        [HttpPost]
        public ActionResult GetDynamicFields(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ServiceObj.GetDynamicFields(DeptID1, Unit1, Dgroup1, Dname1);
                string Count = Convert.ToString(dt.Rows.Count);
                int Checkrcd = Convert.ToInt32(dt.Rows[0][0].ToString());
                ViewBag.AttributeCount = Convert.ToInt32(Count);
                int ctlval = 0;
                if (Checkrcd == 0)
                {
                    //show the alert message..
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ViewDocuments_Model depts = new ViewDocuments_Model();
                        depts.attrbname = dr["Atr_Name"].ToString();
                        depts.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
                        depts.attrbtype = dr["Atr_Type"].ToString();
                        deptsModel.dept.Add(depts);
                        ctlval++;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            //return View(deptsModel);

            return PartialView("DynamicAttributes", deptsModel);
        }


        //Json method for fetching values in databsae to all dropdownlist on pageload And passing list values to view through jsonresult.
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