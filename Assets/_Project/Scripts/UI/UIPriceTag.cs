
using System;
using System.Linq;
using Skoggy.LD45.Game.Players;
using Skoggy.LD45.Game.Products;
using UnityEngine;
using UnityEngine.UI;

namespace Skoggy.LD45.UI
{
    public class UIPriceTag : MonoBehaviour
    {
        public GameObject Root;
        public Text TextDescription;
        public Text TextPrice;
        public Player Player;
        private Product _product;

        private void Set(Product product)
        {
            _product = product;
            if(product == null)
            {
                Root.SetActive(false);
                return;
            }

            Root.SetActive(true);
            TextDescription.text = product.Name;
            TextPrice.text = $"${product.Price}";
        }

        void Update()
        {
            // TODO: Do this update like 4 times / second or something

            if(Player == null)
            {
                Set(null);
                return;
            }

            if(Player.CarryingProduct)
            {
                Set(Player.Product);
            }
            else
            {
                var product = FindNearestProduct();
                Set(product);
            }
        }

        private Product FindNearestProduct()
        {
            var products = GameObject.FindObjectsOfType<Product>().Where(x => !x.InBasket);

            var point = Player.PickupPoint.position;
            
            var product = products
                .Where(x => x != null)
                .Where(x => Vector3.Distance(x.Position, point) < 1.5f)
                .OrderBy(x => Vector3.Distance(x.Position, point))
                .FirstOrDefault();

            return product;
        }
    }
}