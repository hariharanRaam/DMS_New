using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Model;
using DMS.Data;

namespace DMS.Service
{
    public class BasicReport_Service
    {
        BasicReport_Data dataObj = new BasicReport_Data();
        DataTable dt = new DataTable();
        public List<BasicReport_Model> GetBasicReportDetails(string Master, Int64 MasterID, Int64 UserID)
        {
            try
            {
                List<BasicReport_Model> lst_ = new List<BasicReport_Model>();
                DataSet ds = new DataSet();
                ds = dataObj.GetBasicReportDetails(Master, MasterID, UserID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Master == "Dept")
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.DeptID = Convert.ToInt64(row["Dept_Id"].ToString());
                            modelObj_.DeptName = row["Dept_Name"].ToString();
                            lst_.Add(modelObj_);
                        }
                    }
                    if (Master == "Unit")
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.UnitID = Convert.ToInt64(row["unit_id"].ToString());
                            modelObj_.UnitName = row["unit_name"].ToString();
                            lst_.Add(modelObj_);
                        }
                    }
                    if (Master == "DocGroup")
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.CateID = Convert.ToInt64(row["Dgroup_Id"].ToString());
                            modelObj_.CateName = row["Dgroup_Name"].ToString();
                            lst_.Add(modelObj_);
                        }
                    }
                    if (Master == "DocName")
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            BasicReport_Model modelObj_ = new BasicReport_Model();
                            modelObj_.SubCateID = Convert.ToInt64(row["Dname_Id"].ToString());
                            modelObj_.SubCateName = row["Dname_Name"].ToString();
                            lst_.Add(modelObj_);
                        }
                    }
                }
                return lst_;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet onChangeDropdowns(string Master, Int64 MasterID, Int64 UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = dataObj.GetBasicReportDetails(Master, MasterID, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetUploadedFiles(BasicReport_Model modelObj)
        {
          //  List<BasicReport_Model> listobj = new List<BasicReport_Model>();
            try
            {
                return dataObj.GetUploadedFiles(modelObj);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return listobj;
        }

        public List<BasicReport_Model> GetAttributes(BasicReport_Model modelObj)
        {
            List<BasicReport_Model> listobj = new List<BasicReport_Model>();
            try
            {
                dt = dataObj.GetAttributes(modelObj);
                if (modelObj.inAction == "Attribute")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        BasicReport_Model obj = new BasicReport_Model();
                        obj.AttribID = Convert.ToInt64(row["Atr_Id"].ToString());
                        obj.AttribName = row["Atr_Name"].ToString();
                        listobj.Add(obj);
                    }
                }
                else if (modelObj.inAction == "LOV")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        BasicReport_Model obj = new BasicReport_Model();
                        obj.LovName = row["Lovexl_Name"].ToString();
                        listobj.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listobj;
        }

        public DataSet GetIndexedFileDetails(BasicReport_Model _modelObj)
        {
            try
            {
                return dataObj.GetIndexedFileDetails(_modelObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIndexedDocumentDetails(BasicReport_Model _modelObj)
        {
            try
            {
                return dataObj.GetIndexedDocumentDetails(_modelObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
