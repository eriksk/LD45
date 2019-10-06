
using Skoggy.LD45.Game;
using Skoggy.LD45.UI;
using UnityEngine;

namespace Skoggy.LD45
{
    public static class ObjectLocator
    {
        private static Camera _camera;
        private static GameManager _gameManager;
        private static UICheckoutResult _checkoutResult;

        public static void Clear()
        {
            _camera = null;
            _gameManager = null;
            _checkoutResult = null;
        }

        public static Camera Camera => _camera ?? (_camera = Camera.main);
        public static GameManager GameManager => _gameManager = GetOrFind(_gameManager, "[GameManager]");
        public static UICheckoutResult Checkout => _checkoutResult = GetOrFind(_checkoutResult, "[CheckoutResult]", "Canvas");

        private static T GetOrFind<T>(T existing, string name, string containerName = "") where T : Behaviour
        {
            if(existing != null) return existing;

            var gameObject = GameObject.Find(name);
            
            if(gameObject == null) // find if inactive
            {
                gameObject = GameObject.Find(containerName).transform.Find(name)?.gameObject;
            }

            if(gameObject == null)
            {
                Debug.LogError($"Could not find '{typeof(T).FullName}' with name '{name}'");
                return null;
            }

            var component = gameObject.GetComponent<T>();

            if(component == null)
            {
                Debug.LogError($"Could not find component '{typeof(T).FullName}' for game object, accessor name '{name}'", gameObject);
            }

            return component;
        }
    }
}