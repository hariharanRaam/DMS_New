using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DMS.Service;
using System.Data;
using DMS.Web.Filters;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DepartmentMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 

        public List<DepartmentMaster_Model> Department = new List<DepartmentMaster_Model>();

        DepartmentMaster_Model Deptmodel = new DepartmentMaster_Model();
        DepartmentMaster_Service DepSerobj = new DepartmentMaster_Service();
        // GET: DepartmentMaster
        public ActionResult DepartmentMaster()
        {
            return View();
        }
        //read the data
        //public ActionResult DepartmentGrid_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    try
        //    {
        //        return Json(DepSerobj.DepatMstDtl().ToDataSourceResult(request));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //    }
        //    return View();
        //}
        //insert the data
        public ActionResult DepartmentGrid_Save([DataSourceRequest] DataSourceRequest request, DepartmentMaster_Model Deptmodel)
        {
            try
            {
              //  Deptmodel.Createdby = (Session["Emp_Id"].ToString());
                return Json(DepSerobj.DeptMstDtlSave(Deptmodel));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }
         
        //update the data
        public ActionResult DepartmentGrid_Update([DataSourceRequest] DataSourceRequest request, DepartmentMaster_Model Deptmodel)
        {
            try
            {
                Deptmodel.Createdby = (Session["Emp_Id"].ToString());
                return Json(DepSerobj.DeptMstDtlUpdate(Deptmodel));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }
        //delete the data
        //public ActionResult DeletingDepartment(int? DeptID)
        //{
        //    DataTable dt = new DataTable();
        //    string Result = "";
        //    try
        //    {
        //        dt = DepSerobj.DeletingDepartment(DeptID);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Result = dt.Rows[0][0].ToString();
        //        }
        //        return Json(Result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //        return View();
        //    }

        //}
        
    }
}