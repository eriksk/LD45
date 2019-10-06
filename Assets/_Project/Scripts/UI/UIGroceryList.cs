
using System;
using System.Collections.Generic;
using System.Linq;
using Skoggy.LD45.Game.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Skoggy.LD45.UI
{
    public class UIGroceryList : MonoBehaviour
    {
        public Player Player;
        public GameObject RenderRoot;
        public Transform ListContainer;
        public GameObject ListItemPrefab;
        public Color Neutral, Good, Bad, Started;

        void Update()
        {
            if(Player == null) 
            {
                RenderRoot.SetActive(false);
                return;
            }

            if(!Player.CarryingBasked)
            {
                RenderRoot.SetActive(false);
                return;
            }
            
            RenderRoot.SetActive(true);

            var productsInBasket = Player.Basket.Products;

            var productsGroupedWithCount = productsInBasket
                .Select(x => x.Name)
                .ToLookup(x => x);

            var textsWithColor = new List<TextWithColor>();

            var shoppingList = ObjectLocator.GameManager.ShoppingList;
            var shoppingListNames = shoppingList.Select(x => x.ProductName).ToArray();
            foreach(var item in shoppingList.OrderBy(x => x.ProductName))
            {
                var count = productsGroupedWithCount[item.ProductName].Count();
                var text = $"{item.ProductName} {count} / {item.Quantity}";

                var color = Neutral;

                if(count == item.Quantity)
                {
                    color = Good;
                }
                if(count > 0 && count < item.Quantity)
                {
                    color = Started;
                }
                if(count > item.Quantity)
                {
                    color = Bad;
                }

                textsWithColor.Add(new TextWithColor()
                {
                    Text = text,
                    Color = color
                });
            }

            var productsNotInList = productsInBasket
                .Where(x => !shoppingListNames.Contains(x.Name))
                .GroupBy(x => x.Name);

            foreach(var p in productsNotInList)
            {
                textsWithColor.Add(new TextWithColor()
                {
                    Text = $"{p.Key} ({p.Count()}) - NOT ON LIST!",
                    Color = Bad
                });
            }

            RenderList(textsWithColor);
        }

        private void RenderList(List<TextWithColor> textsWithColor)
        {
            DestroyAllChildren(ListContainer);

            foreach(var t in textsWithColor)
            {
                var item = Instantiate(ListItemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(ListContainer);
                var text = item.GetComponent<Text>();
                text.color = t.Color;
                text.text = t.Text;
            }
        }

        private void DestroyAllChildren(Transform parent)
        {
            var count = parent.childCount;
            for(var i = 0; i < count; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    class TextWithColor
    {
        public string Text { get; set; }
        public Color Color { get; set; }
    }
}