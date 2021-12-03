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

        public DbSet<Item> Item { get; set; }
        public DbSet<Photo> Photo { get; set; }
    }
}
