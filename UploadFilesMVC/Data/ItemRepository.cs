using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC.Data
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(EFContext context) : base(context)
        {
        }

        public Item CreateNewItem()
        {
            Item item = new Item();

            return item;
        }
    }
}
