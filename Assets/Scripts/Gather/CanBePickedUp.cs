using Firestone.Core;
using Firestone.Inventory;
using UnityEngine;

namespace Firestone.Gather
{
    public class CanBePickedUp : MonoBehaviour
    {
        [SerializeField] GameObjectData gameObjectData = default;
        [SerializeField] aaOldInventoryData invData = default;
        //cache ref
        int amount = 1;

        //functions
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                invData.PickUpItem(amount, gameObjectData.gameID);
                Destroy(gameObject);
            }
        }

        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = gameObjectData.icon;
        }
    }
}
