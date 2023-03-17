using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web.Filters;
using DMS.Model;
using DMS.Service;
using System.Data;
using System.Configuration;
using ClosedXML.Excel;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class DocArchivalBulkUploadController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DocArchivalBulkUploadController));//Declaring Log4Net.
        DocArchivalBulkUpload_Service bulkserviceObj = new DocArchivalBulkUpload_Service();//Creating DocArchivalBulkUpload object.
        DocArchival_Model ModelObj = new DocArchival_Model(); //Creating DocArchival model object
        DocArchival_Service ServiceObj = new DocArchival_Service();  //Creating DocArchival Service Object
        public List<DocGroupMaster_Model> DocGroup = new List<DocGroupMaster_Model>();
        DocGroupMaster_Model Docgrpmdlobj = new DocGroupMaster_Model();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();

        DataTable Errdatatable = new DataTable();
        [HttpGet]
        public ActionResult DocArchivalBulkUpload()// GET: DocArchivalBulkUpload
        {
            Session["fileid"] = 0;
            Session["Singlefile"] = null;
            Session["Multiplefile"] = null;
            Session["Multiple"] = null;
            Session["Errdatatable"] = null;
            Session["dtexceldata"] = null;
            return View();
        }
        //Bug_Id-9 Validation issue is fixed.
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

        [HttpPost]
        public ActionResult Validation( int Docgroup, int Docname, string useracceptance ) //Validating Documents file is already exist.
        {
            HttpPostedFileBase flexcelfile = Session["Singlefile"] as HttpPostedFileBase; //Attribute excel files.
            HttpPostedFileBase[] flbulkfiles = Session["Multiplefile"] as HttpPostedFileBase[]; //user selected file.
           // List<HttpPostedFileBase> UploadedFiles = new List<HttpPostedFileBase>();
           // List<HttpPostedFileBase> flbulkfiles = Session["Multiple"] as List<HttpPostedFileBase>;
            int message;
            string errormessage = "";

            int sno = 0;
            int totalrecord = 0;
            int success_int = 0;
            int failcount = 0;
            string Result = "";
            DataTable duptable = new DataTable();

            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("File Name");
            Errdatatable.Columns.Add("Error Description");

            try
            {
                DataSet ds = new DataSet(); //Creating dataset.
                if (flexcelfile != null && flbulkfiles != null)
                {
                    int userid = Convert.ToInt32(Session["Emp_Id"].ToString());
                  //  ModelObj.DeptId = Department;
                   // ModelObj.UnitId = Unit;
                    ModelObj.CatgId = Docgroup;
                    ModelObj.SubCatgId = Docname;
                    ModelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                    DataTable dt = new DataTable(); //Creating datatable.
                    dt = bulkserviceObj.getattributes(ModelObj);//Get dynamic Attributes for selected dropdown combination.
                    int rowcount = dt.Rows.Count;
                    dt.Rows.Add(rowcount + 1, "File Name", 1000, "Alpha Numeric", "Y", 0);//adding new rows to datatable.
                    string _checkatrribute = dt.Rows[0][0].ToString();
                    if (_checkatrribute != "attributesnotfound")
                    {
                        if (flexcelfile.ContentLength > 0)
                        {
                            string fileExtension = System.IO.Path.GetExtension(flexcelfile.FileName);//get file selected file extension.
                            string filePath = ConfigurationManager.AppSettings["Path2"].ToString();
                            var InputFileName = Path.GetFileName(flexcelfile.FileName);
                            var ServerSavePath = Path.Combine(Path.Combine(filePath, Path.GetFileName(flexcelfile.FileName)));
                            string fileLocation = ServerSavePath;
                            //Delete the file if already exist in same name.
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
                            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);//connection for excel.
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
                                    //code for removing invalid empty rows from datatable.
                                    dtexceldata = dtexceldata.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field =>
                                    field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                                }
                                totalrecord = dtexceldata.Rows.Count;

                                if (useracceptance == "allow")
                                {
                                    message = savefiles(flbulkfiles, ModelObj, dtexceldata, flexcelfile, userid);//Save Documents
                                    if (message == 1)
                                    {
                                        errormessage = flbulkfiles.Count() + " " + "Files Uploaded and Indexed Scuccessfully..!!";
                                        excelConnection.Close();
                                        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        errormessage = "Uploading Failed..!";
                                        excelConnection.Close();
                                        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                if (dtexceldata.Rows.Count == 0)
                                {
                                    errormessage = "Mandatory fields are empty in Excel file..!";
                                    excelConnection.Close();
                                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                }

                                string _excelduplicates = "N";
                                DataTable dtfindduplicate = new DataTable();
                                dtfindduplicate.Rows.Clear();
                                //code for remove duplicate rows to datatable.
                                var UniqueRows = dtexceldata.AsEnumerable().Distinct(DataRowComparer.Default);
                                dtfindduplicate = UniqueRows.CopyToDataTable();
                                if (dtexceldata.Rows.Count.ToString() != dtfindduplicate.Rows.Count.ToString())
                                {
                                    _excelduplicates = "Y";
                                    errormessage = "Duplicate rows occured do you want continue.!";
                                    excelConnection.Close();
                                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    _excelduplicates = "N";

                                }

                                string ismatched = "N";
                                string isfilematched = "N";
                                string errs = "";

                                    //compare columns.
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        string colmunname = dt.Rows[i]["Atr_Name"].ToString().ToUpper();
                                        string columntype = dt.Rows[i]["Atr_Type"].ToString().ToUpper();
                                        var tempcol = "";
                                        for (int j = 0; j < dtexceldata.Columns.Count; j++)
                                        {
                                            if (colmunname == dtexceldata.Columns[j].ToString().ToUpper())
                                            {
                                                ismatched = "Y";
                                                break;
                                            }
                                            else if (columntype == "AUTONUMBER")
                                            {
                                                ismatched = "Y";
                                                break;
                                            }
                                            else
                                            {
                                                ismatched = "N";
                                                tempcol = dtexceldata.Columns[j].ToString().ToUpper();
                                                if (errormessage == "")
                                                {
                                                    errormessage = tempcol + " Column Name Cannot be Mached with Attribute Column";
                                                }
                                                else { 
                                                
                                                }
                                            }
                                        }
                                        if (ismatched == "N")
                                        {
                                              excelConnection.Close();
                                              return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                        }
                                    }



                                //compare selected files are matched to be mentioned in excel sheet.
                                for (int n = 0; n < dtexceldata.Rows.Count; n++)
                                {
                                    string excelfilename = dtexceldata.Rows[n]["File Name"].ToString().ToUpper();

                                    foreach (HttpPostedFileBase file in flbulkfiles)
                                    {
                                        if (file != null)
                                        {
                                            string physicalfilename = file.FileName;
                                            if (excelfilename == physicalfilename.ToUpper())
                                            {
                                                isfilematched = "Y";
                                                break;
                                            }
                                            else
                                            {
                                                isfilematched = "N";
                                            }
                                        }
                                    }
                                    if (isfilematched == "N")
                                    {
                                        //if (errs == "")
                                        //{
                                        //    errs = "Choosen files does not matched mentioned in excel ";
                                        //}
                                        //else
                                        //{
                                        //    errs = errs + "," + "Choosen files does not matched mentioned in excel ";
                                        //}

                                       errormessage = "Choosen files does not matched mentioned in excel.!";
                                        excelConnection.Close();
                                        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                    }
                                }


                                //for (int y = 0; y < dtexceldata.Rows.Count; y++)
                                //{

                                //    for (int p = 0; p < dt.Rows.Count; p++)
                                //    {
                                //        string dttype = dt.Rows[p]["Atr_Type"].ToString();
                                //        string dtname = dt.Rows[p]["Atr_Name"].ToString().ToUpper();
                                //        int dtlenght = Convert.ToInt32(dt.Rows[p]["Atr_Length"].ToString());
                                //        string dtmandotry = (string)dt.Rows[p]["Atr_Mandotry"].ToString().ToUpper();
                                //        int dtlov = Convert.ToInt32(dt.Rows[p]["Lov_Id"].ToString());
                                //        string dtexcelname = dtexceldata.Columns[p].ToString().ToUpper();
                                //        if (dtname == dtexcelname)
                                //        {
                                //        }
                                //    }
                                //}

                                //code for validate excel data's.
                                
                                string validationflag = "";
                                for (int p = 0; p < dt.Rows.Count; p++)
                                {
                                    errormessage = "";
                                    string dttype = dt.Rows[p]["Atr_Type"].ToString();
                                    string dtname = dt.Rows[p]["Atr_Name"].ToString().ToUpper();
                                    int dtlenght = Convert.ToInt32(dt.Rows[p]["Atr_Length"].ToString());
                                    string dtmandotry = (string)dt.Rows[p]["Atr_Mandotry"].ToString().ToUpper();
                                    int dtlov = Convert.ToInt32(dt.Rows[p]["Lov_Id"].ToString());

                                    for (int q = 0; q < dtexceldata.Columns.Count; q++)
                                    {
                                        string dtexcelname = dtexceldata.Columns[q].ToString().ToUpper();
                                        if (dtname == dtexcelname)
                                        {
                                            for (int j = 0; j < dtexceldata.Rows.Count; j++)
                                            {
                                                errormessage = "";
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
                                                                 if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Attribute Length Cannot be Greater than "+dtlenght;
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Attribute Length Cannot be Greater than "+dtlenght;
                                                                }
                                                                validationflag = "N";
                                                              //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                             if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                          //  break;
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
                                                                if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Numeric Data is Required Only";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Numeric Data is Required Only";
                                                                }
                                                                validationflag = "N";
                                                               // break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                            validationflag = "N";
                                                           // break;
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
                                                                if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Amount Data is Required Only";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Amount Data is Required Only";
                                                                }
                                                                validationflag = "N";
                                                             //   break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                              if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                            validationflag = "N";
                                                          //  break;
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
                                                                 if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Datetime Data is Required Only";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Datetime Data is Required Only";
                                                                }
                                                                validationflag = "N";
                                                              //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            validationflag = "N";
                                                          //  break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                          if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                        validationflag = "Y";
                                                        continue;
                                                    }
                                                }

                                                if (dttype == "Lov Name")//Validate given data is LOV.
                                                {
                                                    if (dtmandotry == "Y")
                                                    {
                                                        if (userdata != "" && userdata != null)
                                                        {
                                                            string LOV_Validation = bulkserviceObj.validateLOV(dtlov, userdata);
                                                            if (LOV_Validation == "valid")
                                                            {
                                                                validationflag = "Y";
                                                            }
                                                            else
                                                            {
                                                                 if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Lov Name Data is Not Matched With Lov Master";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Lov Name Data is Not Matched With Lov Master";
                                                                }
                                                                validationflag = "N";
                                                              //  break;
                                                            }
                                                        }
                                                        else 
                                                        {
                                                             if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                            validationflag = "N";
                                                          //  break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "Y";
                                                        continue;
                                                    }
                                                }

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
                                                                 if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Logical Data is Required only";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Logical Data is Required only";
                                                                }
                                                                validationflag = "N";
                                                             //   break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                             if(errormessage == ""){
                                                                     errormessage =  dtexcelname + "Cannot be Blank ";
                                                                }else{
                                                                 errormessage +=  dtexcelname + "Cannot be Blank ";
                                                                }
                                                            validationflag = "N";
                                                          //  break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        validationflag = "Y";
                                                        continue;
                                                    }
                                                }

                                                if (validationflag == "N")
                                                {

                                                    string isavl = "N";
                                                    for (int m = 0; m < Errdatatable.Rows.Count; m++)
                                                    {
                                                        string temp1 = Errdatatable.Rows[m][1].ToString();
                                                        string temp2 = dtexceldata.Rows[j][dtexceldata.Columns.Count - 1].ToString();
                                                        if (temp1 == temp2)
                                                        {
                                                            Errdatatable.Rows[m][2] = Errdatatable.Rows[m][2] + ", " + errormessage;
                                                            isavl = "Y";
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            isavl = "N";
                                                        }
                                                    }
                                                    if (isavl == "N")
                                                    {
                                                        sno++;
                                                        Errdatatable.Rows.Add(sno, dtexceldata.Rows[j][dtexceldata.Columns.Count - 1].ToString(), errormessage);
                                                    }


                                                 //   sno++;
                                                 //   Errdatatable.Rows.Add(sno, dtexceldata.Rows[j][dtexceldata.Columns.Count -1].ToString(), errormessage);
                                                   // break;
                                                }
                                            }
                                            if (validationflag == "N")
                                            {
                                              //  break;
                                            }
                                        }

                                        if (validationflag == "N")
                                        {

                                            //sno++;
                                            //Errdatatable.Rows.Add(sno, dtexcelname, "Data validation error occured "); 
                                            //break;
                                        }
                                    }
                                    if (validationflag == "N")
                                    {
                                     //   Errdatatable.Add(sno + 1, "", "Data validation error occured ");
                                     //   break;
                                    }

                                }
                                if (validationflag == "N")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Data validation error occured ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Data validation error occured ";
                                    }

                                    errormessage = "Data validation error occured..!";
                                 //   excelConnection.Close();
                                 //   return Json(errormessage, JsonRequestBehavior.AllowGet);
                                }
                                string _wherecondition = "";
                                
                                for (int i = 0; i < dtexceldata.Rows.Count; i++)
                                {
                                    errormessage = "";
                                    _wherecondition = "";
                                    for (int j = 0; j < dtexceldata.Columns.Count - 1; j++)
                                    {
                                        if (dtexceldata.Columns.Count - 2 == j)
                                        {
                                            _wherecondition += "`" + dtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + dtexceldata.Rows[i][j].ToString().Trim() + "'";
                                        }
                                        else
                                        {
                                            _wherecondition += "`" + dtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + dtexceldata.Rows[i][j].ToString().Trim() + "'" + " " + "AND" + " ";
                                        }

                                    }
                                    string attributevalidationflag = "";
                                    try
                                    {
                                        attributevalidationflag = bulkserviceObj.validateattribute(ModelObj, _wherecondition);//Check attributes is already exist.
                                        _wherecondition = "";
                                    }
                                    catch (Exception e) {
                                        attributevalidationflag = e.ToString();
                                        _wherecondition = "";
                                    }
                                    if (attributevalidationflag == "duplicate not found")
                                    {
                                        //message = savefiles(flbulkfiles, ModelObj, dtexceldata, flexcelfile, userid);
                                        //if (message == 1)
                                        //{
                                        //    success_int++;
                                        //    errormessage = flbulkfiles.Count() + " " + "Files Uploaded and Indexed Scuccessfully..!!";
                                        //    //excelConnection.Close();
                                        //   // return Json(errormessage, JsonRequestBehavior.AllowGet);
                                        //}
                                        //else
                                        //{
                                        //    failcount++;
                                        //    errormessage = "Uploading Failed..!";
                                        //   // excelConnection.Close();
                                        //   // return Json(errormessage, JsonRequestBehavior.AllowGet);
                                        //}
                                        int result = 0;
                                        string errormsg = "";
                                        foreach (HttpPostedFileBase file in flbulkfiles)
                                        {
                                            if (file != null)
                                            {

                                                if (dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString() == file.FileName)
                                                {
                                                    result = bulkserviceObj.SaveSingleFile(ModelObj, file);
                                                }
                                                if (result > 0)
                                                {
                                                    message = getindexedinialatributes(result, dtexceldata, file.FileName);

                                                    if (message > 0)
                                                    {
                                                        errormsg = "";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        errormsg = "File Saved but File Indexing Failed while Store Document Attributes on DataBase!";
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    errormsg = "File Archival Failed on While Store File Attributes on DataBase!";
                                                    break;
                                                }

                                            }
                                        }
                                        if (errormsg == "")
                                        {
                                            success_int++;
                                            errormessage = flbulkfiles.Count() + " " + "Files Uploaded and Indexed Scuccessfully..!!";
                                            //excelConnection.Close();
                                            // return Json(errormessage, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            failcount++;
                                            if (errormessage == "")
                                            {
                                                errormessage = errormsg;
                                            }
                                            else
                                            {
                                                errormessage = errormessage + errormsg;
                                            }

                                            try
                                            {
                                                string isavl = "N";
                                                for (int m = 0; m < Errdatatable.Rows.Count; m++)
                                                {
                                                    string temp1 = Errdatatable.Rows[m][1].ToString();
                                                    string temp2 = dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString();
                                                    if (temp1 == temp2)
                                                    {
                                                        Errdatatable.Rows[m][2] = Errdatatable.Rows[m][2] + ", " + errormessage;
                                                        isavl = "Y";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        isavl = "N";
                                                    }
                                                }
                                                if (isavl == "N")
                                                {
                                                    sno++;
                                                    Errdatatable.Rows.Add(sno, dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString(), errormessage);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                var iu = e.ToString();
                                            }
                                            // excelConnection.Close();
                                            // return Json(errormessage, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else {
                                        if (errormessage == "")
                                        {
                                            errormessage = "The Attributes Values Combination are Already available for Another file";
                                        }
                                        else {
                                            errormessage = errormessage + ",The Attributes Values Combination are Already available for Another file";
                                        }
                                        failcount++;
                                        try
                                        {
                                            string isavl = "N";
                                            for (int m = 0; m < Errdatatable.Rows.Count; m++) {
                                                string temp1 = Errdatatable.Rows[m][1].ToString();
                                                string temp2 = dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString();
                                                if (temp1 == temp2)
                                                {
                                                    Errdatatable.Rows[m][2] = Errdatatable.Rows[m][2] + ", " + errormessage;
                                                    isavl = "Y";
                                                    break;
                                                }
                                                else {
                                                    isavl = "N";
                                                }                                            
                                            }
                                            if (isavl == "N") {
                                                sno++;
                                                Errdatatable.Rows.Add(sno, dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString(), errormessage);
                                            }

                                            //sno++;
                                           // Errdatatable.Rows[i][dtexceldata.Columns.Count - 1] = Errdatatable.Rows[i + 1][dtexceldata.Columns.Count - 1] + ", DuplicateAttributesFountInDB";
                                           // Errdatatable.Rows.Add(sno, dtexceldata.Rows[i][dtexceldata.Columns.Count - 1].ToString(), "Data validation error occured ");
                                        }
                                        catch (Exception e) {
                                            var iu = e.ToString();
                                        }
                                    }

                                }
                              

                            }
                            excelConnection.Close();   

                        }
                    }
                    else
                    {
                        //wriiting code for without attributes.
                    }

                }
                else
                {
                    errormessage = "You have not specified a file..!";
                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                }
                if (success_int > 0 && failcount == 0)
                {
                errormessage = success_int + " files upload successfully ";

                }else{
                    if (success_int == 1) { 
                    errormessage = success_int + " file upload successfully, " + failcount +" files Upload failed." + "\n Do you Want To Download Error Report Excel?";
                    }else{
                        errormessage = success_int + " files upload successfully " + failcount + " files Upload failed." + "\n Do you Want To Download Error Report Excel?";
                    }
                }
                Session["Errdatatable"] = Errdatatable;
                return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errormessage = "Invalid excel file..!";

                return Json(new { failcount,errormessage}, JsonRequestBehavior.AllowGet);
                logger.Error(ex.ToString());
            }
        }
        public int UploadFilesCount(IEnumerable<HttpPostedFileBase> files)
        {
            int numberOfFiles = 0;

            if (files != null)
            {
                numberOfFiles = files.Count();
            }
           // files.Contains("");
            return numberOfFiles;
        }
        [HttpPost]
        public JsonResult ValidationN(int Docgroup, int Docname, string useracceptance,string mode) //Validating Documents file is already exist.
        {
            HttpPostedFileBase flexcelfile = Session["Singlefile"] as HttpPostedFileBase; //Attribute excel files.
            HttpPostedFileBase[] flbulkfiles = Session["Multiplefile"] as HttpPostedFileBase[]; //user selected file.
            int message;
            string errormessage = "";

            int sno = 0;
            int totalrecord = 0;
            int success_int = 0;
            int failcount = 0;
            string Result = "";
            string Data1 = "", Data2 = "", Data3 = "";
            DataTable duptable = new DataTable();

            //Errdatatable.Columns.Add("SNo");
            //Errdatatable.Columns.Add("File Name");
            //Errdatatable.Columns.Add("Error Description");

            try
            {
                DataSet ds = new DataSet(); //Creating dataset.
                if (flexcelfile != null)
                {
                    int userid = Convert.ToInt32(Session["Emp_Id"].ToString());
                    //  ModelObj.DeptId = Department;
                    // ModelObj.UnitId = Unit;
                    ModelObj.CatgId = Docgroup;
                    ModelObj.SubCatgId = Docname;
                    ModelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                    DataTable dt = new DataTable(); //Creating datatable.
                    dt = bulkserviceObj.getattributes(ModelObj);//Get dynamic Attributes for selected dropdown combination.
                    int rowcount = dt.Rows.Count;
                    dt.Rows.Add(rowcount + 1, "File Name", 1000, "Alpha Numeric", "Y", 0);//adding new rows to datatable.
                    string _checkatrribute = dt.Rows[0][0].ToString();
                    if (_checkatrribute != "attributesnotfound")
                    {
                        if (flexcelfile.ContentLength > 0)
                        {
                            string fileExtension = System.IO.Path.GetExtension(flexcelfile.FileName);//get file selected file extension.
                            string filePath = ConfigurationManager.AppSettings["Path2"].ToString();
                            var InputFileName = Path.GetFileName(flexcelfile.FileName);
                            var ServerSavePath = Path.Combine(Path.Combine(filePath, Path.GetFileName(flexcelfile.FileName)));
                            string fileLocation = ServerSavePath;
                            //Delete the file if already exist in same name.
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
                            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);//connection for excel.
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
                                    //code for removing invalid empty rows from datatable.
                                    dtexceldata = dtexceldata.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field =>
                                    field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                                }
                                totalrecord = dtexceldata.Rows.Count;

                                //if (useracceptance == "allow")
                                //{
                                //    message = savefiles(flbulkfiles, ModelObj, dtexceldata, flexcelfile, userid);//Save Documents
                                //    if (message == 1)
                                //    {
                                //        errormessage = flbulkfiles.Count() + " " + "Files Uploaded and Indexed Scuccessfully..!!";
                                //        excelConnection.Close();
                                //        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                //    }
                                //    else
                                //    {
                                //        errormessage = "Uploading Failed..!";
                                //        excelConnection.Close();
                                //        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                //    }
                                //}

                                if (dtexceldata.Rows.Count == 0)
                                {
                                    errormessage = "Mandatory fields are empty in Excel file..!";
                                    excelConnection.Close();
                                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                }

                                string _excelduplicates = "N";
                                DataTable dtfindduplicate = new DataTable();
                                dtfindduplicate.Rows.Clear();
                                //code for remove duplicate rows to datatable.
                                var UniqueRows = dtexceldata.AsEnumerable().Distinct(DataRowComparer.Default);
                                dtfindduplicate = UniqueRows.CopyToDataTable();
                                if (dtexceldata.Rows.Count.ToString() != dtfindduplicate.Rows.Count.ToString())
                                {
                                    _excelduplicates = "Y";
                                    errormessage = "Duplicate rows occured do you want continue.!";
                                    excelConnection.Close();
                                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    _excelduplicates = "N";

                                }

                                string ismatched = "N";
                                string isfilematched = "N";
                                string errs = "";

                                Errdatatable = dtexceldata.Copy();
                                Session["dtexceldata"] = dtexceldata.Copy();

                                //compare columns.
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string colmunname = dt.Rows[i]["Atr_Name"].ToString().ToUpper();
                                    string columntype = dt.Rows[i]["Atr_Type"].ToString().ToUpper();
                                    var tempcol = "";
                                    for (int j = 0; j < dtexceldata.Columns.Count; j++)
                                    {
                                        if (colmunname == dtexceldata.Columns[j].ToString().ToUpper())
                                        {
                                            ismatched = "Y";
                                            break;
                                        }
                                        else if (columntype == "AUTONUMBER")
                                        {
                                            ismatched = "Y";
                                            break;
                                        }
                                        else
                                        {
                                            ismatched = "N";
                                            tempcol = dtexceldata.Columns[j].ToString().ToUpper();
                                            if (errormessage == "")
                                            {
                                                errormessage = tempcol + " Column Name Cannot be Mached with Attribute Column";
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    if (ismatched == "N")
                                    {
                                        excelConnection.Close();
                                        return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                Errdatatable.Columns.Add("gid");
                                Errdatatable.Columns.Add("Success / Error");

                                for (int l = 0; l < dtexceldata.Rows.Count; l++) {
                                    Errdatatable.Rows[l]["gid"] = (l).ToString();
                                }

                                List<int> removerows = new List<int>();
                                string[] FileList =new string[flbulkfiles.LongLength];
                                int iloopcount =0;
                                foreach (HttpPostedFileBase file in flbulkfiles)
                                {
                                    if (file != null)
                                    {
                                       
                                        FileList[iloopcount] = file.FileName.ToUpper();
                                        iloopcount += 1;
                                    }
                                }
                                int filematchingcount = 0;
                                //compare selected files are matched to be mentioned in excel sheet.
                                if (mode == "AI")
                                {
                                   
                                    for (int n = 0; n < dtexceldata.Rows.Count; n++)
                                    {

                                        string excelfilename = dtexceldata.Rows[n]["File Name"].ToString().ToUpper();
                                   
                                        if(FileList.Contains(excelfilename) )
                                        {
                                            filematchingcount += 1;
                                        }
                                        //foreach (HttpPostedFileBase file in flbulkfiles)
                                        //{
                                        //    if (file != null)
                                        //    {
                                        //        string physicalfilename = file.FileName;
                                        //        if (excelfilename == physicalfilename.ToUpper())
                                        //        {
                                        //            isfilematched = "Y";
                                        //            break;
                                        //        }
                                        //        else
                                        //        {
                                        //            isfilematched = "N";
                                        //        }
                                        //    }
                                        //}
                                        //if (isfilematched == "N")
                                        //{
                                        //    Errdatatable.Rows[n][Errdatatable.Columns.Count - 1] = "File Name Not Macthced with File Selection";
                                        //}
                                        if (filematchingcount == flbulkfiles.LongLength)
                                        {
                                            break;
                                        }
                                        //else
                                        //{
                                        //    Errdatatable.Rows[n][Errdatatable.Columns.Count - 1] = "File Name Not Macthced with File Selection";
                                        //}
                                    }
                                    if (filematchingcount != flbulkfiles.LongLength)
                                    {
                                        Errdatatable.Rows[filematchingcount+1][Errdatatable.Columns.Count - 1] = "File Name Not Macthced with File Selection";
                                    }
                                }
                                else {
                                    for (int n = 0; n < dtexceldata.Rows.Count; n++)
                                    {
                                        string excelfilename = dtexceldata.Rows[n]["File Name"].ToString().ToUpper();

                                        int filenamechk = bulkserviceObj.getfilenamecheck(ModelObj, excelfilename);
                                        if (filenamechk <= 0)
                                        {
                                            Errdatatable.Rows[n][Errdatatable.Columns.Count - 1] = "File Name Cannot Matched with DB";
                                            removerows.Add(n);
                                        }
                                    }
                                }


                                //for remove unwanted files in excel i.e not available in DB
                                try
                                {
                                    for (int q = 0; q < removerows.Count; q++) {
                                        dtexceldata.Rows[q].Delete();
                                    }
                                    dtexceldata.AcceptChanges();
                                }
                                catch (Exception er) { 
                                
                                }

                                //check attributes names
                                string validationflag = "";
                                for (int q = 0; q < dt.Rows.Count; q++) {
                                    errormessage = "";
                                    string dtname = dt.Rows[q]["Atr_Name"].ToString().ToUpper();
                                    for (int k = 0; k < dtexceldata.Columns.Count; k++) {
                                        if (dtname == dtexceldata.Columns[k].ToString().ToUpper())
                                        {
                                            validationflag = "Y";
                                            break;
                                        }
                                        else {
                                            validationflag = "N";
                                        }
                                    }
                                }

                                if (validationflag != "Y") {
                                    excelConnection.Close();
                                    errormessage = "Attributes Names Are Not Matched With DB";
                                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                                }


                                string datavalidation_flag = "";

                            dl1: for (int g = 0; g < dt.Rows.Count; g++) {
                                    errormessage = "";
                                    string dttype = dt.Rows[g]["Atr_Type"].ToString();
                                    string dtname = dt.Rows[g]["Atr_Name"].ToString().ToUpper();
                                    int dtlenght = Convert.ToInt32(dt.Rows[g]["Atr_Length"].ToString());
                                    string dtmandotry = (string)dt.Rows[g]["Atr_Mandotry"].ToString().ToUpper();
                                    int dtlov = Convert.ToInt32(dt.Rows[g]["Lov_Id"].ToString());

                                dl2: for (int h = 0; h < dtexceldata.Columns.Count; h++)
                                    {
                                        string dtexcelname = dtexceldata.Columns[h].ToString();
                                        if (dtname == dtexcelname.ToUpper())
                                        {
                                            dl3: for (int i = 0; i < dtexceldata.Rows.Count; i++) {
                                                datavalidation_flag = "";
                                                errormessage = "";
                                                string userdata = dtexceldata.Rows[i][dtexcelname].ToString();
                                                switch (dttype)
                                                {
                                                    case "Alpha Numeric":
                                                        if (dtmandotry == "Y")
                                                        {
                                                            if (userdata != "" && userdata != null)
                                                            {
                                                                int userdatalenght = userdata.Length;
                                                                if (dtlenght >= userdatalenght)
                                                                {
                                                                    datavalidation_flag = "Y";
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    if (errormessage == "")
                                                                    {
                                                                        errormessage = dtexcelname + "Attribute Length Cannot be Greater than " + dtlenght;
                                                                    }
                                                                    else
                                                                    {
                                                                        errormessage += dtexcelname + "Attribute Length Cannot be Greater than " + dtlenght;
                                                                    }
                                                                    datavalidation_flag = "N";
                                                                }
                                                            }
                                                            else {
                                                                datavalidation_flag = "N";
                                                                if (errormessage == "")
                                                                {
                                                                    errormessage = dtexcelname + "Cannot be Blank ";
                                                                }
                                                                else
                                                                {
                                                                    errormessage += dtexcelname + "Cannot be Blank ";
                                                                }
                                                            }
                                                        }
                                                        else { 
                                                        datavalidation_flag = "Y";
                                                        continue;
                                                        }
                                                        break;
                                                    case "Numeric":
                                                        bool isnum = IsDigitsOnly(userdata);
                                                        if (isnum)
                                                        {
                                                                datavalidation_flag = "Y";
                                                                continue;
                                                        }
                                                        else
                                                        {
                                                             if (errormessage == "")
                                                                {
                                                                    errormessage = dtexcelname + "Numeric Data is Required Only";
                                                                }
                                                                else
                                                                {
                                                                    errormessage += dtexcelname + "Numeric Data is Required Only";
                                                                }
                                                            datavalidation_flag = "N";


                                                        }
                                                        break;
                                                    case "Amount":
                                                        if (dtmandotry == "Y")
                                                        {
                                                            if (userdata != "" && userdata != null)
                                                            {
                                                                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                                                                bool isFloat = regex.IsMatch(userdata);
                                                                if (isFloat == true)
                                                                {
                                                                    datavalidation_flag = "Y";
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    if (errormessage == "")
                                                                    {
                                                                        errormessage = dtexcelname + "Amount Data is Required Only";
                                                                    }
                                                                    else
                                                                    {
                                                                        errormessage += dtexcelname + "Amount Data is Required Only";
                                                                    }
                                                                    datavalidation_flag = "N";
                                                                    //   break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (errormessage == "")
                                                                {
                                                                    errormessage = dtexcelname + "Cannot be Blank ";
                                                                }
                                                                else
                                                                {
                                                                    errormessage += dtexcelname + "Cannot be Blank ";
                                                                }
                                                                datavalidation_flag = "N";
                                                                //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            datavalidation_flag = "Y";
                                                            continue;
                                                        }
                                                        break;
                                                    case "Datetime":
                                                        if (dtmandotry == "Y")
                                                        {
                                                            if (userdata != "" && userdata != null)
                                                            {
                                                                DateTime date;
                                                                if (DateTime.TryParse(userdata, out date))
                                                                {
                                                                    datavalidation_flag = "Y";
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    if (errormessage == "")
                                                                    {
                                                                        errormessage = dtexcelname + "Datetime Data is Required Only";
                                                                    }
                                                                    else
                                                                    {
                                                                        errormessage += dtexcelname + "Datetime Data is Required Only";
                                                                    }
                                                                    datavalidation_flag = "N";
                                                                    //  break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                datavalidation_flag = "N";
                                                                //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            datavalidation_flag = "Y";
                                                            continue;
                                                        }
                                                        break;
                                                    case "Lov Name":
                                                        if (dtmandotry == "Y")
                                                        {
                                                            if (userdata != "" && userdata != null)
                                                            {
                                                                string LOV_Validation = bulkserviceObj.validateLOV(dtlov, userdata);
                                                                if (LOV_Validation == "valid")
                                                                {
                                                                    datavalidation_flag = "Y";
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    if (errormessage == "")
                                                                    {
                                                                        errormessage = dtexcelname + "Lov Name Data is Not Matched With Lov Master";
                                                                    }
                                                                    else
                                                                    {
                                                                        errormessage += dtexcelname + "Lov Name Data is Not Matched With Lov Master";
                                                                    }
                                                                    datavalidation_flag = "N";
                                                                    //  break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (errormessage == "")
                                                                {
                                                                    errormessage = dtexcelname + "Cannot be Blank ";
                                                                }
                                                                else
                                                                {
                                                                    errormessage += dtexcelname + "Cannot be Blank ";
                                                                }
                                                                datavalidation_flag = "N";
                                                                //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            datavalidation_flag = "Y";
                                                            continue;
                                                        }
                                                        break;
                                                    case "Logical":
                                                        if (dtmandotry == "Y")
                                                        {
                                                            if (userdata != "" && userdata != null)
                                                            {
                                                                if (userdata.ToUpper() == "Y" || userdata.ToUpper() == "N")
                                                                {
                                                                    datavalidation_flag = "Y";
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    if (errormessage == "")
                                                                    {
                                                                        errormessage = dtexcelname + "Logical Data is Required only";
                                                                    }
                                                                    else
                                                                    {
                                                                        errormessage += dtexcelname + "Logical Data is Required only";
                                                                    }
                                                                    datavalidation_flag = "N";
                                                                    //   break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (errormessage == "")
                                                                {
                                                                    errormessage = dtexcelname + "Cannot be Blank ";
                                                                }
                                                                else
                                                                {
                                                                    errormessage += dtexcelname + "Cannot be Blank ";
                                                                }
                                                                datavalidation_flag = "N";
                                                                //  break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            datavalidation_flag = "Y";
                                                            continue;
                                                        }
                                                        break;
                                                
                                                }

                                                if (datavalidation_flag == "N")
                                                {

                                                    if (Errdatatable.Rows[i][Errdatatable.Columns.Count - 1].ToString() == "")
                                                    {
                                                        Errdatatable.Rows[i][Errdatatable.Columns.Count - 1] = errormessage;
                                                    }
                                                    else {
                                                        Errdatatable.Rows[i][Errdatatable.Columns.Count - 1] = Errdatatable.Rows[i][Errdatatable.Columns.Count - 1].ToString() + ", " + errormessage;
                                                    }
                                                }


                                            }  //dl3 loop close    

                                        }
                                    } //dl2 loop close
                                }


                                string _wherecondition = "";

                                for (int i = 0; i < dtexceldata.Rows.Count; i++)
                                {
                                    errormessage = "";
                                    _wherecondition = "";
                                    for (int j = 0; j < dtexceldata.Columns.Count - 1; j++)
                                    {
                                        if (dtexceldata.Columns.Count - 2 == j)
                                        {
                                            _wherecondition += "`" + dtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + dtexceldata.Rows[i][j].ToString().Trim() + "'";
                                        }
                                        else
                                        {
                                            _wherecondition += "`" + dtexceldata.Columns[j].ToString().Trim() + "`" + "=" + "'" + dtexceldata.Rows[i][j].ToString().Trim() + "'" + " " + "AND" + " ";
                                        }

                                    }
                                    string attributevalidationflag = "";
                                    try
                                    {
                                        attributevalidationflag = bulkserviceObj.validateattribute(ModelObj, _wherecondition);//Check attributes is already exist.
                                        _wherecondition = "";
                                    }
                                    catch (Exception e)
                                    {
                                        attributevalidationflag = e.ToString();
                                        _wherecondition = "";
                                    }
                                    if (attributevalidationflag == "duplicate not found")
                                    {
                                        
                                    }
                                    else
                                    {
                                        if (errormessage == "")
                                        {
                                            errormessage = "The Attributes Values Combination are Already available for Another file";
                                        }
                                        else
                                        {
                                            errormessage = errormessage + ",The Attributes Values Combination are Already available for Another file";
                                        }
                                        failcount++;
                                        try
                                        {
                                            if (Errdatatable.Rows[i][Errdatatable.Columns.Count - 1].ToString() == "")
                                            {
                                                Errdatatable.Rows[i][Errdatatable.Columns.Count - 1] = errormessage;
                                            }
                                            else
                                            {
                                                Errdatatable.Rows[i][Errdatatable.Columns.Count - 1] = Errdatatable.Rows[i][Errdatatable.Columns.Count - 1].ToString() + ", " + errormessage;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            var iu = e.ToString();
                                        }
                                    }

                                }


                            }
                            excelConnection.Close();

                        }
                    }
                    else
                    {
                        //wriiting code for without attributes.
                    }

                }
                else
                {
                    errormessage = "You have not specified a file..!";
                    return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                }
                int erroravl = 0;
                for (int r = 0; r < Errdatatable.Rows.Count; r++) {
                    var errordesc = Errdatatable.Rows[r][Errdatatable.Columns.Count - 1].ToString();
                    if (errordesc != "" && errordesc != null && errordesc != "undefined" && errordesc != "null")
                    {
                        erroravl = 1;
                    }
                    else {
                        Errdatatable.Rows[r][Errdatatable.Columns.Count - 1] = "Success";
                    }
                    
                }

                    Data1 = JsonConvert.SerializeObject(failcount);
                Data2 = JsonConvert.SerializeObject(errormessage);
                Data3 = JsonConvert.SerializeObject(Errdatatable);
                string Data4 = JsonConvert.SerializeObject(erroravl);
                Session["Errdatatable"] = Errdatatable;
                return Json(new { Data1, Data2, Data3,Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errormessage = "Invalid excel file..!";

                return Json(new { failcount, errormessage }, JsonRequestBehavior.AllowGet);
                logger.Error(ex.ToString());
            }
        }
      

        //Download Excel
        public ActionResult downloadsexcel()
        {
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "FileBulkUpload.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }


        //Save Documents.
        public int savefiles(HttpPostedFileBase[] flbulkfiles, DocArchival_Model ModelObj, DataTable dtexceldata, HttpPostedFileBase flexcelfile, int userid)
        {
            int message = 0;
            foreach (HttpPostedFileBase file in flbulkfiles)
            {
                if (file != null)
                {
                    int result = bulkserviceObj.SaveSingleFile(ModelObj, file);
                    if (result > 0)
                    {
                        message = getindexedinialatributes(result, dtexceldata, file.FileName);
                    }
                    else
                    {
                        //ViewBag.Message = "Upload Failed.!";
                        //return View("DocArchivalBulkUpload");
                    }
                }
            }
            if (message == 1)
            {
                int msg = bulkserviceObj.saveexclefile(flexcelfile, userid);
                //if (msg == 1)
                //{
                //    ViewBag.Message = flbulkfiles.Count() + " " + "Files Uploaded and Indexed Scuccessfully..!!";
                //}
            }
            else
            {
                //ViewBag.Message = "Uploading Failed..!!";
            }
            return message;
        }

        [HttpPost]
        public void flexcelfile_onchange(HttpPostedFileBase flexcelfile)//onchange method.
        {
            Session["Singlefile"] = null;
            Session["Singlefile"] = flexcelfile;
            //foreach (string file in Request.Files)
            //{
            //    var fileContent = Request.Files[file];

            //    if (fileContent != null && fileContent.ContentLength > 0)
            //    {
            //        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
            //        Session["Singlefile"] = hpf;
            //    }
            //}
        }

        [HttpPost]
        public void flbulkfiles_onchange(HttpPostedFileBase[] flbulkfiles) //onchange method.
        {
            Session["Multiplefile"] = null;
            Session["Multiplefile"] = flbulkfiles;
        }

        public int getindexedinialatributes(int id, DataTable dtx, string psotedfilename)//perform indexing activity.
        {
            SetDocAttributes_Model deptsModel = new SetDocAttributes_Model();//Creating SetDocAttributes model object.
            SetDocAttributes_Service ServiceObj = new SetDocAttributes_Service();//Creating SetDocAttributes service object.
            List<SetDocAttributes_Model> ModelObjList1 = new List<SetDocAttributes_Model>();//Creating list. 
            try
            {
                DataSet ds = new DataSet();
                ds = ServiceObj.InitValues(id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    deptsModel.FileName = ds.Tables[0].Rows[0][1].ToString();
                    deptsModel.FileType = ds.Tables[0].Rows[0][2].ToString();
                    deptsModel.HideDocArchId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    ViewBag.FilePath = ds.Tables[0].Rows[0][3].ToString();
                    deptsModel.Filepathfrom = ds.Tables[0].Rows[0][3].ToString();
                    deptsModel.VersionDate = DateTime.Today;
                    deptsModel.VersionName = "0.1";
                    deptsModel.Signature = "Test";
                    deptsModel.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                   // deptsModel.DeptName = ds.Tables[1].Rows[0][3].ToString();
                    //deptsModel.UnitName = ds.Tables[1].Rows[0][5].ToString();
                    deptsModel.CateName = ds.Tables[1].Rows[0]["Dgroup_name"].ToString();
                    deptsModel.SubCateName = ds.Tables[1].Rows[0]["Dname_name"].ToString();
                }
                DataTable dt = new DataTable();
                dt = ds.Tables[3];

                string category = deptsModel.CateName;
                string subcategory = deptsModel.SubCateName;

                string filePath = ConfigurationManager.AppSettings["Path"].ToString();

                string Catename = filePath + "\\" + category + "\\" + subcategory;
                string SubCatename = subcategory;

                string ExistingfilePath = deptsModel.Filepathfrom;
                deptsModel.Filepathto = filePath +"\\"+ category + "\\" + SubCatename + "\\" + deptsModel.FileName;

                bool loggingDirectoryExists = false;

                DirectoryInfo ObjDirectoryInfo = new DirectoryInfo(Catename);

                if (ObjDirectoryInfo.Exists)
                {
                    loggingDirectoryExists = true;
                    DirectoryInfo ObjSubDirectoryInfo = new DirectoryInfo(SubCatename);
                    //if (ObjSubDirectoryInfo.Exists)
                    //{
                    //    loggingDirectoryExists = true;
                    //}
                    //else
                    //{
                    //    ObjSubDirectoryInfo = ObjDirectoryInfo.CreateSubdirectory(SubCatename);
                    //}
                }
                else
                {
                    Directory.CreateDirectory(Catename);
                    ObjDirectoryInfo = new DirectoryInfo(Catename);
                 //   DirectoryInfo ObjSubDirectoryInfo = ObjDirectoryInfo.CreateSubdirectory(SubCatename);
                    loggingDirectoryExists = true;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SetDocAttributes_Model model = new SetDocAttributes_Model();
                    model.AtrLabel1 = dt.Rows[i][1].ToString();
                    model.AtrType = dt.Rows[i][3].ToString();
                    model.AtrLen = dt.Rows[i][2].ToString();
                    model.AtrMand = dt.Rows[i][4].ToString();
                    model.AtrLovId = Convert.ToInt32(dt.Rows[i][5].ToString());
                    model.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
                    string aname = model.AtrLabel1.ToString();

                    
                        for (int j = 0; j < dtx.Rows.Count; j++)
                        {
                            if (psotedfilename.ToUpper() == dtx.Rows[j]["File Name"].ToString().ToUpper())
                            {
                                if (model.AtrType.ToUpper() == "AUTONUMBER")
                                {
                                    DataTable dt1 = new DataTable();
                                    dt1 = ServiceObj.getmasterval(model.AtrLovId, model.AtrType);
                                    foreach (DataRow dr1 in dt1.Rows)
                                    {

                                        model.AtrText1 = dr1["master_name"].ToString();
                                        // depts.autonumberrowid = dr1["master_code"].ToString();
                                    }
                                    ModelObjList1.Add(model);
                                    break;
                                }
                                else {
                                    model.AtrText1 = dtx.Rows[j][aname].ToString().Trim();
                                    ModelObjList1.Add(model);
                                    break;
                                }
                            }
                        }
                }
                int Result123 = ServiceObj.SaveProperties(deptsModel, ModelObjList1);
                if (Result123 == 1)
                {
                    string Source = ExistingfilePath;
                    FileInfo objfile = new FileInfo(Source);
                    {
                        string Destination = Path.Combine(Catename);
                        System.IO.File.Move(Source, Destination + "\\" + deptsModel.FileName);
                    }
                }
                return Result123;

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return 0;
            }
        }

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            if (str == "") {
                return false;
            }
            return true;
        }

        public JsonResult deleteSessionIndexfile(string GridID)
        {
            int removefileno = Convert.ToInt32(GridID);
            var errorstat = 0;
            try
            {
                DataTable errortable = Session["Errdatatable"] as DataTable;
                DataTable dtexceldata = Session["dtexceldata"] as DataTable;
                errortable.Rows[removefileno].Delete();
                errortable.AcceptChanges();
                dtexceldata.Rows[removefileno].Delete();
                dtexceldata.AcceptChanges();
                for (int i = 0; i < errortable.Rows.Count; i++)
                {
                    var _data = errortable.Rows[i][errortable.Columns.Count - 1].ToString().ToUpper();
                    if (_data != "SUCCESS")
                    {
                        errorstat = 1;
                    }

                }
                Session["Errdatatable"] = null;
                Session["Errdatatable"] = errortable;
                Session["dtexceldata"] = null;
                Session["dtexceldata"] = dtexceldata;
            }
            catch (Exception e) {
                logger.Error(e.ToString());
            }
            var _errorstat = JsonConvert.SerializeObject(errorstat);
            return Json(new { _errorstat }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveIndexOnlyandArchival(int Docgroup, int Docname,string mode)
        {
            int result = 0;
            string errormsg = "";
            var message = 0;
            ModelObj.CatgId = Docgroup;
            ModelObj.SubCatgId = Docname;
            ModelObj.UserId = Convert.ToInt32(Session["Emp_Id"].ToString());
            DataTable dtexceldata = Session["dtexceldata"] as DataTable;
            HttpPostedFileBase[] flbulkfiles = Session["Multiplefile"] as HttpPostedFileBase[];

            try
            {
                if (mode == "AI")
                {
                    for (int h = 0; h < dtexceldata.Rows.Count; h++)
                    {
                        foreach (HttpPostedFileBase file in flbulkfiles)
                        {
                            if (file != null)
                            {
                                if (dtexceldata.Rows[h]["File Name"].ToString() == file.FileName)
                                {
                                    result = bulkserviceObj.SaveSingleFile(ModelObj, file);

                                    if (result > 0)
                                    {
                                        message = getindexedinialatributes(result, dtexceldata, file.FileName);

                                        if (message > 0)
                                        {
                                            errormsg = "";
                                            //break;
                                        }
                                        else
                                        {
                                            errormsg = "File Saved but File Indexing Failed while Store Document Attributes on DataBase!";
                                            // break;
                                        }
                                    }
                                    else
                                    {
                                        errormsg = "File Archival Failed on While Store File Attributes on DataBase!";
                                        // break;
                                    }
                                }
                            }
                        }
                    }

                }
                else {
                    for (int h = 0; h < dtexceldata.Rows.Count; h++)
                    {
                        result = bulkserviceObj.getfilenamecheck(ModelObj, dtexceldata.Rows[h]["File Name"].ToString().ToUpper());
                        if (result > 0)
                        {
                           // result = 0;
                            message = getindexedinialatributes(result, dtexceldata, dtexceldata.Rows[h]["File Name"].ToString());

                            if (message > 0)
                            {
                                errormsg = "";
                                //break;
                            }
                            else
                            {
                                errormsg = "File Indexing Failed while Store Document Attributes on DataBase!";
                                // break;
                            }
                        }
                        else
                        {
                            errormsg = "File Name Not Matched with DataBase!";
                            // break;
                        }
                    }
                }
            }
            catch (Exception er) {
                logger.Error(er.ToString());
            }
            var Data1 = JsonConvert.SerializeObject(errormsg);
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }
    }

}