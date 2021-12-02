using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC.Data
{
    public class RickAppsUploadFilesMVCContext : DbContext
    {
        public RickAppsUploadFilesMVCContext (DbContextOptions<RickAppsUploadFilesMVCContext> options)
            : base(options)
        {
        }

        public DbSet<RickApps.UploadFilesMVC.Models.Item> Item { get; set; }
    }
}
