﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using RickApps.UploadFilesMVC.ViewModels;
using System;

namespace RickApps.UploadFilesMVC.Controllers
{
    /// <summary>
    /// Controller for administrative functions. Enable authorization on this controller to restrict who can modify website content.
    /// </summary>
    [Authorize]
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
        /// Display our default view. 
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
            // by default. This means we obtain a collection of items without their photos. You can change this behavior in
            // the repository call to GetAdminItems by adding .include(p => p.Photos) to the query string. Alternatively, 
            // you could activate ILazyLoader service globally.
            vm.Items = ((ItemRepository)_repository.Items).GetAdminItems(Status);
            return View(vm);
        }

        /// <summary>
        /// Display the item we wish to edit
        /// </summary>
        /// <param name="ID">Item id</param>
        /// <param name="isPhoto">If true we make photo tab active</param>
        /// <returns></returns>
        public ActionResult Edit(int ID, bool isPhoto = false)
        {
            // Get the item along with its photo collection.
            Item item = ((ItemRepository)_repository.Items).GetItem(ID);
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
        [ValidateAntiForgeryToken]
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
                    return RedirectToAction("Index", new { Status = initStatus });
                }
            }
            catch (Exception ex)
            {
            }
            return View("Detail", new AdminDetailViewModel(item));
        }

        /// <summary>
        /// Create a new item and store it to the database.
        /// </summary>
        /// <returns></returns>
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
            return RedirectToAction("Index", new { Status = ItemListingStatus.Draft });
        }

        /// <summary>
        /// Archived items are not displayed to the user, but remain in our database.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveItem(int itemID)
        {
            Item item = null;
            ItemListingStatus initStatus;
            try
            {
                item = ((ItemRepository)_repository.Items).GetItem(itemID);
                initStatus = item.Status;
                item.Status = ItemListingStatus.Sold;
                _repository.Complete();
                TempData["message"] = string.Format("Item {0} is hidden from customers", item.Number);
                return RedirectToAction("Index", new { Status = initStatus });
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
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int itemID)
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
            return RedirectToAction("Index", new { Status = initStatus });
        }
    }
}
