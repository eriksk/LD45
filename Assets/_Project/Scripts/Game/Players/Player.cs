using System;
using System.Collections.Generic;
using System.Linq;
using Skoggy.LD45.Game.Carts;
using Skoggy.LD45.Game.Products;
using UnityEngine;

namespace Skoggy.LD45.Game.Players
{
    public class Player : MonoBehaviour
    {
        public Animation Animation;
        public Rigidbody Rigidbody;
        public bool IsPlayer = false;

        [Header("Movement")]
        public float Speed = 1f;
        public float Damping = 0.1f;
        public float RotationSpeed = 10f;

        public Transform PickupPoint;

        private Vector3 _movement;
        private ShoppingBasket _basket;
        private Product _product;

        void Start()
        {
        }
        
        public bool CarryingAnything => _basket != null || _product != null;

        void Update()
        {
            if(IsPlayer)
            {
                UpdatePlyerInput();
            }

            UpdateInteractables();
        }

        private void UpdateInteractables()
        {
        }

        private void UpdatePlyerInput()
        {
            var input = new Vector3(
                Input.GetAxis("Horizontal"),
                0f,
                Input.GetAxis("Vertical")
            );
    
            _movement = input.normalized * Mathf.Clamp01(input.magnitude);

            if(_movement.magnitude > 0.25f)
            {
                Rigidbody.MoveRotation(Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(_movement.normalized, Vector3.up),
                    RotationSpeed * Time.deltaTime
                ));
            }

            if(Input.GetButtonDown("Jump"))
            {
                HandleAction();
            }
        }

        private void HandleAction()
        {            

            if(CarryingAnything)
            {
                if(_basket != null)
                {
                    _basket.Release();
                    _basket.transform.SetParent(null);
                    _basket = null;
                }
                if(_product != null)
                {
                    _product.Release();
                    _product.transform.SetParent(null);
                    _product = null;
                }
            }
            else
            {
                TryPickup();
            }
        }

        private void TryPickup()
        {
            if(CarryingAnything) return;

            var products = GameObject.FindObjectsOfType<Product>();
            var carts = GameObject.FindObjectsOfType<ShoppingBasket>();

            var items = new List<IPickupable>();
            items.AddRange(products);
            items.AddRange(carts);

            var nearestItem = items
                .Where(x => Vector3.Distance(x.Position, PickupPoint.position) < 1.5f)
                .OrderBy(x => Vector3.Distance(x.Position, PickupPoint.position))
                .FirstOrDefault();

            if(nearestItem == null) return;

            if(nearestItem is Product)
            {
                _product = nearestItem as Product;
                _product.transform.SetParent(transform);
                _product.transform.localPosition = new Vector3(0f, 0.27f, 0.6f);
                _product.transform.localRotation = Quaternion.Euler(0f, -90f, 15f);
                _product.transform.localScale = Vector3.one;
                _product.Grab();
                return;
            }

            if(nearestItem is ShoppingBasket)
            {
                _basket = nearestItem as ShoppingBasket;
                _basket.transform.SetParent(transform);
                _basket.transform.localPosition = new Vector3(0f, 0.27f, 0.6f);
                _basket.transform.localRotation = Quaternion.Euler(0f, -90f, 15f);
                _basket.transform.localScale = Vector3.one;
                _basket.Grab();
                return;
            }
        }

        void FixedUpdate()
        {
            var rigidbody = Rigidbody;

            rigidbody.AddForce(_movement * Speed * Time.fixedDeltaTime);
            var flatVelocity = Rigidbody.velocity;
            flatVelocity.y = 0f;

            rigidbody.AddForce(-flatVelocity * Damping * Time.fixedDeltaTime);
        }
    }
}