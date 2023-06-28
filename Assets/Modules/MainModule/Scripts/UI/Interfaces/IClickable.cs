using UnityEngine;

namespace UI
{
    public interface IClickable
    {
        public void OnPressAnimation();
        
        public void OnUnPressAnimation();
        
        public void OnMouseOverAnimation();
    }
}