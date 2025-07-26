using Project.Game.Core;
using Project.Game.Shapes;

namespace Project.Game.Interfaces
{
    public interface IMatchChecker
    {
        bool CanMatch(DefaultShape shape, SortingSlot slot);
        MatchResult CheckMatch(DefaultShape shape, SortingSlot slot);
    }
}