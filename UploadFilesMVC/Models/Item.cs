using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace RickApps.UploadFilesMVC.Models
{
    public enum ItemListingStatus
    {
        Draft,
        Active,
        Sold
    }
    public class Item
    {
        public Item()
        {
            Photos = new Collection<Photo>();
        }
        public int ID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1.00, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal? Price { get; set; }
        public ItemListingStatus Status { get; set; }
        public string FormattedPrice
        {
            get { return String.Format("{0:C}", Price); }
        }
        public ICollection<Photo> Photos { get; set; }
    }

}
