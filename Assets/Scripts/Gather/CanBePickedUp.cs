using Firestone.Core;
using Firestone.Inventory;
using UnityEngine;

namespace Firestone.Gather
{
    public class CanBePickedUp : MonoBehaviour
    {
        [SerializeField] GameObjectData gameObjectData = default;

        int amount = 1;
		// handled deals with the fact that player has two colliders.
		bool handled = false;

		private static InventoryData inventory = null;
        //functions
        private void OnTriggerStay2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
				if (inventory == null)
					inventory = GameObject.FindWithTag("Player Inventory")
						.GetComponent<PlayerInventory>()?.InventoryData;
				if (inventory == null || handled)
					return;
                bool success = inventory.AddItemToInventory
					(new InventorySlotData(gameObjectData.gameID, amount));
				if (success)
				{
					handled = true;
                	Destroy(gameObject);
				}
            }
        }

        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = gameObjectData.icon;
        }
    }
}
