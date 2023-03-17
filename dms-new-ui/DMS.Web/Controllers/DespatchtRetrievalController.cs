using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;
using DMS.Model;
using System.Reflection;

namespace DMS.Web.Controllers
{
    public class DespatchtRetrievalController : Controller
    {

        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(TreeSearchController));  //Declaring Log4Net 
        DespatchtRetrievalServices DRservice = new DespatchtRetrievalServices();
        SetDocAttributes_Model deptsModel1 = new SetDocAttributes_Model();
        SetDocAttributes_Model depts1;
        SetDocAttributes_Service ServiceObj1 = new SetDocAttributes_Service();
        ViewDocument_Services ServiceObj2 = new ViewDocument_Services();

        Search_Service ServiceObj = new Search_Service();
        ViewDocument_Services ObjService = new ViewDocument_Services();
        // GET: DespatchtRetrieval
        public ActionResult RetrivalChk()
        {
            return View();
        }

        public ActionResult DespatchRetrieval(int requestgid_) {
           ViewDocuments_Model.RequestRerival1 getRequestRetervial = new ViewDocuments_Model.RequestRerival1();
           DataTable dt0 = new DataTable();
           DataTable dt1 = new DataTable();
           DataTable dt2 = new DataTable();
           DataTable despatchdt = new DataTable();
           DataSet ds = new DataSet();
           Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
           try
           {
               ds = DRservice.GetChksummary(_empid,"Summary");
               if (ds.Tables.Count > 0)
               {
                   dt0 = ds.Tables[0];
                   dt1 = ds.Tables[1];
                   despatchdt = ds.Tables[2];
                   getRequestRetervial.Despatchdropdown = ConvertDataTable<DespatchDropDown>(despatchdt);
                   DataRow[] results = dt0.Select("Request_gid = '"+requestgid_+"'");
                   foreach (DataRow dr in results)
                   {
                       getRequestRetervial.NOofDoc = Convert.ToInt32(dr["Noof_Doc"].ToString());
                       getRequestRetervial.Request_From = dr["employee"].ToString();
                       getRequestRetervial.Request_No = dr["Request_No"].ToString();
                       getRequestRetervial.Request_Note = dr["Request_Note"].ToString();
                       getRequestRetervial.RequestDate = dr["Request_Date"].ToString();
                       getRequestRetervial.RequiredDate = dr["Date_Required"].ToString();
                       getRequestRetervial.Attrib_ids = dr["Attrids"].ToString();
                       getRequestRetervial.Request_gid = Convert.ToInt32(dr["Request_gid"].ToString());
                       dt2 = ObjService.getDocnameDocgroup(dr["Attrids"].ToString());
                       foreach (DataRow rs in dt2.Rows)
                       {
                           getRequestRetervial.Docname_Id = Convert.ToInt32(rs["Dname_ID"].ToString());
                           getRequestRetervial.Docgroup_Id = Convert.ToInt32(rs["Dgroup_ID"].ToString());
                           getRequestRetervial.Docname = rs["Dname"].ToString();
                           getRequestRetervial.Docgroup = rs["Dgroup_Name"].ToString();
                       }
                   }
               }
           }
           catch (Exception e) { 
           
           }
           return View(getRequestRetervial);
        }

        public ActionResult ViewDespatch(int requestgid_)
        {
            ViewDocuments_Model.RequestRerival1 getRequestRetervial = new ViewDocuments_Model.RequestRerival1();
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable despatchdt = new DataTable();
            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            try
            {
                ds = DRservice.GetChksummary(_empid,"Summary1");
                if (ds.Tables.Count > 0)
                {
                    dt0 = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    despatchdt = ds.Tables[2];
                    getRequestRetervial.Despatchdropdown = ConvertDataTable<DespatchDropDown>(despatchdt);
                    DataRow[] results = dt0.Select("Request_gid = '" + requestgid_ + "'");
                    foreach (DataRow dr in results)
                    {
                        getRequestRetervial.NOofDoc = Convert.ToInt32(dr["Noof_Doc"].ToString());
                        getRequestRetervial.Request_From = dr["employee"].ToString();
                        getRequestRetervial.Request_No = dr["Request_No"].ToString();
                        getRequestRetervial.Request_Note = dr["Request_Note"].ToString();
                        getRequestRetervial.RequestDate = dr["Request_Date"].ToString();
                        getRequestRetervial.RequiredDate = dr["Date_Required"].ToString();
                        getRequestRetervial.Attrib_ids = dr["Attrids"].ToString();
                        getRequestRetervial.Request_gid = Convert.ToInt32(dr["Request_gid"].ToString());
                        dt2 = ObjService.getDocnameDocgroup(dr["Attrids"].ToString());
                        foreach (DataRow rs in dt2.Rows)
                        {
                            getRequestRetervial.Docname_Id = Convert.ToInt32(rs["Dname_ID"].ToString());
                            getRequestRetervial.Docgroup_Id = Convert.ToInt32(rs["Dgroup_ID"].ToString());
                            getRequestRetervial.Docname = rs["Dname"].ToString();
                            getRequestRetervial.Docgroup = rs["Dgroup_Name"].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return View(getRequestRetervial);
        }

        #region Convert Datatable to List
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion

        public ActionResult RetrivalchkSummary()
        {
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64 (Session["Emp_Id"].ToString());
            try
            {
                ds = DRservice.GetChksummary(_empid,"Summary");
                if (ds.Tables.Count > 0)
                {
                    // var UniqueRows = ds.Tables[0].AsEnumerable().Distinct(DataRowComparer.Default);
                    //  dt = UniqueRows.CopyToDataTable();
                   
                    if (ds.Tables.Count > 1)
                    {
                        dt0 = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                    }
                    else
                    {
                        dt0 = ds.Tables[0];
                        dt2 = ds.Tables[2];
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            string Data1 = "", Data2 = "", Data3 = "";
            Data1 = JsonConvert.SerializeObject(dt0);
            Data2 = JsonConvert.SerializeObject(dt1);
            Data3 = JsonConvert.SerializeObject(dt2);
            return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RetrivalchkdetData(string Retrivid)
        {
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();
            
            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            try
            {
                ds = DRservice.GetChkdetail(Retrivid, _empid);
                if (ds.Tables.Count > 0)
                {
                    // var UniqueRows = ds.Tables[0].AsEnumerable().Distinct(DataRowComparer.Default);
                    //  dt = UniqueRows.CopyToDataTable();

                    if (ds.Tables.Count > 1)
                    {
                        dt0 = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        
                    }
                    else
                    {
                        dt0 = ds.Tables[0];
                        
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt0);
            Data2 = JsonConvert.SerializeObject(dt1);
            
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateRetrivalData(string Retrivid, string DespatchMode, string Despatchdate, string DespatchNote)
        {
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();

            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            try
            {
                dt0 = DRservice.updateRetrivalData(Retrivid,DespatchMode,Despatchdate,DespatchNote, _empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt0);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet); 
        }


        public ActionResult updatereturnRetrivalData(string Retrivid, string Despatchdate, string DespatchNote)
        {
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();

            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            try
            {
                dt0 = DRservice.updatereturnRetrivalData(Retrivid, Despatchdate, DespatchNote, _empid);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt0);
            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewIndexedDocDetails(int filegid)
        {
            int id = filegid;
            try
            {
                DataSet ds = new DataSet();
                ds = ServiceObj1.InitEditValues(id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    deptsModel1.FileName = ds.Tables[0].Rows[0][1].ToString();
                    deptsModel1.FileType = ds.Tables[0].Rows[0][2].ToString();
                    deptsModel1.HideDocArchId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    ViewBag.FilePath = ds.Tables[0].Rows[0][3].ToString();
                    deptsModel1.Filepathfrom = ds.Tables[0].Rows[0][3].ToString();

                    deptsModel1.VersionDate = Convert.ToDateTime(ds.Tables[0].Rows[0][4]);

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    deptsModel1.DeptName = ds.Tables[1].Rows[0][3].ToString();
                    deptsModel1.UnitName = ds.Tables[1].Rows[0][5].ToString();
                    deptsModel1.CateName = ds.Tables[1].Rows[0][7].ToString();
                    deptsModel1.SubCateName = ds.Tables[1].Rows[0][9].ToString();
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ViewBag.AttributeCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                }
                DataTable dt = new DataTable();
                dt = ds.Tables[3];
                //ViewBag.Attribute = dt;
                //Session["dt"] = dt;
                int ctlval = 0;

                string type;
                int LovId;
                DataTable dt1 = new DataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    depts1 = new SetDocAttributes_Model();
                    depts1.attrbname = dr["Atr_Name"].ToString();
                    depts1.attrid = dr["Atr_Id"].ToString();
                    type = dr["Atr_Type"].ToString();
                    if (type == "Lov Name")
                    {
                        LovId = Convert.ToInt32(dr["Lov_Id"].ToString());
                        dt1 = ServiceObj.getmasterval(LovId,type);

                        List<SelectListItem> item8 = new List<SelectListItem>();
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            item8.Add(new SelectListItem
                            {
                                Text = dr1["Lovexl_Name"].ToString(),
                                Value = dr1["Lovexl_Name"].ToString()
                            });
                        }
                        //ViewBag.Lovexl_Name = item8;
                        depts1.LovName = item8;
                    }
                    depts1.attrbdesc = dr["Attribdtl_Description"].ToString();
                    depts1.attrbtype = dr["Atr_Type"].ToString();
                    depts1.attrblen = dr["Atr_Length"].ToString();
                    depts1.attrbMand = dr["Atr_Mandotry"].ToString();
                    depts1.attrbmode = "D";
                    depts1.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
                    deptsModel1.dept.Add(depts1);
                    ctlval++;
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    ViewBag.AttributeCount = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                }
                DataTable dt6 = new DataTable();
                dt6 = ds.Tables[4];

                int ctlval1 = 0;

                string type1;
                // 22-03-2019
                //  DataTable dt9 = new DataTable();
                List<SelectListItem> item9 = new List<SelectListItem>();
                foreach (DataRow dr2 in dt6.Rows)
                {
                    depts1 = new SetDocAttributes_Model();
                    depts1.Satr_Name = dr2["Satr_Name"].ToString();
                    depts1.Satr_Length = Convert.ToInt32(dr2["Satr_Length"].ToString());
                    depts1.Satr_Type = dr2["Satr_Type"].ToString();
                    depts1.Satr_Mandotry = dr2["Satr_Mandotry"].ToString();
                    depts1.Sattribdtl_Description = dr2["Sattribdtl_Description"].ToString();
                    deptsModel1.dept.Add(depts1);
                    ctlval++;
                } 

                return PartialView("ShowIndexedDocDetailsPartialView", deptsModel1);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("ShowIndexedDocDetailsPartialView", deptsModel1);
            }
        }


        public ActionResult RetrievalReturn()
        {
            return View();
        }

        public ActionResult RetrivalreturnData()
        {
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();

            DataSet ds = new DataSet();
            Int64 _empid = Convert.ToInt64(Session["Emp_Id"].ToString());
            try
            {
                ds = DRservice.GetChkdetail(_empid);
                if (ds.Tables.Count > 0)
                {
                    // var UniqueRows = ds.Tables[0].AsEnumerable().Distinct(DataRowComparer.Default);
                    //  dt = UniqueRows.CopyToDataTable();

                    if (ds.Tables.Count > 1)
                    {
                        dt0 = ds.Tables[0];
                        dt1 = ds.Tables[1];

                    }
                    else
                    {
                        dt0 = ds.Tables[0];

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            string Data1 = "", Data2 = "";
            Data1 = JsonConvert.SerializeObject(dt0);
            Data2 = JsonConvert.SerializeObject(dt1);

            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }

    }
}