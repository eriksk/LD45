
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Skoggy.LD45.Game.Carts;
using Skoggy.LD45.Game.Players;
using Skoggy.LD45.Game.Products;
using Skoggy.LD45.UI;
using UnityEngine;
using UnityEngine.UI;

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
        public Text TextBudget;
        public Text TextTimer;
        public UIGameOverScreen GameOverScreen;
        private float _startTime;
        private int _maxTimeForTip;
        private bool _stateWorking;
        private float _timeLeft;
        private bool _gameOver;
        private bool _started;

        void Start()
        {
            TextTimer.text = "00:00";
            ShoppingList = new List<ShoppingItem>();
            _started = false;
        }

        public void StartGame()
        {
            _started = true;
            _timeLeft = (float)TimeSpan.FromMinutes(2).TotalSeconds;
            StartCoroutine(BeginNextCustomer());
            UpdateBudget();
        }

        private int RemainingTimeForBonus
        {
            get
            {
                var progress = (int)(Time.time - _startTime);
                return _maxTimeForTip - progress;
            }
        }

        void Update()
        {
            if(!_started) return;
            if(_gameOver) return;
            _timeLeft -= Time.deltaTime;

            if(_timeLeft <= 0f)
            {
                GameOver();
                return;
            }

            TextTimer.text = $"Time Left: {(int)_timeLeft}";
        }

        public void Restock()
        {
            var containers = GameObject.FindObjectsOfType<ProductContainer>();
            foreach(var container in containers)
            {
                container.Restock();
            }
        }

        private void UpdateBudget()
        {
            TextBudget.text = $"Budget: ${Budget}";
        }

        public void Checkout(ShoppingBasket basket)
        {
            if(basket == null) return;
            _stateWorking = false;

            var totalSales = GetSalesTotal(basket, out var unchargeable, out var perfectBasket);

            Budget += totalSales;

            var timeBonus = RemainingTimeForBonus > 0 ? RemainingTimeForBonus : 0;

            if(perfectBasket && timeBonus > 0)
            {
                Budget += timeBonus;
            }

            Destroy(basket.gameObject);
            Restock();

            Budget -= unchargeable;
            UpdateBudget();

            ObjectLocator.Checkout.Show(totalSales - unchargeable, perfectBasket ? timeBonus : 0, totalSales / 2);

            StartCoroutine(BeginNextCustomer());
        }

        private void GameOver()
        {
            _gameOver = true;
            GameOverScreen.gameObject.SetActive(true);
            GameOverScreen.Initialize(Budget);
        }

        private IEnumerator BeginNextCustomer()
        {
            yield return new WaitForSeconds(2);
            
            Instantiate(BasketPrefab, BasketSpawnPosition.position, BasketSpawnPosition.rotation);
            CreateNewShoppingList();

            yield return null;
            _stateWorking = true;
        }

        private int GetSalesTotal(ShoppingBasket basket, out int unchargeable, out bool perfectBasket)
        {
            perfectBasket = false;
            unchargeable = 0;
            if(basket.Products.Count == 0) return 0;

            var products = basket.Products.ToLookup(x => x.Name);
            var total = 0;

            perfectBasket = true;

            var validItems = ShoppingList.Select(x => x.ProductName).ToArray();

            foreach(var item in ShoppingList)
            {
                var charged = products[item.ProductName].Count();
                if(charged > 0)
                {
                    int count = Mathf.Min(item.Quantity, charged);
                    var overflow = charged > item.Quantity ? charged - item.Quantity : 0;
                    var price = products[item.ProductName].First().Price;
                    total += count * price;
                    if(count != item.Quantity)
                    {
                        perfectBasket = false;
                    }
                    unchargeable += overflow * price;
                }
                else
                {
                    perfectBasket = false;
                }
            }    

            var nonValidItems = products
                .Where(x => !validItems.Contains(x.Key))
                .Sum(x => x.Sum(f => f.Price));

            unchargeable += nonValidItems;

            return total;
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
            _startTime = Time.time;
            _maxTimeForTip = ShoppingList.Sum(x => x.Quantity) * 5;
        }
    }

    public class ShoppingItem
    {
        public string ProductName;
        public int Quantity;
    }
}