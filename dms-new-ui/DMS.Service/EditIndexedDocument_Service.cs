using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;

namespace DMS.Service
{
    public class EditIndexedDocument_Service
    {
        EditIndexedDocument_Data dataobj = new EditIndexedDocument_Data();
        public DataSet Getindexedrecords()
        {
            return dataobj.getindexedrecodrs();
        }
        public DataSet Getindexedrecordsforapproval()
        {
            return dataobj.Getindexedrecordsforapproval();
        }
      
    }
}
