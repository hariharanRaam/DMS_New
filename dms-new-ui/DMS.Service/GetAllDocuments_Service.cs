using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Data;
using DMS.Model;
using System.Data;

namespace DMS.Service
{
    public class GetAllDocuments_Service
    {
        GetAllDocuments_Data Objdata = new GetAllDocuments_Data();

        public DataSet GetDocuments(Int64 empid, string activeflag, string docname, string docgroup, string submission, string status)
        {
            return Objdata.getdocuments(empid, activeflag,docname,docgroup , submission,status);
        }
    }
}
