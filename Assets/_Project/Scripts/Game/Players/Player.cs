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
        public Animator Animator;
        public Rigidbody Rigidbody;
        public bool IsPlayer = false;

        [Header("Movement")]
        public float Speed = 1f;
        public float Damping = 0.1f;
        public float RotationSpeed = 10f;

        public Transform PickupPoint;
        public Transform GrabHandle;

        private Vector3 _movement;
        private ShoppingBasket _basket;
        private Product _product;

        void Start()
        {
        }
        
        public bool CarryingAnything => _basket != null || _product != null;
        public bool CarryingProduct => _product != null;
        public bool CarryingBasked => _basket != null;
        public Product Product => _product;

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

            Animator.SetBool("walking", _movement.magnitude > 0.25f);
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
                    var basket = GetBasketIfNear();

                    _product.Release();
                    _product.transform.SetParent(null);

                    if(basket != null)
                    {
                        if(basket.AddToBasket(_product))
                        {
                            // TODO: Effects and stuff, and check shopping list
                        }
                    }

                    _product = null;
                }
                
                Animator.SetBool("grabbed", false);
            }
            else
            {
                var pickedUpAnything = TryPickup();
                if(pickedUpAnything)
                {
                    Animator.SetBool("grabbed", true);
                }
            }
        }

        private ShoppingBasket GetBasketIfNear()
        {
            var carts = GameObject.FindObjectsOfType<ShoppingBasket>();
            
            return carts
                .Where(x => Vector3.Distance(x.Position, PickupPoint.position) < 1.5f)
                .OrderBy(x => Vector3.Distance(x.Position, PickupPoint.position))
                .FirstOrDefault();
        }

        private bool TryPickup()
        {
            if(CarryingAnything) return false;

            var products = GameObject.FindObjectsOfType<Product>().Where(x => !x.InBasket);
            var carts = GameObject.FindObjectsOfType<ShoppingBasket>();

            var items = new List<IPickupable>();
            items.AddRange(products);
            items.AddRange(carts);

            var nearestItem = items
                .Where(x => Vector3.Distance(x.Position, PickupPoint.position) < 1.5f)
                .OrderBy(x => Vector3.Distance(x.Position, PickupPoint.position))
                .FirstOrDefault();

            if(nearestItem == null) return false;

            if(nearestItem is Product)
            {
                _product = nearestItem as Product;
                _product.transform.SetParent(GrabHandle);
                _product.transform.localPosition = Vector3.zero;
                // _product.transform.localRotation = Quaternion.Euler(0f, -90f, 15f);
                _product.transform.localScale = Vector3.one;
                _product.Grab();
                return true;
            }

            if(nearestItem is ShoppingBasket)
            {
                _basket = nearestItem as ShoppingBasket;
                _basket.transform.SetParent(GrabHandle);
                _basket.transform.localPosition = Vector3.zero;
                _basket.transform.localRotation = Quaternion.Euler(90f, -90f, 0f);
                _basket.transform.localScale = Vector3.one;
                _basket.Grab();
                return true;
            }

            return false;
        }

        void FixedUpdate()
        {
            var rigidbody = Rigidbody;

            rigidbody.AddForce(_movement * Speed * Time.fixedDeltaTime);
            var flatVelocity = Rigidbody.velocity;
            flatVelocity.y = 0f;

            rigidbody.AddForce(-flatVelocity * Damping * Time.fixedDeltaTime);

            var position = transform.position;
            position.y = 0f; // lulz
            transform.position = position;
        }
    }
}