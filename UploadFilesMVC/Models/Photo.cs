using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RickApps.UploadFilesMVC.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public int ItemID { get; set; }
        public int Sequence { get; set; }
        public string Location { get; set; }
    }
}
