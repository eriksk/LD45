
using System;
using System.Collections.Generic;
using System.Linq;
using Skoggy.LD45.Game.Carts;
using Skoggy.LD45.Game.Players;
using UnityEngine;

namespace Skoggy.LD45.Game
{
    public class GameManager : MonoBehaviour
    {
        public Player Player;
        public List<ShoppingItem> ShoppingList;
        public List<string> AvailableProducts;

        void Start()
        {
            ShoppingList = new List<ShoppingItem>();
            CreateNewShoppingList();            
        }

        public void Restock()
        {
        }

        public void Checkout()
        {
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

            var items = UnityEngine.Random.Range(3, 15);

            for(var i = 0; i < items; i++)
            {
                if(notUsedProducts.Count == 0) return;

                var productIndex = UnityEngine.Random.Range(0, notUsedProducts.Count);
                var productName = notUsedProducts[productIndex];
                notUsedProducts.RemoveAt(productIndex);

                ShoppingList.Add(new ShoppingItem()
                {
                    ProductName = productName,
                    Quantity = UnityEngine.Random.Range(1, 5)
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