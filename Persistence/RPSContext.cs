using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class RPSContext : DbContext
    {
        public RPSContext(DbContextOptions<RPSContext> options) : base(options)
        {
            
        }

        public DbSet<RPSGame> RPSGames { get; set; }
    }
}