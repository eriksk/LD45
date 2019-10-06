
using Skoggy.LD45.Game.Players;
using UnityEngine;

namespace Skoggy.LD45.UI
{
    public class UIGroceryList : MonoBehaviour
    {
        public Player Player;
        public GameObject RenderRoot;

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
        }
    }
}