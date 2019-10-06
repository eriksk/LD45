using System.Collections.Generic;
using Skoggy.LD45.Game.Products;
using UnityEngine;

namespace Skoggy.LD45.Game.Carts
{
    public class ShoppingBasket : MonoBehaviour, IPickupable
    {
        public Rigidbody Rigidbody;
        public Collider Collider;
        public Transform CenterOfMass;

        public Vector3 Position => transform.position;
        private List<Product> _products = new List<Product>();

        public List<Product> Products => _products;

        void Start()
        {
            Rigidbody.centerOfMass = CenterOfMass.localPosition;
        }

        public bool AddToBasket(Product product)
        {
            if(product.InBasket)
            {
                return false;
            }

            product.DisableUsage(transform);
            product.InBasket = true;
            product.transform.localPosition = Vector3.up * (0.4f + (_products.Count * 0.15f));
            _products.Add(product);
            return true;
        }

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
            Rigidbody.centerOfMass = CenterOfMass.localPosition;
            Collider.enabled = true;
        }
    }
}