using Project.Game.Core;
using UnityEngine;

namespace Project.Game.Interfaces
{
    public interface ISlotDetector
    {
        SortingSlot FindSlotAt(Vector3 position);
        bool IsInSlot(Vector3 position, SortingSlot slot);
    }
}