using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;
using System;

namespace RickApps.UploadFilesMVC.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Item CreateNewItem();
    }
}
