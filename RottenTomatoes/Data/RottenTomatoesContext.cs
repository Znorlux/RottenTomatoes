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
    }
}
