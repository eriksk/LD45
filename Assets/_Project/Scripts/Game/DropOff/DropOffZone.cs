
using Skoggy.LD45.Game.Carts;
using UnityEngine;

namespace Skoggy.LD45.Game.DropOff
{
    public class DropOffZone : MonoBehaviour
    {
        public Material Green, Red, Blue;
        public MeshRenderer Renderer;

        private float _timeout;

        void Update()
        {
            _timeout -= Time.deltaTime;

            if(_timeout <= 0f)
            {
                Renderer.sharedMaterial = Blue;
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            OnTriggerStay(collider);
        }

        public void OnTriggerStay(Collider collider)
        {
            var basket = collider.gameObject.GetComponent<ShoppingBasket>();
            if(basket == null) return;

            _timeout = 0.3f;

            var basketMatchesShoppingList = ObjectLocator.GameManager.EverythingOnShoppingListIsInBasket(basket);
            if(!basketMatchesShoppingList)
            {
                Renderer.sharedMaterial = Red;
                return;
            }

            Renderer.sharedMaterial = Green;
        }

        public void OnTriggerExit(Collider collider)
        {
            var basket = collider.gameObject.GetComponent<ShoppingBasket>();
            if(basket == null) return;
            
            Renderer.sharedMaterial = Blue;
        }
    }
}