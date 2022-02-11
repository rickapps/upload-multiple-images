using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using RickApps.UploadFilesMVC.ViewModels;
using System.Diagnostics;

namespace RickApps.UploadFilesMVC.Controllers
{
    /// <summary>
    /// User can browse items in the database but not make changes.
    /// Admin controller allows database updates.
    /// </summary>
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow) : base(uow)
        {
        }

        /// <summary>
        /// Tell us about the website
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// List all active items using the designated sort order
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public IActionResult List(ItemSortKey sortBy = ItemSortKey.New)
        {
            HomeListViewModel vm = new HomeListViewModel();
            // Let's see how complicated we can make it just to populate a simple drop down.
            // But seriously, you really might want to obtain all your data from a repository for larger projects.
            vm.Sorts = new SelectList(((ItemRepository)_repository.Items).ItemSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;
            // Retrieve all the active items along with their photos. Due to lazy loading we have to explicitly
            // ask for the photos in the method GetInventoryItems
            vm.Items = ((ItemRepository)_repository.Items).GetInventoryItems(sortBy);
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Introductory Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display a specific item record. SortKey is used to preserve
        /// our sort order when we return to the list page.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public IActionResult Detail(int? id, ItemSortKey sortBy)
        {
            Item item = null;
            HomeDetailViewModel vm = null;
            if (id.HasValue)
            {
                // Get item with photos
                item = _repository.Items.GetItem(id.Value);
                vm = new HomeDetailViewModel(item);
            }
            if (item == null)
            {
                return NotFound();
            }

            return View(vm);
        }
    }
}
