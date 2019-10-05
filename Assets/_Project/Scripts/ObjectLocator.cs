
using UnityEngine;

namespace Skoggy.LD45
{
    public static class ObjectLocator
    {
        private static Camera _camera;

        public static void Clear()
        {
            _camera = null;
        }

        public static Camera Camera => _camera ?? (_camera = Camera.main);

    }
}