
using Skoggy.LD45.Game.Players;
using UnityEngine;

namespace Skoggy.LD45.Game.DropOff
{
    public class DropOffZone : MonoBehaviour
    {
        public Material Green, Red, Blue;
        public MeshRenderer Renderer;

        public void OnTriggerEnter(Collider collider)
        {
            OnTriggerStay(collider);
        }

        public void OnTriggerStay(Collider collider)
        {
            var player = collider.gameObject.GetComponent<Player>();
            if(player == null) return;

            if(!player.CarryingBasked)
            {
                Renderer.sharedMaterial = Red;
                return;
            }

            // TODO: 
            var basketMatchesShoppingList = false;
            if(!basketMatchesShoppingList)
            {
                Renderer.sharedMaterial = Red;
                return;
            }

            Renderer.sharedMaterial = Green;
            // TODO: Consume basket and move on ??
        }

        public void OnTriggerExit(Collider collider)
        {
            var player = collider.gameObject.GetComponent<Player>();
            if(player == null) return;
            
            Renderer.sharedMaterial = Blue;
        }
    }
}