using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RickApps.MVCWebsite.ViewModels
{
    public class AdminDetailViewModel
    {
        private Item _item;

        public AdminDetailViewModel(Item item)
        {
            _item = item;
            OrigStatus = _item.Status;
            ShowPhoto = false;
        }

        public IEnumerable<SelectListItem> ItemStatusList { get; set; }

        public ItemListingStatus OrigStatus { get; set; }

        // Indicates if the photo tab should be active
        public bool ShowPhoto { get; set; }

        public int ItemID
        {
            get
            {
                return (int)_item.ItemID;
            }
        }

        public int Number { get; set; }

        public decimal ItemPrice
        {
            get { return _item.Price ?? (decimal)0.00; }

            set { _item.Price = value; }
        }

        public string Description { get; set; }

        public ItemListingStatus ItemStatus { get; set; }

        public IEnumerable<Photo> ItemImages
        {
            get { return _item.Photos; }
        }
    }
}