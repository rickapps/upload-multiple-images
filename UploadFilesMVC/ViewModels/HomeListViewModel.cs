namespace RickApps.UploadFilesMVC.ViewModels
{
    using RickApps.UploadFilesMVC.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RickApps.UploadFilesMVC.Data;
    using System.Collections.Generic;

    public class HomeListViewModel
    {
        public HomeListViewModel()
        {
            PageTitle = "Item List";
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
        public string PageTitle { get; private set; }

    }
}