using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;

namespace RickApps.UploadFilesMVC.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        IEnumerable<Photo> GetItemImages(int itemID);
    }
}
