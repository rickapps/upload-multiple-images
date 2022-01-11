using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using RickApps.UploadFilesMVC.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public IActionResult List(ItemSortKey sortBy = ItemSortKey.New)
        {
            HomeListViewModel vm = new HomeListViewModel();
            // Let's see how complicated we can make it just to populate a simple drop down.
            // But seriously, you really might want to obtain all your data from a repository for larger projects.
            vm.Sorts = new SelectList(((ItemRepository)_repository.Items).ItemSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;
            // Now we get all the items that have the specified status. Note that EF core does not do lazy loading
            // by default. This means we obtain a collection of items without their photos. You can change this in
            // the repository call to GetAdminItems by adding .include(p => p.Photos) to the query string. Alternatively, 
            // you could activate ILazyLoader service globally.
            vm.Items = ((ItemRepository)_repository.Items).GetAdminItems(ItemListingStatus.Active);
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Items
        public IActionResult Index()
        {
            return View();
        }

        // GET: Items/Details/5
        public IActionResult Details(int? id)
        {
            Item item = null;
            if (id.HasValue)
            {
                item = _repository.Items.Get(id.Value);
            }
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
