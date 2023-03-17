using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using DMS.Model;
using DMS.Service;
using DMS.Data;
namespace DMS.Web.Controllers
{
    public class OrgHierarchyController : Controller
    {
        //
        // GET: /OrgHierarchy/
        OrgHierarchy_Service OrgSerobj = new OrgHierarchy_Service();
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /OrgHierarchy/Details/5
        public ActionResult OrgHierarchy()
        {
            return View();
        }

        public JsonResult OrgSelect()
        {
            string Data1 = "";
            try
            {

                
                
                Data1 = JsonConvert.SerializeObject(OrgSerobj .Orgmstdetail ());
               

            }
            catch (Exception ex)
            {
               // logger.Error(ex.ToString());
            }

            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrgSave(List<OrgHierarchy_Model> objModel)
        {
            Int32 UserId= Convert.ToInt32 (Session["Emp_Id"].ToString());
            string sResult = OrgSerobj.SaveOrg(objModel, UserId);
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /OrgHierarchy/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OrgHierarchy/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OrgHierarchy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OrgHierarchy/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OrgHierarchy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OrgHierarchy/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
