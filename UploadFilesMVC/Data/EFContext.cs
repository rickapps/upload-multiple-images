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
        /// <summary>
        /// The two DvSet properties below are not really used other than to tell
        /// EF what we want our tables named. If you deleted them, all would
        /// work the same.
        /// </summary>
        public DbSet<Item> Item { get; set; }
        public DbSet<Photo> Photo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Just make it clear we want a one to many  
            modelBuilder.Entity<Photo>()
                .HasOne<Item>(s => s.Item)
                .WithMany(g => g.Photos)
                .HasForeignKey(s => s.ItemID);
        }
    }
}
