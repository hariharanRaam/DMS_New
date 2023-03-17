using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using DMS.Model;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;

//prakash code
namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class PhysicalArchivalController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GetAllDocumentsController));
        PhysicalArchival_Service serviceObj = new PhysicalArchival_Service();
        PhysicalArchival_Model modelObj = new PhysicalArchival_Model();
        BasicReport_Service serviceObj1 = new BasicReport_Service();

        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();

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


        // GET: PhysicalArchival
        public ActionResult PhysicalArchival()
        {
            return View();
        }

        public ActionResult PhysicalArchivalNew()
          {
            return View();
        }

        public ActionResult StorageListing() {
            return View();
        }

        public ActionResult GetIndexedDocuments(int? Dgroup1, int? Dname1, string Dbcon, string type, string activeflag, string from_date, string to_date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.GetIndexedDocuments(Dgroup1, Dname1, Dbcon, type, activeflag, from_date, to_date);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }


        public ActionResult GetIndexedRetrivals(int? Dgroup1, int? Dname1, string attrib_ids,string grid_mode)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = serviceObj.GetIndexedRetrivals(Dgroup1, Dname1, attrib_ids,grid_mode);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        public ActionResult ShowPopUpWindow(int? docgroup, int? docname, string Aaction, int? attribgid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = serviceObj.Initialvalues(docgroup, docname, Aaction, attribgid);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    Session["dt"] = dt;
                    foreach (DataRow dr in dt.Rows)
                    {
                        PhysicalArchival_Model depts = new PhysicalArchival_Model();
                        depts.Satrname = dr["Satr_Name"].ToString();
                        //depts.Satrid = dr["Satr_Id"].ToString();
                        depts.Satrtype = dr["Satr_Type"].ToString();
                        depts.Satrlen = dr["Satr_Length"].ToString();
                        depts.SatrMand = dr["Satr_Mandotry"].ToString();
                        if (Aaction == "Edit" || Aaction == "Delete")
                        {
                            depts.Satrdesc = dr["Sattribdtl_Description"].ToString();
                        }
                        else
                        {
                            depts.Satrdesc = null;
                        }
                        if (Aaction == "Edit") {
                            modelObj.activeflag = dr["Active"].ToString();
                        }
                        modelObj.dept.Add(depts);
                    }
                    //modelObj.SatrCount = Convert.ToInt32(ds.Tables[1].Rows[0]["count"].ToString());
                }
                //return PartialView("DynamicStorageAttributes", modelObj);
                return Json(modelObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("DynamicStorageAttributes", modelObj);
            }
        }

        [HttpPost]
        public ActionResult Save(string[] form, PhysicalArchival_Model modelObj, string[] form1)
        {
            int Result = 0;
            try
            {
                List<PhysicalArchival_Model> ModelObjList = new List<PhysicalArchival_Model>();
                int AtrCount = modelObj.SatrCount;
                modelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
               // modelObj.activeflag = activeflag;
                DataTable dt = Session["dt"] as DataTable;

                for (int j = 0; j < form1.Length; j++)
                {
                    for (int i = 0; i < form.Length; i++)
                    {
                        PhysicalArchival_Model ModelObject = new PhysicalArchival_Model();
                        ModelObject.Satrname = dt.Rows[i]["Satr_Name"].ToString();
                        ModelObject.Satrtype = dt.Rows[i]["Satr_Type"].ToString();
                        ModelObject.Satrlen = dt.Rows[i]["Satr_Length"].ToString();
                        ModelObject.SatrMand = dt.Rows[i]["Satr_Mandotry"].ToString();
                        //ModelObject.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                        ModelObject.Atrgid = Convert.ToInt32(form1[j].ToString());
                        ModelObject.DynamicVal = form[i];
                        ModelObjList.Add(ModelObject);
                    }
                }
                //passing model object and model list to service class
                Result = serviceObj.SaveStorageAttributes(modelObj, ModelObjList);
                if (Result == 1)
                {
                    if (modelObj.action == "Edit")
                    {
                        Result = 2;
                    }
                    if (modelObj.action == "Delete")
                    {
                        Result = 3;
                    }
                }
                return Json(new { status = Result });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(new { status = Result });
            }
        }

        public ActionResult checkvalidone(int? GridID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            try
            {
                dt = serviceObj.checkvalidone(GridID);
                Result = dt.Rows[0][0].ToString();
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }


        #region dropdown onchange methods,
        public JsonResult onChangeDropdowns(string Master = "", Int64 MasterID = 0)   //Method for fetching all dropdown values
        {
            List<BasicReport_Model> lst_dept = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_unit = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_docgroup = new List<BasicReport_Model>();
            List<BasicReport_Model> lst_docname = new List<BasicReport_Model>();
            DataSet ds = new DataSet();
            try
            {
                Int64 UserID = Convert.ToInt32(Session["Emp_Id"].ToString());
                ds = serviceObj1.onChangeDropdowns(Master, MasterID, UserID);
                if (ds.Tables.Count > 0)
                {
                    if (Master == "GetDocName")
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.CateID = Convert.ToInt64(row["Dgroup_Id"].ToString());
                            modelObj_.CateName = row["Dgroup_Name"].ToString();
                            lst_docgroup.Add(modelObj_);
                        }

                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.mastercode = row["master_code"].ToString();
                            modelObj_.mastername = row["master_name"].ToString();
                            lst_dept.Add(modelObj_);

                        }
                    }
                    if (Master == "GetDocGroup")
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.SubCateID = Convert.ToInt64(row["Dname_Id"].ToString());
                            modelObj_.SubCateName = row["Dname_Name"].ToString();
                            lst_docname.Add(modelObj_);
                        }
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.mastercode = row["master_code"].ToString();
                            modelObj_.mastername = row["master_name"].ToString();
                            lst_dept.Add(modelObj_);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { lst_dept, lst_docgroup, lst_docname }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public void flexcelfile_onchange(HttpPostedFileBase flexcelfile)//onchange method.
        {
            Session["Physicalfile"] = null;
            Session["Physicalfile"] = flexcelfile;

        }

        [HttpPost]
        public ActionResult Validation( int Docgroup, int Docname, string useracceptance) //Validating Documents file is already exist.
        {
            HttpPostedFileBase flexcelfile = Session["Physicalfile"] as HttpPostedFileBase; //Attribute excel files.
            string errormessage = "";
            OleDbConnection excelConnection = new OleDbConnection();
            try
            {
                DataSet ds = new DataSet(); //Creating dataset.
             //   modelObj.DeptId = Department;
              //  modelObj.UnitId = Unit;
                modelObj.CatgId = Docgroup;
                modelObj.SubCatgId = Docname;
                modelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                DataSet ads = new DataSet();
                ads = serviceObj.getattributes(modelObj);//Get dynamic Attributes&StorageAttributes for selected dropdown combination.
                DataTable dt = new DataTable();
                DataTable sdt = new DataTable();
                if (ads.Tables.Count > 1)
                {
                    dt = ads.Tables[0];//Get dynamic Attributes for selected dropdown combination.
                    int rowcount = dt.Rows.Count;
                    dt.Rows.Add(rowcount + 1, "File Name", 1000, "Alpha Numeric", "Y", 0);//adding new rows to datatable.
                    sdt = ads.Tables[1];//Get dynamic StorageAttributes for selected dropdown combination.
                }
                else
                {
                    errormessage = "Attributes Not Avilable..!";
                    //excelConnection.Close();
                    return Json(errormessage, JsonRequestBehavior.AllowGet);
                }

                if (flexcelfile.ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(flexcelfile.FileName);//get file selected file extension.
                    string filePath = ConfigurationManager.AppSettings["Path2"].ToString();
                    var InputFileName = Path.GetFileName(flexcelfile.FileName);
                    var ServerSavePath = Path.Combine(Path.Combine(filePath, Path.GetFileName(flexcelfile.FileName)));
                    string fileLocation = ServerSavePath;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    //Save file to server folder.  
                    flexcelfile.SaveAs(ServerSavePath);
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
                    excelConnection = new OleDbConnection(excelConnectionString);//connection for excel.
                    excelConnection.Close();//excel connection close.
                    excelConnection.Open();//excel connection open.
                    DataTable dtexcel = new DataTable();//datatable object.
                    dtexcel = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dtexcel == null)
                    {
                        return null;
                    }
                    String[] excelSheets = new String[dtexcel.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dtexcel.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);//connection for excel.
                    int excel = excelSheets.Count();
                    if (excel == 0)
                    {
                        errormessage = "Please select valid file..!";
                        excelConnection.Close();
                    }
                    else
                    {
                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                        DataTable dtexceldata = new DataTable();
                        if (ds.Tables.Count > 0)
                        {
                            dtexceldata = ds.Tables[0]; //Excel Data
                        }

                        if (dtexceldata.Rows.Count == 0)
                        {
                            errormessage = "Mandatory fields are empty in Excel file..!";
                            excelConnection.Close();
                            return Json(errormessage, JsonRequestBehavior.AllowGet);
                        }

                        string ismatched = "N";
                        string isfilematched = "N";

/*
                        //compare Attribute columns.
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string colmunname = dt.Rows[i]["Atr_Name"].ToString().ToUpper();
                            for (int j = 0; j < dtexceldata.Columns.Count; j++)
                            {
                                if (colmunname == dtexceldata.Columns[j].ToString().ToUpper())
                                {
                                    ismatched = "Y";
                                    break;
                                }
                                else
                                {
                                    ismatched = "N";
                                }
                            }
                            if (ismatched == "N")
                            {
                                errormessage = "Attributes name does not matched in excel.!";
                                excelConnection.Close();
                                return Json(errormessage, JsonRequestBehavior.AllowGet);
                            }
                        }
                        */

                        //compare Storage Attribute columns.
                        for (int i = 0; i < sdt.Rows.Count; i++)
                        {
                            string colmunname = sdt.Rows[i]["Satr_Name"].ToString().ToUpper();
                            for (int j = 0; j < dtexceldata.Columns.Count; j++)
                            {
                                if (colmunname == dtexceldata.Columns[j].ToString().ToUpper())
                                {
                                    ismatched = "Y";
                                    break;
                                }
                                else
                                {
                                    ismatched = "N";
                                }
                            }
                            if (ismatched == "N")
                            {
                                errormessage = "Storage Attributes name does not matched in excel.!";
                                excelConnection.Close();
                                return Json(errormessage, JsonRequestBehavior.AllowGet);
                            }
                        }



                        ////code for validate excel data's.
                        //string validationflag = "N";
                        //for (int p = 0; p < dt.Rows.Count; p++)
                        //{
                        //    string dttype = dt.Rows[p]["Atr_Type"].ToString();
                        //    string dtname = dt.Rows[p]["Atr_Name"].ToString().ToUpper();
                        //    int dtlenght = Convert.ToInt32(dt.Rows[p]["Atr_Length"].ToString());
                        //    string dtmandotry = (string)dt.Rows[p]["Atr_Mandotry"].ToString().ToUpper();
                        //    int dtlov = Convert.ToInt32(dt.Rows[p]["Lov_Id"].ToString());

                        //    for (int q = 0; q < dtexceldata.Columns.Count; q++)
                        //    {
                        //        string dtexcelname = dtexceldata.Columns[q].ToString().ToUpper();
                        //        if (dtname == dtexcelname)
                        //        {
                        //            for (int j = 0; j < dtexceldata.Rows.Count; j++)
                        //            {
                        //                string userdata = dtexceldata.Rows[j][dtexcelname].ToString();

                        //                if (dttype == "Alpha Numeric")//Validate given data is Alpha Numeric.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            int userdatalenght = userdata.Length;
                        //                            if (dtlenght >= userdatalenght)
                        //                            {
                        //                                validationflag = "Y";
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //                if (dttype == "Numeric")//Validate given data is Numeric.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            int num;
                        //                            bool isNumeric = int.TryParse(userdata, out num);
                        //                            if (isNumeric == true)
                        //                            {
                        //                                validationflag = "Y";
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //                if (dttype == "Amount")//Validate given data is Double.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                        //                            bool isFloat = regex.IsMatch(userdata);
                        //                            if (isFloat == true)
                        //                            {
                        //                                validationflag = "Y";
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //                if (dttype == "Datetime")//Validate given data is Datetime.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            DateTime date;
                        //                            if (DateTime.TryParse(userdata, out date))
                        //                            {
                        //                                validationflag = "Y";
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //                if (dttype == "Lov Name")//Validate given data is LOV.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            string LOV_Validation = serviceObj.validateLOV(dtlov, userdata);
                        //                            if (LOV_Validation == "valid")
                        //                            {
                        //                                validationflag = "Y";
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //                if (dttype == "Logical")//Validate given data is Logical.
                        //                {
                        //                    if (dtmandotry == "Y")
                        //                    {
                        //                        if (userdata != "" && userdata != null)
                        //                        {
                        //                            if (userdata.ToUpper() == "Y" || userdata.ToUpper() == "N")
                        //                            {
                        //                                validationflag = "Y";
                        //                                continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                validationflag = "N";
                        //                                break;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            validationflag = "N";
                        //                            break;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        validationflag = "Y";
                        //                        continue;
                        //                    }
                        //                }

                        //            }
                        //            if (validationflag == "N")
                        //            {
                        //                break;
                        //            }
                        //        }
                        //        //if (validationflag == "N")
                        //        //{
                        //        //    break;
                        //        //}
                        //    }
                        //    if (validationflag == "N")
                        //    {
                        //        break;
                        //    }
                        //} 
                        /* Attribute Validation End */

                        string validationflag = "N";
                        string itsattribute = "Y";
                        /*Storage Attribute Validation Start*/
                        for (int p = 0; p < sdt.Rows.Count; p++)
                        {
                            string dttype = sdt.Rows[p]["Satr_Type"].ToString();
                            string dtname = sdt.Rows[p]["Satr_Name"].ToString().ToUpper();
                            int dtlenght = Convert.ToInt32(sdt.Rows[p]["Satr_Length"].ToString());
                            string dtmandotry = (string)sdt.Rows[p]["Satr_Mandotry"].ToString().ToUpper();
                            //int dtlov = Convert.ToInt32(dt.Rows[p]["Lov_Id"].ToString());

                            for (int q = 0; q < dtexceldata.Columns.Count; q++)
                            {
                                string dtexcelname = dtexceldata.Columns[q].ToString().ToUpper();
                                //if (dtexcelname.ToUpper() == "FILE NAME")
                                //{
                                //    itsattribute = "N";
                                //}
                                if (dtexcelname.ToUpper() != "FILE NAME")
                                {
                                    if (dtname == dtexcelname)
                                    {
                                        for (int j = 0; j < dtexceldata.Rows.Count; j++)
                                        {
                                            string userdata = dtexceldata.Rows[j][dtexcelname].ToString();

                                            if (dttype == "Alpha Numeric")//Validate given data is Alpha Numeric.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        int userdatalenght = userdata.Length;
                                                        if (dtlenght >= userdatalenght)
                                                        {
                                                            validationflag = "Y";
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }

                                            if (dttype == "Numeric")//Validate given data is Numeric.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        int num;
                                                        bool isNumeric = int.TryParse(userdata, out num);
                                                        if (isNumeric == true)
                                                        {
                                                            validationflag = "Y";
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }

                                            if (dttype == "Amount")//Validate given data is Double.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                                                        bool isFloat = regex.IsMatch(userdata);
                                                        if (isFloat == true)
                                                        {
                                                            validationflag = "Y";
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }

                                            if (dttype == "Datetime")//Validate given data is Datetime.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        DateTime date;
                                                        if (DateTime.TryParse(userdata, out date))
                                                        {
                                                            validationflag = "Y";
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }

                                            /*if (dttype == "Lov Name")//Validate given data is LOV.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        string LOV_Validation = serviceObj.validateLOV(dtlov, userdata);
                                                        if (LOV_Validation == "valid")
                                                        {
                                                            validationflag = "Y";
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }*/

                                            if (dttype == "Logical")//Validate given data is Logical.
                                            {
                                                if (dtmandotry == "Y")
                                                {
                                                    if (userdata != "" && userdata != null)
                                                    {
                                                        if (userdata.ToUpper() == "Y" || userdata.ToUpper() == "N")
                                                        {
                                                            validationflag = "Y";
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "N";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    validationflag = "Y";
                                                    continue;
                                                }
                                            }

                                        }
                                        if (validationflag == "N")
                                        {
                                            break;
                                        }
                                    }
                                    //if (validationflag == "N")
                                    //{
                                    //    break;
                                    //}
                                }
                            }
                            if (validationflag == "N")
                            {
                                break;
                            }
                        }
                        /*Storage Attribute Validation End*/

                        if (validationflag == "N")
                        {
                            errormessage = "Data validation error occured..!";
                            excelConnection.Close();
                            return Json(errormessage, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            errormessage = "Success";
                            dt = SaveStorageAttribute(dtexceldata, modelObj);
                            if (dt.Rows.Count > 0)
                            {
                                errormessage = dt.Rows[0]["Message"].ToString();
                            }
                            excelConnection.Close();
                            return Json(errormessage, JsonRequestBehavior.AllowGet);
                        }
                    }




                }
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errormessage = "Invalid excel file..!";
                excelConnection.Close();
                return Json(errormessage, JsonRequestBehavior.AllowGet);
                logger.Error(ex.ToString());
            }

        }

        public DataTable SaveStorageAttribute(DataTable dt, PhysicalArchival_Model ModelObj)
        {
            /*for (int i = 0; i <= dt.Rows.Count; i++)
            {

            }*/
            DataTable Result = new DataTable();
            Result = serviceObj.SaveStorageAttribute(dt, ModelObj);

            return Result;

        }

        public JsonResult DocGroupNamesNew(string parentcode, string dependcode, int maxorg, int currentorg)
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
            return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
        }

    }
}