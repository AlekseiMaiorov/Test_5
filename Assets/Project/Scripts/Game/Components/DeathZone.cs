using Project.Game.Interfaces;
using UnityEngine;

namespace Project.Game.Components
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDeathZoneHandler>(out var handler))
            {
                handler.OnReachedDeathZone();
            }
        }
    }
}