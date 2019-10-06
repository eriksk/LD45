using UnityEngine;

namespace Skoggy.LD45.UI
{
    [ExecuteInEditMode]
    public class UIFitToParent : MonoBehaviour
    {
        public Vector2 Padding;

        void Update()
        {
            var parent = transform.parent as RectTransform;
            if(parent == null) return;

            var t = transform as RectTransform;
            t.localPosition = Vector3.zero;
            t.sizeDelta = parent.sizeDelta - Padding;
        }
    }
}