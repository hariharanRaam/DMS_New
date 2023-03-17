using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class ViewDocumentAttributes_Model
    {
        public int groupID_ { get; set; }
        public Int64 docnameId { get; set; }
        public Int64 docgrpId { get; set; }
        public Int64 unitId { get; set; }
        public Int64 depId { get; set; }

        public Int64 Atr_Id { get; set; }
        public String Atr_Name { get; set; }
        public String Atr_Type { get; set; }
        public String Atr_Length { get; set; }
        public String Atr_Mandotry { get; set; }
        public String Dgroup_Name { get; set; }
        public String Dname_Name { get; set; }
        public String Unit_Name { get; set; }
        public String Dept_Name { get; set; }
        public Int64 Dgroup_Id { get; set; }
        public Int64 Dname_Id { get; set; }
        public Int64 Unit_Id { get; set; }
        public Int64 Dept_Id { get; set; }
        public Int64 Attrib_orderId { get; set; }
        public Int64 Lov_Id { get; set; }
    }
}
