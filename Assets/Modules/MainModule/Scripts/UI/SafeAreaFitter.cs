using System;
using UnityEngine;

namespace Content.Scripts.UI
{
    public enum SafeAreaType
    {
        Base,
        Inverse
    }

    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private SafeAreaType safeAreaType;

        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;

            switch (safeAreaType)
            {
                case SafeAreaType.Base:
                {
                    var anchorMin = Vector2.zero;
                    var anchorMax = safeArea.position + safeArea.size;
                    
                    anchorMax.x /= Screen.width;
                    anchorMax.y /= Screen.height;
                    
                    rectTransform.anchorMin = anchorMin;
                    rectTransform.anchorMax = anchorMax;
                    break;
                }
                case SafeAreaType.Inverse:
                {
                    var anchorMin = safeArea.position;
                    var anchorMax = safeArea.position + safeArea.size;
                    
                    anchorMin.x /= Screen.width;
                    anchorMin.y /= Screen.height;
                    anchorMax.x /= Screen.width;
                    anchorMax.y /= Screen.height;

                    rectTransform.anchorMin = new Vector2(0f, anchorMax.y);
                    rectTransform.anchorMax = Vector2.one;
                    break;
                }
            }
        }
    }
}