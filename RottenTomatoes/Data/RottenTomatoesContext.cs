using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RottenTomatoes.Models;

namespace RottenTomatoes.Data
{
    public class RottenTomatoesContext : DbContext
    {
        public RottenTomatoesContext (DbContextOptions<RottenTomatoesContext> options)
            : base(options)
        {
        }

        public DbSet<RottenTomatoes.Models.User> User { get; set; } = default!;

        public DbSet<RottenTomatoes.Models.Movie> Movie { get; set; } = default!;

        public DbSet<RottenTomatoes.Models.Serie> Serie { get; set; } = default!;

        public DbSet<RottenTomatoes.Models.Top10> Top10 { get; set; } = default!;

        public DbSet<RottenTomatoes.Models.FavoriteMovie> FavoriteMovie { get; set; } = default!;

        public DbSet<RottenTomatoes.Models.FavoriteSerie> FavoriteSerie { get; set; } = default!;
    }
}
