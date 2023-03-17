using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using DMS.Service;
using DMS.Model;
using System.IO;
using Newtonsoft.Json;
using DMS.Web.Filters;
using Kendo.Mvc.UI;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using DMS.Web.Filters;
//vadivu
namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class EmployeeController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocumentLinkingController));  //Declaring Log4Net 
        Employee_Model Empmdlobj = new Employee_Model();
        Employee_Service Empservobj = new Employee_Service();
        DataSet ds = new DataSet();
        // GET: Employee
        public ActionResult Employee()
        {
            Session.Remove("GridData");
            Session.Remove("GridDatatype");
            return View();
        }
        [HttpPost]
        public ActionResult Employee(HttpPostedFileBase File, Employee_Model objmodel)//excel bulk upload in employee master
        {
            List<Employee_Model> lstmodel = new List<Employee_Model>();
            Employee_Model mod = new Employee_Model();
            try
            {
                Session.Remove("GridData");
                Session.Remove("GridDatatype");
                DataTable dt3 = new DataTable();

                if (Request.Files["file"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string filePath = ConfigurationManager.AppSettings["Path1"].ToString();

                        var InputFileName = Path.GetFileName(File.FileName);
                        var ServerSavePath = Path.Combine(Path.Combine(filePath, Path.GetFileName(File.FileName)));
                        string fileLocation = ServerSavePath;
                        //Delete the file if already exist in same name.
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        //Save file to server folder.  
                        File.SaveAs(ServerSavePath);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=PathToExcelFileWithExtension;Extended Properties='Excel 8.0;HDR=YES'";

                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }

                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Close();
                        excelConnection.Open();

                        DataTable dt = new DataTable();
                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        int excel = excelSheets.Count();
                        if (excel == 0)
                        {
                            ViewBag.Message = "please select valid file";
                        }
                        else
                        {
                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }
                            DataTable dt1 = new DataTable();
                            if (ds.Tables.Count > 0)
                            {
                                dt1 = ds.Tables[0];
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                ViewBag.Message = "Mandatory fields are empty in Excel file";
                                return View("Employee");
                            }
                            if (dt1.Columns[3].ToString().Trim().ToUpper() == "TITLE ID" &&
                                dt1.Columns[6].ToString().Trim().ToUpper() == "CITY ID" &&
                                dt1.Columns[8].ToString().Trim().ToUpper() == "REGION ID" &&
                                dt1.Columns[10].ToString().Trim().ToUpper() == "PIN ID" &&
                                dt1.Columns[12].ToString().Trim().ToUpper() == "STATE ID" &&
                                dt1.Columns[18].ToString().Trim().ToUpper() == "DEPARTMENT ID" &&
                                dt1.Columns[20].ToString().Trim().ToUpper() == "UNIT ID" &&
                                dt1.Columns[22].ToString().Trim().ToUpper() == "GRADE ID" &&
                                dt1.Columns[24].ToString().Trim().ToUpper() == "EMPLOYEE TYPE ID" &&
                                dt1.Columns[27].ToString().Trim().ToUpper() == "USER GROUP ID")//to check header name valid or not
                            {
                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {
                                    Employee_Model mdlobj = new Employee_Model();
                                    mdlobj.UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                                    if (dt1.Rows[j][0].ToString() != string.Empty &&
                                        dt1.Rows[j][1].ToString() != string.Empty &&
                                        dt1.Rows[j][2].ToString() != string.Empty &&
                                        dt1.Rows[j][3].ToString() != string.Empty &&
                                        dt1.Rows[j][4].ToString() != string.Empty &&
                                        dt1.Rows[j][5].ToString() != string.Empty &&
                                        dt1.Rows[j][6].ToString() != string.Empty &&
                                        dt1.Rows[j][7].ToString() != string.Empty &&
                                        dt1.Rows[j][8].ToString() != string.Empty &&
                                        dt1.Rows[j][9].ToString() != string.Empty &&
                                        dt1.Rows[j][10].ToString() != string.Empty &&
                                        dt1.Rows[j][11].ToString() != string.Empty &&
                                        dt1.Rows[j][12].ToString() != string.Empty &&
                                        dt1.Rows[j][13].ToString() != string.Empty &&
                                        dt1.Rows[j][14].ToString() != string.Empty &&
                                        dt1.Rows[j][15].ToString() != string.Empty &&
                                        dt1.Rows[j][16].ToString() != string.Empty &&
                                        dt1.Rows[j][17].ToString() != string.Empty &&
                                        dt1.Rows[j][18].ToString() != string.Empty &&
                                        dt1.Rows[j][19].ToString() != string.Empty &&
                                        dt1.Rows[j][20].ToString() != string.Empty &&
                                        dt1.Rows[j][21].ToString() != string.Empty &&
                                        dt1.Rows[j][22].ToString() != string.Empty &&
                                        dt1.Rows[j][23].ToString() != string.Empty &&
                                        dt1.Rows[j][24].ToString() != string.Empty &&
                                        dt1.Rows[j][25].ToString() != string.Empty &&
                                        dt1.Rows[j][26].ToString() != string.Empty
                                        )//to check excel data missing or not
                                    {
                                        mdlobj.EmployeeCode = dt1.Rows[j][0].ToString();
                                        mdlobj.EmployeeName = dt1.Rows[j][1].ToString();
                                        mdlobj.Title = dt1.Rows[j][2].ToString();
                                        mdlobj.TitleID = Convert.ToInt32(dt1.Rows[j][3]);
                                        mdlobj.Address = dt1.Rows[j][4].ToString();
                                        mdlobj.City = dt1.Rows[j][5].ToString();
                                        mdlobj.CityID = Convert.ToInt32(dt1.Rows[j][6].ToString());
                                        mdlobj.Region = dt1.Rows[j][7].ToString();
                                        mdlobj.RegionID = Convert.ToInt32(dt1.Rows[j][8].ToString());
                                        mdlobj.Pin = dt1.Rows[j][9].ToString();
                                        mdlobj.PinID = Convert.ToInt32(dt1.Rows[j][10].ToString());
                                        mdlobj.State = dt1.Rows[j][11].ToString();
                                        mdlobj.StateID = Convert.ToInt32(dt1.Rows[j][12].ToString());
                                        mdlobj.EmailID = dt1.Rows[j][13].ToString();
                                        mdlobj.MobileNo = dt1.Rows[j][14].ToString();
                                        mdlobj.LanNo = dt1.Rows[j][15].ToString();
                                        mdlobj.DOJ = Convert.ToDateTime(dt1.Rows[j][16]);
                                        mdlobj.Dept_Name = dt1.Rows[j][17].ToString();
                                        mdlobj.Dept_Id = Convert.ToInt32(dt1.Rows[j][18].ToString());
                                        mdlobj.Unit = dt1.Rows[j][19].ToString();
                                        mdlobj.UnitID = Convert.ToInt32(dt1.Rows[j][20].ToString());
                                        mdlobj.Grade = dt1.Rows[j][21].ToString();
                                        mdlobj.GradeID = Convert.ToInt32(dt1.Rows[j][22].ToString());
                                        mdlobj.EmployeeType = dt1.Rows[j][23].ToString();
                                        mdlobj.emptypeId = Convert.ToInt32(dt1.Rows[j][24].ToString());
                                        mdlobj.Password = dt1.Rows[j][25].ToString();
                                        mdlobj.UserGroup = dt1.Rows[j][26].ToString();
                                        mdlobj.UserGroupID = Convert.ToInt32(dt1.Rows[j][27].ToString());

                                        lstmodel.Add(mdlobj);
                                    }
                                    else
                                    {
                                        ViewBag.Message = "Excel data is Missing";
                                        return View("Employee");
                                    }
                                }

                                DataSet ds1 = new DataSet();
                                DataTable dt2 = new DataTable();

                                ds = Empservobj.SaveEmpdtl(lstmodel, dt1);
                                if (ds.Tables.Count > 0)
                                {
                                    dt2 = ds.Tables[0];
                                    //dt1 = ds.Tables[1];
                                }
                                string result = ds.Tables[0].Rows[0][0].ToString();
                                if (result == "success")
                                {
                                    ViewBag.Message = "File uploaded successfully";
                                    Session["GridData"] = dt1;//ds.Tables[2];
                                    Session["GridDatatype"] = ds.Tables[1];
                                }
                                else
                                {
                                    Session["GridData"] = ds.Tables[2];
                                    Session["GridDatatype"] = ds.Tables[1];
                                    ViewBag.Message = "File Upload Failed";
                                    return View("Employee");
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Invalid Header Name";
                                return View("Employee");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please select valid Excel file";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        #region KendoGrid Bind
        public JsonResult Getfile()
        {
            string Data1 = "", Data2 = "";
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                dt = Session["GridData"] as DataTable;
                dt1 = Session["GridDatatype"] as DataTable;
                //ds = Empservobj.GetFile(Filename);
                //if (ds.Tables.Count > 0)
                //{
                //    dt = ds.Tables[0];
                //    dt1 = ds.Tables[1];
                //}
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //download empty excel headername   download
        [HttpGet]
        public FileResult DownloadTemplate(string a)
        {
            string fullPath = "";
            try
            {
                //get the temp folder and file path in server
                fullPath = System.Configuration.ConfigurationManager.AppSettings["LCF"];
                //return the file for download, this is an Excel 
                //so I set the file content type to "application/vnd.ms-excel"
                return File(fullPath, "application/vnd.ms-excel", "Employee.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return File(fullPath, "application/vnd.ms-excel", "Employee.xlsx");
            }
        }
        //empty excel sheet download
        [HttpGet]
        public ActionResult Exceldata(string data)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Empservobj.getexceldata(data);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Excel");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= Masters.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }

        }

    }
}
