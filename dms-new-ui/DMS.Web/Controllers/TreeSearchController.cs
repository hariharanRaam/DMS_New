using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS.Model;
using DMS.Service;
using System.Data;
using Newtonsoft.Json;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net;
using System.Net.Mail;
using GleamTech.DocumentUltimate.AspNet.UI;
using GleamTech.Collections;
using GleamTech.DocumentUltimate;
using GleamTech.Data;
using DMS.Web.Filters;

namespace DMS.Web.Controllers
{
    [UserAuntheication]
    public class TreeSearchController : Controller
    {
        SetDocAttributes_Model deptsModel1 = new SetDocAttributes_Model();
        SetDocAttributes_Model depts1;
        SetDocAttributes_Service ServiceObj1 = new SetDocAttributes_Service();
        ViewDocument_Services ServiceObj2 = new ViewDocument_Services();
        PhysicalArchival_Model modelObj = new PhysicalArchival_Model();
        Search_Service ServiceObj = new Search_Service();
        BasicReport_Service serviceObj = new BasicReport_Service();
        DocGroupMaster_Service Docgrpsrv = new DocGroupMaster_Service();
    
        ViewDocument_Services ObjService = new ViewDocument_Services();
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(TreeSearchController));  //Declaring Log4Net 
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

        // GET: Hierarchy
        public ActionResult Index()
        {
            TreeSearch_services context = new TreeSearch_services();
            try
            {//GetTreeData
                /*  var plist = context.HierarchyDetails.Where(p => p.PerentId == null).Select(a => new
                  {
                      a.Id,
                      a.HierarchyName
                  }).ToList();*/
                ViewBag.plist = context.GetTreeData();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
           // GetHierarchy();
            return View();
        }

        public JsonResult GetHierarchy()
        {
            List<HierarchyDetail> hdList;
            List<HierarchyViewModel> records;
            TreeSearch_services context = new TreeSearch_services();
            //try
            // {
            hdList = context.GetTreeData().ToList();

            records = hdList.Where(l => l.PerentId == 0)
                .Select(l => new HierarchyViewModel
                {
                    Id = l.Id,
                    text = l.HierarchyName,
                    perentId = l.PerentId,
                    children = GetChildren(hdList, l.Id)
                    //children = GetChildren(hdList, l.PerentId)
                }).ToList();
            // }
            //catch (Exception ex)
            //{
            //    logger.Error(ex.ToString());
            //}

            return this.Json(records, JsonRequestBehavior.AllowGet);
            // return View();
        }

        private List<HierarchyViewModel> GetChildren(List<HierarchyDetail> hdList, Int64 parentId)
        {
            return hdList.Where(l => l.PerentId == parentId)
                .Select(l => new HierarchyViewModel
                {
                    Id = l.Id,
                    text = l.HierarchyName,
                    perentId = l.PerentId,
                    children = GetChildren(hdList, l.Id)
                }).ToList();
        }

        [HttpPost]
        public ActionResult AttributeDataPartial(Int64 DocgroupId, Int64 DocNameId)
        {
            //string Data1 = "";
            TreeSearch_services objservice = new TreeSearch_services();
            //List<DMS.Model.Attribute> deptsModel = new List<DMS.Model.Attribute>();
            TreeSearch_Model deptsModel = new TreeSearch_Model();
            Int32 _empid = Convert.ToInt32(Session["Emp_Id"].ToString());
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = objservice.GetAttributes(DocgroupId,DocNameId,_empid);
                dt = ds.Tables[0];
                //Data1 = JsonConvert.SerializeObject(dt);
                // deptsModel = objservice.GetAttributes(data, _empid);
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
                        TreeSearch_Model depts = new TreeSearch_Model();
                        depts.Atr_Name = dr["Atr_Name"].ToString();
                        depts.attrctlname = dr["Atr_Name"].ToString() + ctlval.ToString();
                        depts.Dname_Name = dr["Dname_Name"].ToString();
                        depts.Dgroup_Name = dr["Dgroup_Name"].ToString();
                        depts.Dgroup_Id = Convert.ToInt64(dr["Dgroup_Id"].ToString());
                        depts.Dname_Id = Convert.ToInt64(dr["Dname_Id"].ToString());
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
                            ViewBag.Lovexl_Name = item8;
                            depts.LovName = item8;
                        }
                        depts.Atr_Type = dr["Atr_Type"].ToString();
                        List<SelectListItem> operat = new List<SelectListItem>();
                        operat.Add(new SelectListItem
                        {
                            Text = "AND",
                            Value = "AND"
                        });
                        operat.Add(new SelectListItem
                        {
                            Text = "OR",
                            Value = "OR"
                        });
                        depts.operators = operat;

                        List<SelectListItem> cond = new List<SelectListItem>();
                        cond.Add(new SelectListItem
                        {
                            Text = "Contains",
                            Value = "Contains"
                        });
                        cond.Add(new SelectListItem
                        {
                            Text = "StartWith",
                            Value = "StartWith"
                        });
                        cond.Add(new SelectListItem
                        {
                            Text = "EndWith",
                            Value = "EndWith"
                        });
                        cond.Add(new SelectListItem
                        {
                            Text = "Equal",
                            Value = "Equal"
                        });
                        depts.Conditions = cond;

                        deptsModel.dept.Add(depts);
                        ctlval++;
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return PartialView(deptsModel);
        }

        public ActionResult testpartial(int obj)
        {
            return PartialView(obj);
        }

        public ActionResult AttributeDataPartial(TreeSearch_Model obj)
        {

            return PartialView(obj);
        }

        public ActionResult GetDocuments(string Attributetypes1, string atrname1, string Docgroupid1, string Docnameid1, string Dyntextvalues1, string Conditions1, string operators1)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            TreeSearch_services objservice = new TreeSearch_services();
            Int32 _empid = Convert.ToInt32(Session["Emp_Id"].ToString());
            ds = objservice.GetDocuments(Attributetypes1, atrname1, Docgroupid1, Docnameid1, _empid, Dyntextvalues1, Conditions1, operators1);
            if (ds.Tables.Count > 0)
            {
                // var UniqueRows = ds.Tables[0].AsEnumerable().Distinct(DataRowComparer.Default);
                //  dt = UniqueRows.CopyToDataTable();
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

        public ActionResult showdocuments(int? viewid, string mode)
        {
            string filepath = "";
            try
            {
                filepath = ObjService.SingleFileDownload(viewid, mode);
                ViewBag.FilePath = filepath;
                return PartialView("DocpartialviewForTreeSearch", ViewBag.FilePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("DocpartialviewForTreeSearch");
            }
        }

        public ActionResult Showmailpartialview()
        {
            try
            {
                return PartialView("OutlookPartialViewForTreeSearch");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("OutlookPartialViewForTreeSearch");
            }
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
                   // deptsModel1.DeptName = ds.Tables[1].Rows[0][3].ToString();
                   // deptsModel1.UnitName = ds.Tables[1].Rows[0][5].ToString();
                    deptsModel1.CateName = ds.Tables[1].Rows[0][3].ToString();
                    deptsModel1.SubCateName = ds.Tables[1].Rows[0][5].ToString();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    ViewBag.Orglvlval4 = ds.Tables[5].Rows[0][8].ToString();
                    ViewBag.Orglvlval3 = ds.Tables[5].Rows[0][7].ToString();
                    ViewBag.Orglvlval2 = ds.Tables[5].Rows[0][6].ToString();
                    ViewBag.Orglvlval1 = ds.Tables[5].Rows[0][5].ToString();
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
                                Text = dr1["master_name"].ToString(),
                                Value = dr1["master_code"].ToString()
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
                DataTable dt6= new DataTable();
                dt6 = ds.Tables[4];
                //int ctlval1= 0;

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

        #region Linked & Interfiling Documents
        public ActionResult ShowlinkandInterfileDocs(Int64 Attribid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();
                DataTable dt6 = new DataTable();
                ds1 = ServiceObj2.GetInterFilingLinkedDocuments( 0, 0, Attribid);
                ds = ServiceObj2.GetLinkedDocuments( 0, 0, Attribid);
                ds2 = ServiceObj2.GetRetrievalaudit(Attribid);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        dt2 = ds.Tables[1];
                    }
                }

                if (ds1.Tables.Count > 1)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        dt3 = ds1.Tables[0];
                    }

                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        dt4 = ds1.Tables[1];
                    }
                }
                if (ds2.Tables.Count > 1)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        dt5 = ds2.Tables[0];
                    }

                    if (ds2.Tables[1].Rows.Count > 0)
                    {
                        dt6 = ds2.Tables[1];
                    }
                }
                string Data3, Data1, Data2, Data4, Data5, Data6;
                Data1 = JsonConvert.SerializeObject(dt1);
                Data2 = JsonConvert.SerializeObject(dt2);
                Data3 = JsonConvert.SerializeObject(dt3);
                Data4 = JsonConvert.SerializeObject(dt4);
                Data5 = JsonConvert.SerializeObject(dt5);
                Data6 = JsonConvert.SerializeObject(dt6);
                return Json(new { Data1, Data2, Data3, Data4, Data5, Data6 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }

        }
        #endregion

        public ActionResult showlinkinterfilingattributes(string type = null, Int64 gid = 0)
        {
            try
            {
                ViewBag.type = type;
                ViewBag.gid = gid;
                return PartialView("LinkInterfilingPartialView");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("LinkInterfilingPartialView");
            }
        }

        public ActionResult showdynamicattributes(int group_gid, string group_type)
        {
            string Data1 = "";
            try
            {
                DataTable dt = new DataTable();
                dt = ServiceObj2.showdynamicattributes(group_gid, group_type);
                if (dt.Rows.Count > 0)
                {
                    Data1 = JsonConvert.SerializeObject(dt);
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Showmailpartialview1(Int64 attachedid = 0, string mode = null)
        {
            try
            {
                ViewBag.attachedid = attachedid;
                ViewBag.mode = mode;
                return PartialView("OutlookPartialViewForSingleFile");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("OutlookPartialViewForSingleFile");
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
                ds = ServiceObj2.Getmaildetails(Indexed_gid, EmpID, type);
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

        public ActionResult SendMail(string mailto, string subject, string content, string filelocation, string mailcc, string mailtype)
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
                //mail.CC.Add(new MailAddress(mailcc));
                //mail.Bcc.Add(new MailAddress("Three@gmail.com"));
                mail.Subject = subject;
                mail.Body = content;

                Attachment attach = new Attachment(filelocation);
                mail.Attachments.Add(attach);

                if (mailcc != "")
                {
                    string[] values = mailcc.Split(';');
                    for (int i = 0; i < values.Length; i++)
                    {
                        string cc = values[i].Trim();
                        mail.CC.Add(new MailAddress(cc));
                    }
                }

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

        //public ActionResult viewstorageattributes(int? attribgid)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        ds = ServiceObj1.storagedata(attribgid);
        //        if (ds.Tables.Count > 0)
        //        {
        //            dt = ds.Tables[0];
        //            Session["dt"] = dt;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                PhysicalArchival_Model depts = new PhysicalArchival_Model();
        //                depts.Satrname = dr["Satr_Name"].ToString();
        //                //depts.Satrid = dr["Satr_Id"].ToString();
        //                depts.Satrtype = dr["Satr_Type"].ToString();
        //                depts.Satrlen = dr["Satr_Length"].ToString();
        //                depts.SatrMand = dr["Satr_Mandotry"].ToString();
        //                if (Aaction == "Edit" || Aaction == "Delete")
        //                {
        //                    depts.Satrdesc = dr["Sattribdtl_Description"].ToString();
        //                }
        //                else
        //                {
        //                    depts.Satrdesc = null;
        //                }
        //                modelObj.dept.Add(depts);
        //            }
        //            //modelObj.SatrCount = Convert.ToInt32(ds.Tables[1].Rows[0]["count"].ToString());
        //        }
        //        return PartialView("DynamicStorageAttributes", modelObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.ToString());
        //        return PartialView("DynamicStorageAttributes", modelObj);
        //    }
        //}

        #region RequestRetrival
        public ActionResult ShowRequestReterivalview()
        {
            try
            {
                ViewBag.Employeeid = Convert.ToInt32(Session["Emp_Id"]);
                ViewBag.Employeename = Session["Employee_Name"].ToString();

                return PartialView("RequestRetrivalPartialView");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("RequestRetrivalPartialView");
            }
        }

        [HttpGet]
        public ActionResult SaveRequestRetival()
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return View();

        }

        [HttpPost]
        public ActionResult SaveRequestRetival(string Requestdate, string Requestno, int noofdcoc, int Employeeid, string Requireddate, string Description, string status, string atttributeid, Int64 RequestType)
        {

            int i = 0;
            int k = 0;
            string Result = "";
            int attributeid = 0;
            string[] array = atttributeid.Split(',');
            try
            {

                i = ServiceObj2.SaveRequestRetrival(Requestdate, Requestno, noofdcoc, Employeeid, Requireddate, Description, status, RequestType);
                if (i > 0)
                {
                    //int j = ServiceObj2.ChildSaveRequestRetrival(i, attributeid);
                    //if (j > 0)
                    //{
                    //    Result = "Success";
                    //}

                    for (k = 0; k < array.Length; k++)
                    {
                        int j = ServiceObj2.ChildSaveRequestRetrival(i, Convert.ToInt32(array[k]), Employeeid);

                        if (j > 0)
                        {
                            Result = "Success";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetRequestRetival()
        {
            int Employeeid = Convert.ToInt32(Session["Emp_Id"]);
            string Data1 = "";
            List<ViewDocuments_Model.RequestRerival> getRequestRetervial = new List<ViewDocuments_Model.RequestRerival>();
            try
            {

                getRequestRetervial = ViewDocument_Services.GetRequestRerival(Employeeid);
                Data1 = JsonConvert.SerializeObject(getRequestRetervial);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }

        
        #endregion

        /*new screen link and delink */
        public ActionResult LinkandDelink()
        {
            return View();
        }

        public ActionResult linkanddelinkNew(int fileid) {
            ViewBag.fileid = fileid;
            return View();
        }

        /* Retreival Request*/

        public ActionResult RetreivalRequest(string Attribid1)
        {
            try
            {
                DataTable dt = new DataTable();
                ViewBag.Employeeid = Convert.ToInt32(Session["Emp_Id"]);
                ViewBag.Employeename = Session["Employee_Name"].ToString();
                ViewBag.Attrib_id = Attribid1;
                dt = ObjService.getDocnameDocgroup(Attribid1);
                foreach (DataRow rs in dt.Rows) {
                    ViewBag.Dname_ID = rs["Dname_ID"].ToString();
                    ViewBag.Dgroup_ID = rs["Dgroup_ID"].ToString();
                    ViewBag.Dname = rs["Dname"].ToString();
                    ViewBag.Dgroup_Name = rs["Dgroup_Name"].ToString();
                }
            }
            catch (Exception p) { 
            
            }
            return View();
        }


        [HttpPost]
        public ActionResult Check_Document_Under_Modification(string Doc_GID)
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                dt = ServiceObj1.Check_Document_Under_Modification(Doc_GID);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
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
                ds = serviceObj.onChangeDropdowns(Master, MasterID, UserID);
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

        public ActionResult ViewIndexedDocDetailsNew(int filegid)
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
                    // deptsModel1.DeptName = ds.Tables[1].Rows[0][3].ToString();
                    // deptsModel1.UnitName = ds.Tables[1].Rows[0][5].ToString();
                    deptsModel1.CateName = ds.Tables[1].Rows[0][3].ToString();
                    deptsModel1.SubCateName = ds.Tables[1].Rows[0][5].ToString();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    ViewBag.Orglvlval4 = ds.Tables[5].Rows[0][8].ToString();
                    ViewBag.Orglvlval3 = ds.Tables[5].Rows[0][7].ToString();
                    ViewBag.Orglvlval2 = ds.Tables[5].Rows[0][6].ToString();
                    ViewBag.Orglvlval1 = ds.Tables[5].Rows[0][5].ToString();
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
                        dt1 = ServiceObj.getmasterval(LovId, type);

                        List<SelectListItem> item8 = new List<SelectListItem>();
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            item8.Add(new SelectListItem
                            {
                                Text = dr1["master_name"].ToString(),
                                Value = dr1["master_code"].ToString()
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
                //int ctlval1= 0;

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

                return PartialView("ShowIndexedDocDetailsPartialVw", deptsModel1);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return PartialView("ShowIndexedDocDetailsPartialVw", deptsModel1);
            }
        }

        public ActionResult GetFileTaken(String Attribid)
        {
            Int64 Employeeid = Convert.ToInt64(Session["Emp_Id"]);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string Data1 = "";
            try
            {

                ds = ObjService.GetFileTaken(Employeeid, Attribid);
                dt = ds.Tables[0];
                Data1 = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return Json(Data1, JsonRequestBehavior.AllowGet);
        }

    }
}