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
    
    public class OrgHierarchy_Service
    {
        OrgHierarchy_Data Orgmstdataobj = new OrgHierarchy_Data();

        //read the data
        public List<OrgHierarchy_Model> Orgmstdetail()
        {
            return Orgmstdataobj.Orgmstdetail();
        }
        public string SaveOrg(List<OrgHierarchy_Model> objModel,Int32 UserId )
        {
            return Orgmstdataobj.SaveOrgHierarchy( objModel,UserId);
        }
    }
}
