using RickApps.UploadFilesMVC.Interfaces;

namespace RickApps.UploadFilesMVC.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFContext _context;

        public UnitOfWork(EFContext context)
        {
            _context = context;
           Items = new ItemRepository(_context);
           ItemPhotos = new PhotoRepository(_context);
        }

        public IItemRepository Items { get; private set; }

        public IPhotoRepository ItemPhotos { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
