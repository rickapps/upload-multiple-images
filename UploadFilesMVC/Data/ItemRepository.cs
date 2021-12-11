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
            ItemStatusList = new Dictionary<ItemListingStatus, string>
            {
                {ItemListingStatus.Active, "Active Store Items" },
                {ItemListingStatus.Draft, "Draft Items Not Listed" },
                {ItemListingStatus.Sold, "Sold or Archived" }
            };
        }

        public Item CreateNewItem()
        {
            Item item = new Item();

            return item;
        }

        public IEnumerable<Item> GetAdminItems(ItemListingStatus status)
        {
            IQueryable<Item> itemList = Context.Set<Item>();
            itemList = itemList.Where(p => p.Status == status);

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
