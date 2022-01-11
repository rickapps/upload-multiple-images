namespace RickApps.UploadFilesMVC.ViewModels
{
    using RickApps.UploadFilesMVC.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RickApps.UploadFilesMVC.Data;
    using System.Collections.Generic;

    /// <summary>
    /// RickApps.com
    /// Copyright (C) 2017 Rick Eichhorn
    /// Created: October 28, 2017
    /// </summary>
    public class HomeListViewModel
    {
        public HomeListViewModel()
        {
            PageTitle = "Inventory";
        }
        /// <summary>
        /// Gets or sets the sorting combo box list.
        /// </summary>
        public IEnumerable<SelectListItem> Sorts { get; set; }

        public ItemSortKey SortBy { get; set; } 

        public IEnumerable<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the page label
        /// </summary>
        public string PageTitle { get; set; }

    }
}