using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;

namespace DMS.Service
{
    public class ApproveEditedIndexedDoc_Service
    {
        ApproveEditedIndexedDoc_Data DataObj = new ApproveEditedIndexedDoc_Data();
        public DataSet InitValues(int? Id, string Mode)
        {
            return DataObj.Initiatecontrols(Id, Mode);
        }

        public Int32 Approvetoupdatefile(int? id, string Mode)
        {
            return DataObj.approvetoupdatefile(id, Mode);
        }

        public Int32 Reject(int? id, string Mode)
        {
            return DataObj.Reject(id, Mode);
        }

    }
}
