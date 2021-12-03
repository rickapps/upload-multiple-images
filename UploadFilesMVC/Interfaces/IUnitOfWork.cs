namespace RickApps.UploadFilesMVC.Interfaces
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }
        IPhotoRepository ItemPhotos { get; }
        int Complete();
    }
}
