using Project.Game.Core;
using Project.Game.Interfaces;
using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.MatchCheckers
{
    public class StandardMatchChecker : MonoBehaviour, IMatchChecker
    {
        public bool CanMatch(DefaultShape shape, SortingSlot slot)
        {
            if (!shape ||
                !slot ||
                !shape.Data ||
                !slot.Data)
                return false;

            return shape.Data.Id == slot.Data.Id;
        }

        public MatchResult CheckMatch(DefaultShape shape, SortingSlot slot)
        {
            bool isMatch = CanMatch(shape, slot);

            return new MatchResult
            {
                IsMatch = isMatch,
                ScoreValue = isMatch ? shape.Data.ScoreValue : 0,
                HealthPenalty = isMatch ? 0 : shape.Data.HealthPenalty,
                EffectId = isMatch ? shape.Data.MatchEffectId : shape.Data.ExplosionEffectId,
                SoundId = isMatch ? shape.Data.MatchSoundId : shape.Data.ExplosionSoundId
            };
        }
    }
}