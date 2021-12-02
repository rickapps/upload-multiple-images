using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RickApps.UploadFilesMVC.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
    
}
