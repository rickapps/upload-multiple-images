using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace RickApps.UploadFilesMVC.Data
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(EFContext context) : base(context)
        {

        }

        public IEnumerable<Photo> GetItemImages(int itemID)
        {
            IQueryable<Photo> imageList = Context.Set<Photo>();
            imageList = imageList.Where(p => p.ItemID == itemID);
            return imageList.OrderBy(p => p.Sequence);
        }

        public EFContext EFContext
        {
            get { return Context as EFContext; }
        }
    }
}
