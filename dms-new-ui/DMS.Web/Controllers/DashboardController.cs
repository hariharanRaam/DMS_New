using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DMS.Service;
using DMS.Model;
using DMS.Web.Filters;
using System.Configuration;
using MySql.Data.MySqlClient;
//using DMS.Model;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DashboardController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GetAllDocumentsController));  //Declaring Log4Net 
        PiechartService psrvice = new PiechartService();
        MySqlConnection Con = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
        // GET: Dashboard
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
        public JsonResult DocGroupNamesNew(string parentcode, string dependcode,int maxorg,int currentorg)
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
            return Json(new { Data1,Data2,Data3,Data4}, JsonRequestBehavior.AllowGet);
        }
         public ActionResult Dashboard()
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            //DataTable dt2 = new DataTable();
            //DataTable dt3 = new DataTable();
            //DataTable dt4 = new DataTable();
            //DataTable dt5 = new DataTable();
            //DataTable dt6 = new DataTable();
            //ds1 = psrvice.GetDta();
            //dt2 = ds1.Tables[0];
            //dt3 = ds1.Tables[1];
            //dt4 = ds1.Tables[2];
            //dt5 = ds1.Tables[3];
            //dt6 = ds1.Tables[4];

            //ViewBag.Dept = ds1.Tables[0].Rows[0][0].ToString();
            //ViewBag.Unit = ds1.Tables[1].Rows[0][0].ToString();
            //ViewBag.Docname = ds1.Tables[2].Rows[0][0].ToString();
            //ViewBag.Docgroup = ds1.Tables[3].Rows[0][0].ToString();
            //ViewBag.docattributes = ds1.Tables[4].Rows[0][0].ToString();
             ds1 = psrvice.GetDta1();
            ds2 = psrvice.GetDt();
            //ViewData.Model = ds1;

            var _buildList = new List<PieChart>();
            {
                foreach (DataRow _row in ds1.Tables[0].Rows)
                {
                    _buildList.Add(new PieChart 
                    {
                        //chose what you want from the dataset results and assign it your view model fields 

                        master_code = _row["master_code"].ToString(),
                        master_name = _row["master_name"].ToString(),
                        archid = Convert.ToInt32(_row["archid"].ToString()),
                        
                    });



                }

            }
          //  ViewBag.Message = d
          //  ViewData["MyData"] = _buildList;
            ViewData.Model = _buildList;
            ViewBag.DD = ds2.Tables[0].Rows[0][0].ToString();
            ViewBag.DT = ds2.Tables[0].Rows[0][1].ToString();
            ViewBag.DG = ds2.Tables[0].Rows[0][2].ToString();
            ViewBag.ORG1 = ds2.Tables[0].Rows[0][3].ToString();
            ViewBag.ORG2 = ds2.Tables[0].Rows[0][4].ToString();
            ViewBag.ORG1N = ds2.Tables[0].Rows[0][5].ToString();
            ViewBag.ORG2N = ds2.Tables[0].Rows[0][6].ToString();
            return View(_buildList);
        }

    
        public ActionResult DashboardNew()
        {
            //DataSet ds1 = new DataSet();
            //DataTable dt2 = new DataTable();
            //DataTable dt3 = new DataTable();
            //DataTable dt4 = new DataTable();
            //DataTable dt5 = new DataTable();
            //DataTable dt6 = new DataTable();
            //ds1 = psrvice.GetDta();
            //dt2 = ds1.Tables[0];
            //dt3 = ds1.Tables[1];
            //dt4 = ds1.Tables[2];
            //dt5 = ds1.Tables[3];
            //dt6 = ds1.Tables[4];

            //ViewBag.Dept = ds1.Tables[0].Rows[0][0].ToString();
            //ViewBag.Unit = ds1.Tables[1].Rows[0][0].ToString();
            //ViewBag.Docname = ds1.Tables[2].Rows[0][0].ToString();
            //ViewBag.Docgroup = ds1.Tables[3].Rows[0][0].ToString();
            //ViewBag.docattributes = ds1.Tables[4].Rows[0][0].ToString();
            return View();
        }

        public ActionResult Piechart()
        {
            List<PieChart> piechrtmdl = new List<PieChart>();
            PieChart pcmdl = new PieChart();
            //var Result = 0;
            try
            {
                DataSet ds = new DataSet();
                //DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = psrvice.GetDta();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcmdl.Department = ds.Tables[0].Rows[0][0].ToString();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    pcmdl.Department = ds.Tables[1].Rows[0][0].ToString();
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    pcmdl.Department = ds.Tables[2].Rows[0][0].ToString();
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    pcmdl.Department = ds.Tables[3].Rows[0][0].ToString();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    pcmdl.Department = ds.Tables[4].Rows[0][0].ToString();
                }

                ViewBag.Department = ds.Tables[0].Rows[0][0].ToString();
                ViewBag.Unit = ds.Tables[1].Rows[0][0].ToString();
                ViewBag.Docname = ds.Tables[2].Rows[0][0].ToString();
                ViewBag.Docgroup = ds.Tables[3].Rows[0][0].ToString();
                ViewBag.Docattributes = ds.Tables[4].Rows[0][0].ToString();

                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        //PIE CHART
        [HttpGet]
        public ActionResult Piechartcount()
        {
            PiechartService pisrvice = new PiechartService();
            List<PieChart> piesrvicelst = new List<PieChart>();

            PieChart piesrv = new PieChart();
            PieChart get = new PieChart();
            try
            {
                DataTable dt1 = new DataTable();

                DataTable dt = new DataTable();
                //DataTable dt2 = new DataTable();
                //DataTable dt3= new DataTable();
                //DataTable dt4 = new DataTable();
                //DataTable dt5= new DataTable();
                //DataTable dt6 = new DataTable();
                DataSet ds = new DataSet();
                //  DataSet ds1 = new DataSet();
                ds = pisrvice.getdata();
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                /* ds1 = psrvice.GetDta();
                 DataTable dt2 = new DataTable();
                 dt2 = ds1.Tables[0];
                 DataTable dt3 = new DataTable();
                 dt3 = ds1.Tables[1];
                 DataTable dt4 = new DataTable();
                 dt4 = ds1.Tables[2];
                 DataTable dt5 = new DataTable();
                 dt5 = ds1.Tables[3];
                 DataTable dt6 = new DataTable();
                 dt6 = ds1.Tables[4];*/

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    piesrv = new PieChart();
                    piesrv.totalcount = Convert.ToInt32(dt.Rows[i][0].ToString());
                    piesrv.Dept_name = dt.Rows[i][1].ToString();
                    /*  piesrv.Dept = dt2.Rows[0][0].ToString();
                       piesrv.unit = dt3.Rows[0][0].ToString();
                       piesrv.docgroup = dt4.Rows[0][0].ToString();
                       piesrv.docname = dt5.Rows[0][0].ToString();
                       piesrv.docattributes = dt6.Rows[0][0].ToString();*/

                    piesrvicelst.Add(piesrv);
                }

                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                //return Json(piesrvicelst, JsonRequestBehavior.AllowGet);
                return Json(new { piesrvicelst, Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(piesrvicelst, JsonRequestBehavior.AllowGet);
                // return Json(new { piesrvicelst, Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}




