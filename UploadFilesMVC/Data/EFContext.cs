using Microsoft.EntityFrameworkCore;
using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC.Data
{
    public class EFContext : DbContext
    {
        public EFContext (DbContextOptions<EFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
    }
}
