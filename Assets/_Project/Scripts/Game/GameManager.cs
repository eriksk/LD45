
using System;
using System.Collections.Generic;
using System.Linq;
using Skoggy.LD45.Game.Carts;
using Skoggy.LD45.Game.Players;
using Skoggy.LD45.Game.Products;
using UnityEngine;

namespace Skoggy.LD45.Game
{
    public class GameManager : MonoBehaviour
    {
        public Player Player;
        public List<ShoppingItem> ShoppingList;
        public List<string> AvailableProducts;
        public int Budget = 0;
        public Transform DropOffZone;
        public GameObject BasketPrefab;
        public Transform BasketSpawnPosition;

        void Start()
        {
            ShoppingList = new List<ShoppingItem>();
            CreateNewShoppingList();            
        }

        public void Restock()
        {
            var totalSalesCostOfRestockedItems = 0;
            var containers = GameObject.FindObjectsOfType<ProductContainer>();
            foreach(var container in containers)
            {
                totalSalesCostOfRestockedItems += container.Restock();
            }

            Budget -= totalSalesCostOfRestockedItems / 2;
        }

        public void Checkout(ShoppingBasket basket)
        {
            if(basket == null) return;

            var total = GetSalesTotal(basket);

            Budget += total;

            Destroy(basket.gameObject);
            Restock();
            Instantiate(BasketPrefab, BasketSpawnPosition.position, BasketSpawnPosition.rotation);
            CreateNewShoppingList();
        }

        private int GetSalesTotal(ShoppingBasket basket)
        {
            if(basket.Products.Count == 0) return 0;

            // TODO: Get prices of all on list only
            return basket.Products.Sum(x => x.Price);
        }

        public bool EverythingOnShoppingListIsInBasket(ShoppingBasket basket)
        {
            if(basket == null) return false;
            
            var basketProducts = basket.Products
                .ToLookup(x => x.Name);

            foreach(var item in ShoppingList)
            {
                if(basketProducts[item.ProductName].Count() < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
        
        private void CreateNewShoppingList()
        {
            ShoppingList = new List<ShoppingItem>();

            var notUsedProducts = AvailableProducts.ToArray().ToList();

            var items = UnityEngine.Random.Range(2, 5);

            for(var i = 0; i < items; i++)
            {
                if(notUsedProducts.Count == 0) return;

                var productIndex = UnityEngine.Random.Range(0, notUsedProducts.Count);
                var productName = notUsedProducts[productIndex];
                notUsedProducts.RemoveAt(productIndex);

                ShoppingList.Add(new ShoppingItem()
                {
                    ProductName = productName,
                    Quantity = UnityEngine.Random.Range(1, 3)
                });
            }
        }
    }

    public class ShoppingItem
    {
        public string ProductName;
        public int Quantity;
    }
}