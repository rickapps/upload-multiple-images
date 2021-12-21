using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.MVCWebsite.ViewModels;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using RickApps.UploadFilesMVC.ViewModels;
using System;

namespace RickApps.UploadFilesMVC.Controllers
{
    public class AdminController : BaseController
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        public AdminController(IUnitOfWork uow) : base(uow)
        {
        }
        #endregion

        /// <summary>
        /// Display our default view. I don't decorate the method with [HttpPost] as I don't 
        /// need to have two Index methods.
        /// </summary>
        /// <param name="ItemStatus"></param>
        /// <returns></returns>
        public ActionResult Index(ItemListingStatus Status = ItemListingStatus.Active)
        {
            AdminIndexViewModel vm = new AdminIndexViewModel();
            // Let's see how complicated we can make it just to populate a simple drop down.
            // But seriously, you really might want to obtain all your data from a repository for larger projects.
            vm.StatusList = new SelectList(((ItemRepository)_repository.Items).ItemStatusList, "Key", "Value", Status);
            vm.Status = Status;
            // Now we get all the items that have the specified status. Note that EF core does not do lazy loading
            // by default. This means we obtain a collection of items without their photos. You can change this in
            // the repository call to GetAdminItems by adding .include(p => p.Photos) to the query string. Alternatively, 
            // you could activate ILazyLoader service globally.
            vm.Items = ((ItemRepository)_repository.Items).GetAdminItems(Status);
            return View(vm);
        }

        /// <summary>
        /// Display the item we wish to edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int itemID, bool isPhoto = false)
        {
            // Get the item along with its photo collection.
            Item item = ((ItemRepository)_repository.Items).GetItem(itemID);
            if (item == null)
            {
                item = new Item();
            }
            AdminDetailViewModel vm = new AdminDetailViewModel(item);
            vm.ItemStatusList = new SelectList(((ItemRepository)_repository.Items).ItemStatusList, "Key", "Value", item.Status);
            vm.ShowPhoto = isPhoto;
            return View("Detail", vm);
        }

        /// <summary>
        /// Save the changes to the item and return to the index screen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int itemID, IFormCollection collection)
        {
            Item item = null;
            ItemListingStatus initStatus;
            ItemListingStatus newStatus;
            try
            {
                if (ModelState.IsValid)
                {
                    item = ((ItemRepository)_repository.Items).GetItem(itemID);
                    initStatus = item.Status;
                    item.Name = collection["Name"];
                    item.Description = collection["Description"];
                    item.Price = decimal.Parse(collection["ItemPrice"]);
                    if (Enum.TryParse<ItemListingStatus>(collection["Status"], out newStatus))
                        item.Status = newStatus;

                    _repository.Complete();
                    TempData["message"] = string.Format("Item {0} has been updated", item.Number);
                    return RedirectToAction("Index", new { ItemStatus = initStatus });
                }
            }
            catch (Exception ex)
            {
            }
            return View("Detail", new AdminDetailViewModel(item));
        }

        public ActionResult Create()
        {
            Item item;
            try
            {
                item = ((ItemRepository)_repository.Items).CreateNewItem();
                item.Name = "New Item";
                item.Status = ItemListingStatus.Draft;
                _repository.Items.Add(item);
                _repository.Complete();
                TempData["message"] = string.Format("New item {0} has been added", item.Number);
            }
            catch (Exception ex)
            {
                TempData["message"] = string.Format("Could not add new item.");
            }
            return RedirectToAction("Index", new { ItemStatus = ItemListingStatus.Draft });
        }

        [HttpPost]
        public ActionResult ArchiveItem(int itemID, FormCollection collection)
        {
            Item item = null;
            ItemListingStatus initStatus = ItemListingStatus.Active;
            try
            {
                item = ((ItemRepository)_repository.Items).GetItem(itemID);
                initStatus = item.Status;
                item.Status = ItemListingStatus.Sold;
                _repository.Complete();
                TempData["message"] = string.Format("Item {0} is hidden from customers", item.Number);
                return RedirectToAction("Index", new { ItemStatus = initStatus });
            }
            catch (Exception ex)
            {
            }
            return View("Detail", new AdminDetailViewModel(item));
        }

        /// <summary>
        /// Delete the item and return to the index screen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int itemID, IFormCollection collection)
        {
            ItemListingStatus initStatus = ItemListingStatus.Active;
            try
            {
                Item item = ((ItemRepository)_repository.Items).GetItem(itemID);
                int Number = item.Number;
                initStatus = item.Status;
                _repository.Items.Remove(item);
                _repository.Complete();
                TempData["message"] = string.Format("Item {0} has been deleted", Number.ToString());
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index", new { ItemStatus = initStatus });
        }
    }
}
