using DMS.Model;
using DMS.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Web;
using MySql.Data.MySqlClient;
using DMS.Web.Filters;
using System.Text.RegularExpressions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class SearchController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SearchController));  //Declaring Log4Net 
        Search_Service ServiceObj = new Search_Service();
        ViewDocuments_Model deptsModel = new ViewDocuments_Model();
        // GET: Search
        public ActionResult Search(List<ViewDocuments_Model> Grid_list)
        {
            return View(deptsModel);
        }

        public ActionResult DynamicAttributes(ViewDocuments_Model vdObj)
        {
            return PartialView(vdObj);
        }

        //Post method for getting dynamic attributes.
        [HttpPost]
        public ActionResult GetDynamicFields(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ServiceObj.GetDynamicFields(DeptID1, Unit1, Dgroup1, Dname1);
                string Count = Convert.ToString(dt.Rows.Count);
                int Checkrcd = Convert.ToInt32(dt.Rows[0][0].ToString());
                ViewBag.AttributeCount = Convert.ToInt32(Count);
                int ctlval = 0;
                string type;
                int LovId;
                DataTable dt1 = new DataTable();
                if (Checkrcd == 0)
                {
                    //show the alert message..
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ViewDocuments_Model depts = new ViewDocuments_Model();
                        depts.attrbname = dr["Atr_Name"].ToString();
                        depts.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
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
                            depts.LovName = item8;
                        }
                        depts.attrbtype = dr["Atr_Type"].ToString();
                        deptsModel.dept.Add(depts);
                        ctlval++;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return PartialView("DynamicAttributes", deptsModel);
        }

        public ActionResult GetDocuments(string[] form, int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1)
        {
            List<ViewDocuments_Model> Grid_list = new List<ViewDocuments_Model>();
            try
            {
                string[] attrnameval = form[0].ToString().Split(',');
                string retval = string.Empty;
                int i = 1;
                retval = "0";
                foreach (string item in attrnameval)
                {
                    if (item == "''" && retval == "0")
                    {
                        retval = "0";
                    }
                    else
                    {
                        if (retval == "0")
                        {
                            retval = "";
                        }
                        if (i == attrnameval.Length)
                        {
                            if (item.ToString() != string.Empty)
                            {
                                retval = retval + item.ToString();
                            }
                        }
                        else
                        {
                            if (item.ToString() != string.Empty)
                            {
                                retval = retval + item.ToString() + ",";
                            }
                        }
                    }
                    i++;
                }

                if (retval == "")
                {
                    retval = "0";
                }

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = ServiceObj.GetDocuments(DeptID1, Unit1, Dgroup1, Dname1, retval);

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
                string Data1, Data2;
                Data1 = JsonConvert.SerializeObject(dt);
                Data2 = JsonConvert.SerializeObject(dt1);
                //Data1 = JsonConvert.SerializeObject(dt);
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        //ActionResult for fetching Linked/Grouped Documents. selva
        public ActionResult ShowLinkedDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                ds = ServiceObj.GetLinkedDocuments(DeptID1, Unit1, Dgroup1, Dname1, Attribid);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dt1 = ds.Tables[1];
                    }

                }

                string Data3, Data4;
                Data3 = JsonConvert.SerializeObject(dt);
                Data4 = JsonConvert.SerializeObject(dt1);

                return Json(new { Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        //Show InterFiling Documents
        public ActionResult ShowInterFilingDocuments(int? DeptID1, int? Unit1, int? Dgroup1, int? Dname1, int? Attribid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                ds = ServiceObj.GetInterFilingLinkedDocuments(DeptID1, Unit1, Dgroup1, Dname1, Attribid);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dt1 = ds.Tables[1];
                    }
                }
                string Data3;
                string Data4;
                Data3 = JsonConvert.SerializeObject(dt);
                Data4 = JsonConvert.SerializeObject(dt1);
                return Json(new { Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        #region Download File
        public ActionResult DownloadFile(int attacheid)
        {
            try
            {
                int Attachid = attacheid;
                string file = ServiceObj.SingleFileDownload(Attachid);

                var fileBytes = System.IO.File.ReadAllBytes(file);
                var response = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = file
                };
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();
        }

        public ActionResult Download()
        {
            try
            {
                int Attachid = Convert.ToInt32(Request.Form["attacheid"].ToString());
                string file = ServiceObj.SingleFileDownload(Attachid);

                var fileBytes = System.IO.File.ReadAllBytes(file);
                var response = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = file
                };
                return new JsonResult()
                {
                    Data = new { attacheid = Attachid }
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return View();//not required
        }
        #endregion

        public ActionResult DownloadMailFile1(string downloadpath)
        {
            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(downloadpath);
                var response = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = downloadpath
                };
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadpath);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }
        }

        public ActionResult Downloadmailfile(string downloadpath)
        {
            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(downloadpath);
                var response = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = downloadpath
                };
                return new JsonResult()
                {
                    Data = new { downloadpath = downloadpath }
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return View();
            }

        }

        //#region DisplayOutlook
        //public ActionResult DisplayOutlook()
        //{
        //    try
        //    {
        //        int Attachid = Convert.ToInt32(Request.Form["attacheid"].ToString());
        //        string file = ServiceObj.SingleFileDownload(Attachid);
        //        Outlook.Application oApp = new Outlook.Application();
        //        Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
        //        oMailItem.Attachments.Add(file);
        //        oMailItem.Display(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //    }
        //    return View();
        //}
        //#endregion

        public ActionResult Showmailpartialview()
        {
            try
            {
                return PartialView("OutlookPartialView");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("OutlookPartialView");
            }
        }

        public JsonResult Getgetfromidandattachfile(int? Indexed_gid, string type)
        {
            string Frommailaddress = "";
            string Attachementfile = "";
            string attachementlocation = "";
            Int32 EmpID = Convert.ToInt32(Session["Emp_Id"].ToString());
            DataSet ds = new DataSet();
            try
            {
                ds = ServiceObj.Getmaildetails(Indexed_gid, EmpID, type);
                if (ds.Tables.Count > 0)
                {
                    Frommailaddress = ds.Tables[0].Rows[0][0].ToString();
                    Attachementfile = ds.Tables[1].Rows[0][0].ToString();
                    attachementlocation = ds.Tables[1].Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Frommailaddress, Attachementfile, attachementlocation }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMail(string mailto, string subject, string content, string filelocation)
        {
            try
            {
                string result = "";
                string Mailfrom = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"];
                string Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                int smtport = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                string host = System.Configuration.ConfigurationManager.AppSettings["Host"];

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Mailfrom);
                mail.To.Add(new MailAddress(mailto));
                //mail.Bcc.Add(new MailAddress("Three@gmail.com"));
                mail.Subject = subject;
                mail.Body = content;

                Attachment attach = new Attachment(filelocation);
                mail.Attachments.Add(attach);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = host;
                smtp.Port = smtport;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
                credential.UserName = Mailfrom;
                credential.Password = Password;

                smtp.Credentials = credential;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                string result = "failed";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //Json method for fetching values in databsae to all dropdownlist And passing list values to view through jsonresult.
        public JsonResult DepartmentEdit(string type, string actiontype)
        {

            List<Dep_union_dropdown> dept = new List<Dep_union_dropdown>();
            try
            {
                dept = ServiceObj.GeALL(type, actiontype);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(dept, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to unit,docgroup and docname dropdownlist using DepartmentId based.And passing list values to view through jsonresult. 
        public JsonResult GetDept(int DeptID)
        {
            List<Dep_union_dropdown> Get_unit = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname = new List<Dep_union_dropdown>();
            try
            {
                Get_unit = ServiceObj.GetUnit(DeptID);
                Get_docgroup = ServiceObj.GetDocGroup(DeptID);
                Get_docname = ServiceObj.GetDocName(DeptID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_unit, Get_docgroup, Get_docname }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnit(int UnitID)
        {
            List<Dep_union_dropdown> Get_dept1 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup1 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname1 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept1 = ServiceObj.GetDept1(UnitID);
                Get_docgroup1 = ServiceObj.GetDocGroup1(UnitID);
                Get_docname1 = ServiceObj.GetDocName1(UnitID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept1, Get_docgroup1, Get_docname1 }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,Unit and docname dropdownlist using DocgroupId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocGroup(int DocGroupID)
        {
            List<Dep_union_dropdown> Get_dept2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit2 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docname2 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept2 = ServiceObj.GetDept2(DocGroupID);
                Get_unit2 = ServiceObj.GetUnit2(DocGroupID);
                Get_docname2 = ServiceObj.GetDocName2(DocGroupID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept2, Get_unit2, Get_docname2 }, JsonRequestBehavior.AllowGet);
        }

        //Json method for fetching values in databsae to department,Unit and docgroup dropdownlist using DocnameId based.And passing list values to view through jsonresult. 
        public JsonResult GetDocName(int DocNameId)
        {
            List<Dep_union_dropdown> Get_dept3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_unit3 = new List<Dep_union_dropdown>();
            List<Dep_union_dropdown> Get_docgroup3 = new List<Dep_union_dropdown>();
            try
            {
                Get_dept3 = ServiceObj.GetDept3(DocNameId);
                Get_unit3 = ServiceObj.GetUnit3(DocNameId);
                Get_docgroup3 = ServiceObj.GetDocGroup3(DocNameId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Get_dept3, Get_unit3, Get_docgroup3 }, JsonRequestBehavior.AllowGet);
        }


    }
}