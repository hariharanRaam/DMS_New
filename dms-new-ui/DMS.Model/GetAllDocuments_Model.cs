using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
   public class GetAllDocuments_Model
    {
       public int DocId { get; set; }
       public string DocName { get; set; }
       public string DocType { get; set; }
       public string DocPath { get; set; }
       public string DocCreatedby { get; set; }
       public string DocCreateddate { get; set; }
       public string View { get; set; }
    }
}
