using UnityEngine;

namespace Firestone.Core
{
    public class InRangeCollider : MonoBehaviour
    {
        public bool inRange = false;

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                inRange = false;
            }
        }
    }
}