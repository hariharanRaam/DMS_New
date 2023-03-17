using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DMS.Data;

namespace DMS.Service
{
    public class ViewStorageAttributes_Service
    {
        ViewStorageAttributes_Data dataObj = new ViewStorageAttributes_Data();
        public DataSet GetDynamicStorageAttributes()
        {
            return dataObj.GetDynamicStorageAttributes();
        }

        public DataTable GetStorageAttributeValues(int GroupAtrID)
        {
            return dataObj.GetStorageAttributeValues(GroupAtrID);
        }

        //docname,docgroup,unit,dep
        public DataTable GetStorageAttributeValuesEdit(int GroupAtrID)
        {
            return dataObj.GetStorageAttributeValuesEdit(GroupAtrID);
        }

    }
}
