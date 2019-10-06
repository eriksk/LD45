
using Skoggy.LD45.Game.Carts;
using Skoggy.LD45.Game.Players;
using UnityEngine;

namespace Skoggy.LD45.Game.DropOff
{
    public class CheckoutZone : MonoBehaviour
    {
        public Material Green, Red, Blue;
        public MeshRenderer Renderer;
        public DropOffZone DropOffZone;
        private bool _inside;

        void Update()
        {
            if(!_inside) return;

            if(DropOffZone.ContainsBasket() && Input.GetButtonDown("Jump"))
            {
                ObjectLocator.GameManager.Checkout(DropOffZone.Basket);
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            OnTriggerStay(collider);
        }

        public void OnTriggerStay(Collider collider)
        {
            var player = collider.gameObject.GetComponent<Player>();
            if(player == null) return;

            _inside = true;

            if(DropOffZone.ContainsBasket())
            {
                Renderer.sharedMaterial = Green;
                return;
            }

            Renderer.sharedMaterial = Red;
            return;
        }

        public void OnTriggerExit(Collider collider)
        {
            var player = collider.gameObject.GetComponent<Player>();
            if(player == null) return;
            
            Renderer.sharedMaterial = Blue;
            _inside = false;
        }
    }
}