using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public class RpsGameStore : IRPSGameStore
    {
        private readonly RPSContext context;

        public RpsGameStore(RPSContext context)
        {
            this.context = context;
        }

        public async Task Add(RPSGame game)
        {
            await context.RPSGames.AddAsync(game);
            await context.SaveChangesAsync();
        }

        public async Task Update(RPSGame game)
        {
            context.RPSGames.Update(game);
            await context.SaveChangesAsync();
        }

        public async Task<RPSGame> GetGame()
        {
            return await context.RPSGames.Include("Games").AsNoTracking().FirstOrDefaultAsync(g => g.OverallOutcome == null);
        }
    }
}