using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RickApps.UploadFilesMVC.Models
{
    public class Photo
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int Sequence { get; set; }
        public string LinkToLargeImage { get; set; }
        public string LinkToMediumImage { get; set; }
        public string LinkToSmallImage { get; set; }
        public Item Item { get; set; }
    }
}
