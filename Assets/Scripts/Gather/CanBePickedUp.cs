using Firestone.Core;
using Firestone.Inventory;
using UnityEngine;

namespace Firestone.Gather
{
    public class CanBePickedUp : MonoBehaviour
    {
        [SerializeField] GameObjectData gameObjectData = default;

        int amount = 1;

		private static InventoryData inventory = null;
        //functions
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
				if (inventory == null)
					inventory = GameObject.FindWithTag("Player Inventory")
						.GetComponent<PlayerInventory>()?.InventoryData;
				if (inventory == null || inventory.IsFull)
					return;
                inventory.AddItemToInventory(new InventorySlotData(gameObjectData.gameID, amount));
                Destroy(gameObject);
            }
        }

        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = gameObjectData.icon;
        }
    }
}
