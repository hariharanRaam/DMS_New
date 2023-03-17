using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Data;
using DMS.Model;
using DMS.Data;
using System.Web;

namespace DMS.Service
{
    public class DocArchivalBulkUpload_Service
    {
        DocArchivalBulkUpload_Data dataObj = new DocArchivalBulkUpload_Data();

        public DataTable getattributes(DocArchival_Model ModelObj)
        {
            return dataObj.getattributes(ModelObj);
        }

        //public DataTable savebulkupload(HttpPostedFileBase flexcelfile, HttpPostedFileBase[] flbulkfiles, DocArchival_Model ModelObj, DataTable dtexceldata)
        //{
        //    return dataObj.savebulkupload(flexcelfile, flbulkfiles, ModelObj, dtexceldata);
        //}

        public int SaveSingleFile(DocArchival_Model ModelObj, HttpPostedFileBase file)
        {
            int Result;
            try
            {
                Result = dataObj.SaveSingleDocDetails(ModelObj, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public int saveexclefile(HttpPostedFileBase flexcelfile, int userid)
        {
            return dataObj.saveexclefile(flexcelfile, userid);
        }

        public string validateLOV(int dtlov, string userdata)
        {
            return dataObj.validateLOV(dtlov, userdata);
        }

        public string validateattribute(DocArchival_Model ModelObj, string _wherecondition)
        {
            return dataObj.validateattribute(ModelObj, _wherecondition);
        }
        public int getfilenamecheck(DocArchival_Model ModelObj, string _filename) {
            return dataObj.getfilenamecheck_data(ModelObj, _filename);
        }
    }
}
