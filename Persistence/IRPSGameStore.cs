using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence
{
    public interface IRPSGameStore
    {
        Task Add(RPSGame game);

        Task Update(RPSGame game);

        Task<RPSGame> GetGame();
    }
}