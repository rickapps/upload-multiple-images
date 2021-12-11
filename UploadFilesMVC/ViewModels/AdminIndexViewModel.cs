using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;

namespace RickApps.UploadFilesMVC.ViewModels
{
    public class AdminIndexViewModel
    {
        public AdminIndexViewModel()
        {
            ItemStatus = ItemListingStatus.Active;
        }
        /// <summary>
        /// Gets or sets the sorting combo box list.
        /// </summary>

        public IEnumerable<SelectListItem> StatusList { get; set; }
        /// <summary>
        /// Gets or sets our list of items to display
        /// </summary>
        public IEnumerable<Item> Items { get; set; }

        /// <summary>
        /// The current display - archived or active listings
        /// </summary>
        public ItemListingStatus ItemStatus { get; set; }
    }
}