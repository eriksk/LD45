using System.Collections.Generic;
using UnityEngine;

namespace Skoggy.LD45.Game.Products
{
    public class ProductContainer : MonoBehaviour
    {
        public GameObject ProductPrefab;
        public Transform[] ProductPositions;

        private List<Product> _products = new List<Product>();

        void Start()
        {
            Restock();
        }

        public int Restock()
        {
            var stillInStock = 0;
            foreach(var product in _products)
            {
                if(product == null) continue;
                if(product.gameObject == null) continue;
                stillInStock++;
                Destroy(product.gameObject);
            }
            _products.Clear();
            
            var soldItems = ProductPositions.Length - stillInStock;

            var price = 0;

            foreach(var p in ProductPositions)
            {
                var product = Instantiate(ProductPrefab, p.position, p.rotation).GetComponent<Product>();
                _products.Add(product);
                price = product.Price;
            }

            return soldItems * price;
        }
    }
}