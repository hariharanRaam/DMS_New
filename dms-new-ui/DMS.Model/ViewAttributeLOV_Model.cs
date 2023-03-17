using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class ViewAttributeLOV_Model
    {
        public string lovtext { get; set; }
        public ViewAttributeLOV_Model()
        {
            dept = new List<ViewAttributeLOV_Model>();
        }
        public List<ViewAttributeLOV_Model> dept { get; set; }
        public int slno { get; set; }
        public int masterid { get; set; }
        public int UserId { get; set; }
    }
}
