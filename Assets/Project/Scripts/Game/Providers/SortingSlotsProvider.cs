using System.Collections.Generic;
using System.Linq;
using Project.Game.Core;
using Project.Game.Interfaces;
using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.Providers
{
    public class SortingSlotsProvider : MonoBehaviour, ISortingSlotsProvider
    {
        [Header("Sorting Slots")]
        [SerializeField] private List<SortingSlot> _sortingSlots = new List<SortingSlot>();
        
        public List<ShapeData> GetAvailableShapeData()
        {
            return _sortingSlots
                  .Where(slot => slot && slot.Data)
                  .Select(slot => slot.Data as ShapeData)
                  .Where(data => data)
                  .ToList();
        }
    }
}