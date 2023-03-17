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
    public class ViewAttributeLOV_Service
    {
        ViewAttributeLOV_Data dataObj = new ViewAttributeLOV_Data();
        public DataSet GetListView()
        {
            return dataObj.GetListView();
        }

        public DataTable getlovvalues(int? lovid)
        {
            return dataObj.getlovvalues(lovid);
        }

        public int Updatelov(ViewAttributeLOV_Model modelObj, List<ViewAttributeLOV_Model> lstobj)
        {
            return dataObj.Updatelov(modelObj, lstobj);
        }
    }
}
