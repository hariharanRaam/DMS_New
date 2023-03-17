using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;


namespace DMS.Service
{
    public class GetDeletedIndexedDocuments_Service
    {
        GetDeletedIndexedDocuments_Data dataobj = new GetDeletedIndexedDocuments_Data();

        public DataSet getdeletednidexedrecords()
        {
            return dataobj.Getdeletedindexeddocs();
        }
    }
}
