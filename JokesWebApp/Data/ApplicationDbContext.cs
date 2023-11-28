using JokesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<account> Accounts { get; set; }
    }
}
