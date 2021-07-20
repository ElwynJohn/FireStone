using UnityEngine;

namespace Firestone.Core
{
    public class MouseOverThisCollider : MonoBehaviour
    {
        public bool mouseOverThisCollider = false;

        private void OnMouseEnter()
        {
            mouseOverThisCollider = true;
        }

        private void OnMouseExit()
        {
            mouseOverThisCollider = false;
        }
    }
}