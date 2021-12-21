using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;

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
            foreach (var image in _item.Photos)
            {
                continue;
            }
        }

        public IEnumerable<SelectListItem> ItemStatusList { get; set; }

        public ItemListingStatus OrigStatus { get; set; }

        // Indicates if the photo tab should be active
        public bool ShowPhoto { get; set; }

        public int ItemID
        {
            get
            {
                return (int)_item.ID;
            }
        }

        public int Number 
        { 
            get { return _item.Number; } 
            set { _item.Number = value; } 
        }

        public string Name
        {
            get { return _item.Name; }
            set { _item.Name = value; }
        }

        public decimal ItemPrice
        {
            get { return _item.Price ?? (decimal)0.00; }

            set { _item.Price = value; }
        }

        public string Description 
        {
            get { return _item.Description; } 
            set { _item.Description = value; }
        }

        public ItemListingStatus Status 
        { 
            get { return _item.Status; } 
            set { _item.Status = value; } 
        }

        public IEnumerable<Photo> ItemImages
        {
            get { return _item.Photos; }
        }
    }
}