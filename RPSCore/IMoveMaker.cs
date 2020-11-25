using Contracts;

namespace RPSCore
{
    public interface IMoveMaker
    {
        Move MakeMove(int dynamitesLeft);
    }
}