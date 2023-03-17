
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


namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class LOVMasterController : Controller
    {
        LOVMaster_Model objmodel = new LOVMaster_Model();
        LOVMaster_Service LobjService = new LOVMaster_Service();

        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LOVMasterController));  //Declaring Log4Net 
        // GET: LOVMaster
        public ActionResult LOVMaster()
        {
            return View();

        }
        ////Bug_Id-5 Loading issue fixed.
        [HttpPost]
        public ActionResult LOVMaster(HttpPostedFileBase File, LOVMaster_Model objmodel)
        {
            try
            {
                LOVMaster_Model objmodel1 = new LOVMaster_Model();
                LOVMaster_Model objmodels = new LOVMaster_Model();
                DataSet ds = new DataSet();
                //to save the data in lovhdr
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

                        //Save file to server folder  
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
                                return View("LOVMaster");
                            }

                            if (dt1.Columns[0].ToString().Trim().ToUpper() == "ID" && dt1.Columns[1].ToString().Trim().ToUpper() == "NAME")
                            {
                                string InputData = objmodel.LovName;
                                int UserID = Convert.ToInt32(Session["Emp_Id"].ToString());

                                List<LOVMaster_Model> lstmodel = new List<LOVMaster_Model>();
                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {

                                    LOVMaster_Model mdlobj = new LOVMaster_Model();
                                    if (dt1.Rows[j][0].ToString() != string.Empty && dt1.Rows[j][1].ToString() != string.Empty)
                                    {
                                        mdlobj.excelId = Convert.ToInt32(dt1.Rows[j][0]);
                                        mdlobj.excelName = dt1.Rows[j][1].ToString();
                                        mdlobj.LovName = InputData;
                                        lstmodel.Add(mdlobj);

                                    }
                                    else
                                    {
                                        ViewBag.Message = "Excel data is Missing";
                                        return View("LOVMaster");
                                    }
                                }
                                int ret = LobjService.SaveLovMaster(InputData, UserID);
                                int result = LobjService.SaveLovdtl(lstmodel, UserID);
                                if (result == 1)
                                {
                                    ViewBag.Message = "File uploaded successfully";
                                }
                                else
                                {
                                    ViewBag.Message = "File Upload Failed";
                                    return View("LOVMaster");
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Invalid Header Name";
                                return View("LOVMaster");
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

                ViewBag.Message = "Invalid excel file";
                logger.Error(ex.ToString());

            }
            return View();
        }

        #region KendoGrid Bind
        public JsonResult Getfile(string Filename) //Bug_Id-2 fixed.
        {
            string Data1 = "", Data2 = "";
            try
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                ds = LobjService.GetFile(Filename);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
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

    }
}