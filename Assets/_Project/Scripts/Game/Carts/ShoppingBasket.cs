using UnityEngine;

namespace Skoggy.LD45.Game.Carts
{
    public class ShoppingBasket : MonoBehaviour, IPickupable
    {
        public Rigidbody Rigidbody;
        public Collider Collider;

        public Vector3 Position => transform.position;

        public void Grab()
        {
            Destroy(Rigidbody);
            Collider.enabled = false;
        }

        public bool IsCart() => true;
        public bool IsProduct() => false;

        public void Release()
        {
            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Collider.enabled = true;
        }
    }
}