
using UnityEngine;

namespace Skoggy.LD45.Game.Consumables
{
    public class Consumable : MonoBehaviour
    {
        public float Weight = 1f;
        private bool _consumed;

        public float Consume()
        {
            if(_consumed) return 0f;

            _consumed = true;

            return Weight;
        }

        void Update()
        {
            if(_consumed)
            {
                Destroy(gameObject);
            }
        }
    }
}