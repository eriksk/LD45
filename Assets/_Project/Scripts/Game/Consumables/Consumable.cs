
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skoggy.LD45.Game.Consumables
{
    public class Consumable : MonoBehaviour
    {
        public float Weight = 1f;
        public List<Component> DestroyOnConsumed;
        private bool _consumed;

        public bool CanConsume(float weight)
        {
            return !_consumed && weight >= Weight;
        }

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
                Destroy(this);
            }
        }

        public void Attach(Transform t, float radius)
        {
            transform.SetParent(t);
            var position = transform.localPosition.normalized * radius;
            transform.localPosition = position;

            foreach(var c in DestroyOnConsumed)
            {
                Destroy(c);
            }
        }
    }
}