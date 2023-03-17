using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;
using DMS.Model;

namespace DMS.Service
{
    public class PhysicalArchival_Service
    {
        PhysicalArchival_Data dataObj = new PhysicalArchival_Data();
        public DataSet GetIndexedDocuments(int? Dgroup1, int? Dname1, string Dbcon, string type, string activeflag, string from_date, string to_date)
        {
            return dataObj.GetIndexedDocuments(Dgroup1, Dname1, Dbcon, type, activeflag,from_date,to_date);
        }

        public DataSet GetIndexedRetrivals(int? Dgroup1, int? Dname1, string attrib_ids, string grid_mode)
        {
            return dataObj.GetIndexedRetrivals(Dgroup1, Dname1, attrib_ids,grid_mode);
        }

        public DataSet Initialvalues(int? docgroup, int? docname, string Aaction, int? attribgid)
        {
            return dataObj.Initialvalues(docgroup, docname, Aaction, attribgid);
        }

        public int SaveStorageAttributes(PhysicalArchival_Model modelObj, List<PhysicalArchival_Model> ModelObjList)
        {
            int Result;
            try
            {
                Result = dataObj.SaveStorageAttributes(modelObj, ModelObjList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public DataTable checkvalidone(int? GridID)
        {
            return dataObj.checkvalidone(GridID);
        }

        public DataSet getattributes(PhysicalArchival_Model ModelObj)
        {
            return dataObj.getattributes(ModelObj);
        }
        public string validateLOV(int dtlov, string userdata)
        {
            return dataObj.validateLOV(dtlov, userdata);
        }

        public DataTable SaveStorageAttribute(DataTable dt, PhysicalArchival_Model ModelObj)
        {
            DataTable Resultdt = new DataTable();
            string itsattribute = "Y";
            try
            {

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    itsattribute = "Y";

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        if (itsattribute == "Y")
                        {
                            if ("FILE NAME" == dt.Columns[i].ToString().ToUpper())
                            {
                                itsattribute = "N";
                                ModelObj.FileName = dt.Rows[j][i].ToString();
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    ModelObj.Attributescol = "'" + dt.Columns[i].ToString().ToUpper() + "'";
                                    ModelObj.Attributesval = "'" + dt.Rows[j][i].ToString() + "'";
                                }
                                else
                                {
                                    ModelObj.Attributescol = ModelObj.Attributescol + "$^`" + "'" + dt.Columns[i].ToString().ToUpper() + "'";
                                    ModelObj.Attributesval = ModelObj.Attributesval + "$^`" + "'" + dt.Rows[j][i].ToString() + "'";
                                }
                            }
                        }
                        else if (dt.Columns[i].ToString().ToUpper() != "FILE NAME")
                        {
                            if (dt.Columns[i - 1].ToString().ToUpper() == "FILE NAME")
                            {
                                ModelObj.StorageAttribcol = "'" + dt.Columns[i].ToString().ToUpper() + "'";
                                ModelObj.StorageAttribval = "'" + dt.Rows[j][i].ToString() + "'";
                            }
                            else
                            {
                                ModelObj.StorageAttribcol = ModelObj.StorageAttribcol + "$^`" + "'" + dt.Columns[i].ToString().ToUpper() + "'";
                                ModelObj.StorageAttribval = ModelObj.StorageAttribval + "$^`" + "'" + dt.Rows[j][i].ToString() + "'";
                            }
                        }
                    }

                    //Resultdt = dataObj.SaveStorageAttribute(ModelObj);
                    Resultdt = dataObj.SaveStorageAttributeNew(ModelObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Resultdt;
        }

    }
}
