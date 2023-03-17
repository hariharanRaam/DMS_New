using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DMS.Web.Filters;
using DMS.Model;
using DMS.Service;
using System.Data;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class CreateMasterController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 
        //EmployeeMaster_Model EDtlObj = new EmployeeMaster_Model(); //Creating model object.
        //EmployeeMaster__Service EmpSrvObj = new EmployeeMaster__Service();//Creating service object.

        //public List<DepartmentMaster_Model> Department = new List<DepartmentMaster_Model>();
        //DepartmentMaster_Model Deptmodel = new DepartmentMaster_Model();
        //DepartmentMaster_Service DepSerobj = new DepartmentMaster_Service();

        public List<CreateMaster_Model> Department = new List<CreateMaster_Model>();
        CreateMaster_Model Deptmodel = new CreateMaster_Model();
        CreatetMaster_Service DepSerobj = new CreatetMaster_Service();

        public ActionResult CreateMaster()// GET: CreateMaster.
        {
            return View();
        }
        //Employee Master read.
        public ActionResult MasterGrid_Read([DataSourceRequest]DataSourceRequest request, CreateMaster_Model Deptmodel,string Master_TypeId) 
        {
            try
            {
                //string sMasterTypeId= (Session["MasterTypeId"].ToString());
               // Deptmodel.MasterTypeId = (Session["MasterTypeId"].ToString());
                //var result = DepSerobj.DepatMstDtl(Deptmodel).ToDataSourceResult(request);

                //var children = result
                //    .Select(c => new DepartmentMaster_Model(c.ChildParentID, c.ChildId, c.FirstName, c.LastNme))
                //    .ToList();
                Deptmodel.MasterTypeId = Master_TypeId;
                return Json(DepSerobj.DepatMstDtl(Deptmodel).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }
        public ActionResult MasterGrid_ReadOnchange([DataSourceRequest]DataSourceRequest request, CreateMaster_Model Deptmodel)
        {
            try
            {
                //string sMasterTypeId= (Session["MasterTypeId"].ToString());
                // Deptmodel.MasterTypeId = (Session["MasterTypeId"].ToString());
               
                return Json(DepSerobj.DepatMstDtl(Deptmodel).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }
        public JsonResult GetMasterType(string Master_TypeId)
        {
            List<CreateMaster_Model> Get_Dept = new List<CreateMaster_Model>();
            try
            {
                Get_Dept = DepSerobj.GetMasterType();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQcdMaster(string Master_TypeId)
        {
            List<CreateMaster_Model> Get_Dept = new List<CreateMaster_Model>();
            try
            {
                Get_Dept = DepSerobj.GetQcdMaster(Master_TypeId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        }
        //insert the data
        public ActionResult DepartmentGrid_Save([DataSourceRequest] DataSourceRequest request, CreateMaster_Model Deptmodel, string Master_TypeId, string dep_id="")
        {
            try
            {
                Deptmodel.DependId = dep_id;
                Deptmodel.MasterTypeId = Master_TypeId;
                Deptmodel.Createdby = (Session["Emp_Id"].ToString());
                return Json(DepSerobj.DeptMstDtlSave(Deptmodel));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        //update the data
        public ActionResult DepartmentGrid_Update(CreateMaster_Model Deptmodel, string dep_id="")
        {
            try
            {

                Deptmodel.DependId = dep_id;
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
        public ActionResult DeletingDepartment(string ID, string MasterTypeId)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = DepSerobj.DeletingDepartment(ID,MasterTypeId);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }

        }        //binding popup dropdown.
        //public JsonResult GetMasterType(string CommonVal)
        //{
        //    List<EmployeeMaster_Model> Get_Dept = new List<EmployeeMaster_Model>();
        //    try
        //    {
        //        Get_Dept = EmpSrvObj.GetDepartment(CommonVal);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //    }
        //    return Json(Get_Dept, JsonRequestBehavior.AllowGet);
        //}
    }
}