using DMS.Model;
using DMS.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Web.Controllers
{
    public class AutoNumberingController : Controller
    {
        AutoNumbering_Service obj_srv = new AutoNumbering_Service();
        AutoNumbering_Model obj_model = new AutoNumbering_Model();
        // GET: AutoNumbering
        public ActionResult AutoNumbering()
        {
            return View();
        }

        public ActionResult GetAllAutonumbering() {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
           // List<AutoNumbering_Model> autonumeberlist = new List<AutoNumbering_Model>();
            obj_srv = new AutoNumbering_Service();
            ds = obj_srv.getAlldata();
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
            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt);
            Data2 = JsonConvert.SerializeObject(dt1);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoNumber_Save(string rowid, string prefix, string year, string month, string date, string sequence, string mode)
        {
            obj_model = new AutoNumbering_Model();
            obj_srv = new AutoNumbering_Service();
            obj_model.autonumber_rowid = rowid;
            obj_model.autonumber_prefix = prefix;
            obj_model.autonumber_year = year;
            obj_model.autonumber_month = month;
            obj_model.autonumber_date = date;
            obj_model.autonumber_sequence = sequence;
            obj_model.mode = mode;
            obj_model.createdby = (Session["Emp_Id"].ToString());
            return Json(obj_srv.AutonumberingIUD(obj_model));    
        }
    }
}