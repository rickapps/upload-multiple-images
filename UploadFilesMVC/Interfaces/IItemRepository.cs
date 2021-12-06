using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Item CreateNewItem();
    }
}
