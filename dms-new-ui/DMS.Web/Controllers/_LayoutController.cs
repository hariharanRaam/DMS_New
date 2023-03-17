using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DMS.Service;
using DMS.Model;

namespace DMS.Web.Controllers
{
    public class _LayoutController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(_LayoutController));  //Declaring Log4Net 
        EmpDetails_Model Objmodel = new EmpDetails_Model();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        // GET: Shared
        public ActionResult _Layout()
        {
            return View();
        }

        public ActionResult _LayoutNew() {
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

        public JsonResult setOrglvls(Array arr1, Array arr2, Array arr3, Array arr4)
        {
            LayoutModel modObj = new LayoutModel();
            if (modObj.orglvl1code == null || modObj.orglvl1code == "0" )
            {
                Session["orglvl1code"] = "";
                Session["orglvl1value"] = "";
            }
            else {
                Session["orglvl1code"] = modObj.orglvl1code;
                Session["orglvl1value"] = modObj.orglvl1value;
            }
            if (modObj.orglvl2code == null || modObj.orglvl2code == "0")
            {
                Session["orglvl2code"] = "";
                Session["orglvl2value"] = "";
            }
            else {
                Session["orglvl2code"] = modObj.orglvl2code;
                Session["orglvl2value"] = modObj.orglvl2value;
            }
            if (modObj.orglvl3code == null || modObj.orglvl3code == "0")
            {
                Session["orglvl3code"] = "";
                Session["orglvl3value"] = "";
            }
            else {
                Session["orglvl3code"] = modObj.orglvl3code;
                Session["orglvl3value"] = modObj.orglvl3value;
            }
            if (modObj.orglvl4code == null || modObj.orglvl4code == "0")
            {
                Session["orglvl4code"] = "";
                Session["orglvl4value"] = "";
            }
            else {
                Session["orglvl4code"] = modObj.orglvl4code;
                Session["orglvl4value"] = modObj.orglvl4value;
            }
            return Json("Success");
        }

        [HttpPost]
        public ActionResult EmpProfilePartialView()
        {
            // string emp_name = Session["Employee_Name"].ToString();
            int emp_id = Convert.ToInt32(Session["Emp_Id"].ToString());
            EmpProfileServ Objservice = new EmpProfileServ();
            try
            {

                DataTable dt = new DataTable();
                dt = Objservice.EmpProfileSer(emp_id);
                if (dt.Rows.Count > 0)
                {
                    Objmodel.EmpCode = dt.Rows[0]["Emp_Code"].ToString();
                    Objmodel.EmpName = dt.Rows[0]["Emp_Name"].ToString();
                    Objmodel.Grade = dt.Rows[0]["Grade_Name"].ToString();
                    Objmodel.EmpTitle = dt.Rows[0]["Title_Name"].ToString();
                    Objmodel.EmpDOJ = dt.Rows[0]["Emp_DOJ"].ToString();
                    Objmodel.City = dt.Rows[0]["City_Name"].ToString();
                    Objmodel.Pin = dt.Rows[0]["Pin_Code"].ToString();
                    Objmodel.State = dt.Rows[0]["State_Name"].ToString();
                    Objmodel.Region = dt.Rows[0]["Region_Name"].ToString();
                    Objmodel.EmpMobileNo = dt.Rows[0]["Emp_MobileNo"].ToString();
                    Objmodel.EmpEmailId = dt.Rows[0]["Emp_EmailId"].ToString();
                    Objmodel.EmpRole = dt.Rows[0]["usergroup_name"].ToString();
                    if (dt.Rows[0]["Active"].ToString().Equals("Y"))
                    {
                        Objmodel.EmpStatus = "Active";
                    }
                    else {
                        Objmodel.EmpStatus = "InActive";
                    }
                }
                else
                {
                    ViewBag.grd_lst = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Objmodel);
            //return PartialView("~/Views/Shared/EmpProfilePartialView.cshtml", Objmodel);
        }

 
    }
}