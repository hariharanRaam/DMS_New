using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
    public class DocArchival_Model
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public int UserId { get; set; }

        public int DeptId { get; set; }
        public int UnitId { get; set; }
        public int CatgId { get; set; }
        public int SubCatgId { get; set; }

        public Int64? FileID { get; set; }

        public string activeflag { get; set; }
        public string FileSize { get; set; }

       // public string ActionMode { get; set; }

        public class DocArchivalFileCache {
            public string Fileno { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string FileSize { get; set; }
            public string FileIsExists { get; set; }
            public int totalnooffiles { get; set; }
            public string totalnoofsizeinnumbers { get; set; }
            public string totalnoofsize { get; set; }

        }
    }
}
