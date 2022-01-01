using Microsoft.EntityFrameworkCore;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RickApps.UploadFilesMVC.Data
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(EFContext context) : base(context)
        {
            // Create our item status list for the drop downs.
            ItemStatusList = new Dictionary<ItemListingStatus, string>
            {
                {ItemListingStatus.Active, "Active Store Items" },
                {ItemListingStatus.Draft, "Draft Items Not Listed" },
                {ItemListingStatus.Sold, "Sold or Archived" }
            };
        }

        /// <summary>
        /// Create a new item and populate fields with default values.
        /// </summary>
        /// <returns></returns>
        public Item CreateNewItem()
        {
            Item item = new Item();
            // Figure out the next item number. This works provided we
            // don't have too many simultaneous users creating items.
            item.Number = _entity.Max(p => p.Number) + 1;
            item.Status = ItemListingStatus.Draft;
            item.Name = "New Item";
            item.Price = 0;
            return item;
        }

        /// <summary>
        /// This is different from Get() in the base class because
        /// it also obtains the photo collection for the specified
        /// item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItem(int id)
        {
            var item = _entity.Single(i => i.ID == id);
            // Populate the photo collection. Lazy loading is not
            // turned on so we have to do it explicitly. When you
            // read up on eager, lazy, and explicit loading, make
            // sure you are reading about EF Core, not just EF.
            Context.Entry(item).Collection(p => p.Photos).Load();
            return item;
        }

        public IEnumerable<Item> GetAdminItems(ItemListingStatus status)
        {
            var itemList = _entity.Where(p => p.Status == status);

            return itemList.OrderByDescending(p => p.Number);
        }

        

        /// <summary>
        /// Use to populate dropdown
        /// </summary>
        public Dictionary<ItemListingStatus, string> ItemStatusList
        {
            get;
            private set;
        }


    }
}
