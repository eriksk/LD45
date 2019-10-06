
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Skoggy.LD45.UI
{
    public class UICheckoutResult : MonoBehaviour
    {
        public Text TextSales, TextTip, TextRestock;
        public GameObject Root;

        public void Show(int sales, int tip, int restock)
        {
            Root.SetActive(true);
            TextSales.text = $"${sales}";
            TextTip.text = $"${tip}";
            TextRestock.text = $"-${restock}";
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(3);
            Root.SetActive(false);
        }
    }
}