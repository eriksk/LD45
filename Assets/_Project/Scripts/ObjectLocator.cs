
using Skoggy.LD45.Game;
using UnityEngine;

namespace Skoggy.LD45
{
    public static class ObjectLocator
    {
        private static Camera _camera;
        private static GameManager _gameManager;

        public static void Clear()
        {
            _camera = null;
            _gameManager = null;
        }

        public static Camera Camera => _camera ?? (_camera = Camera.main);
        public static GameManager GameManager => _gameManager = GetOrFind(_gameManager, "[GameManager]");

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