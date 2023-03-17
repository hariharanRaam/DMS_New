using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DMS.Model
{
    public class LOVMaster_Model
    {
        public string LovName { get; set; }
        public int excelId { get; set; }
        public string excelName { get; set; }
        public string path { get; set; }
    }
}
