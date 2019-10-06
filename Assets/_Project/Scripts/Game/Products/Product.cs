
using System;
using UnityEngine;

namespace Skoggy.LD45.Game.Products
{
    public class Product : MonoBehaviour, IPickupable
    {
        public string Name;
        public int Price = 10;
        
        private Rigidbody _rigidbody;
        private Collider _collider;

        public Vector3 Position => transform.position;
        public bool InBasket = false;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
        
        public void Grab()
        {
            if(InBasket) return;
            Destroy(_rigidbody);
            _collider.enabled = false;
        }

        public void DisableUsage(Transform newParent)
        {
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            Destroy(_rigidbody);
            Destroy(_collider);
        }

        public void Release()
        {
            if(InBasket) return;
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _collider.enabled = true;
        }

        public bool IsCart() => false;
        public bool IsProduct() => true;
    }
}