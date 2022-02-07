using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;

namespace RickApps.UploadFilesMVC.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Item CreateNewItem();
        Item GetItem(int id);

        IEnumerable<Item> GetAdminItems(ItemListingStatus status);

        IEnumerable<Item> GetInventoryItems(ItemSortKey sort);
    }
}
