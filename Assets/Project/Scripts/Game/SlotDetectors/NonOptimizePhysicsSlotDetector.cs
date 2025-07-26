using Project.Game.Core;
using Project.Game.Interfaces;
using UnityEngine;

namespace Project.Game.SlotDetectors
{
    public class NonOptimizePhysicsSlotDetector : MonoBehaviour, ISlotDetector
    {
        [SerializeField] private float _detectionRadius = 1f;
        
        public SortingSlot FindSlotAt(Vector3 position)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(position);

            for (var i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i];
                if (hit.gameObject == gameObject) continue;

                SortingSlot slot = hit.GetComponent<SortingSlot>();
                if (slot) return slot;
            }

            return null;
        }

        public bool IsInSlot(Vector3 position, SortingSlot slot)
        {
            if (!slot) return false;
            
            Collider2D slotCollider = slot.GetComponent<Collider2D>();
            if (slotCollider)
            {
                return slotCollider.bounds.Contains(position);
            }
            
            return Vector3.Distance(position, slot.transform.position) <= _detectionRadius;
        }
    }
}