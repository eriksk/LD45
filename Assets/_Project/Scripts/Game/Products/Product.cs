
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

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
        
        public void Grab()
        {
            Destroy(_rigidbody);
            _collider.enabled = false;
        }
        
        public void Release()
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _collider.enabled = true;
        }

        public bool IsCart() => false;
        public bool IsProduct() => true;
    }
}