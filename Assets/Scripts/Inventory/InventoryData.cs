using UnityEngine;
using Firestone.Core;
using System;

namespace Firestone.Inventory
{
	public class InventoryUpdateEventArgs : EventArgs
	{
		public InventoryUpdateEventArgs(int inventorySlotIndex, InventorySlotData inventorySlotData)
		{
			this.InventorySlotIndex = inventorySlotIndex;
			this.InventorySlotData = inventorySlotData;
		}
		public int InventorySlotIndex;
		public InventorySlotData InventorySlotData;
	}

    public class InventoryData
    {
        public InventoryData(InventorySlotData[] inventoryData, int inventorySize)
        {
            this.inventoryData = new InventorySlotData[inventorySize];
			inventorySlotsLeft = inventorySize;
            for (int i = 0; i < inventoryData.Length && i < this.inventoryData.Length; i++)
            {
				this.inventoryData[i].itemID.itemID = -1;
                this.inventoryData[i] = inventoryData[i];
				if (GameObjectData.IsAnItem(inventoryData[i].itemID))
					inventorySlotsLeft--;
            }
        }

		private void OnInventoryUpdate(InventoryUpdateEventArgs args)
			=> InventoryUpdate?.Invoke(this, args);
		public event EventHandler<InventoryUpdateEventArgs> InventoryUpdate;

        public InventorySlotData[] inventoryData { get; private set; }
		private int inventorySlotsLeft;
		public bool IsFull { get => inventorySlotsLeft == 0; }

		public bool AddItemToInventory(InventorySlotData item)
		{
			int firstEmptySlot = -1;
			for (int i = 0; i < inventoryData.Length; i++)
			{
				if (inventoryData[i].itemID.itemID == item.itemID.itemID)
				{
					inventoryData[i].amount += item.amount;
					OnInventoryUpdate(new InventoryUpdateEventArgs
						(i, inventoryData[i]));
					return true;
				}
				if (!GameObjectData.IsAnItem(inventoryData[i].itemID))
					firstEmptySlot = i;
			}
			if (firstEmptySlot < 0)
				return false;
			inventoryData[firstEmptySlot].amount += item.amount;
			inventoryData[firstEmptySlot].itemID.itemID += item.itemID.itemID;
			inventorySlotsLeft--;
			OnInventoryUpdate(new InventoryUpdateEventArgs
				(firstEmptySlot, inventoryData[firstEmptySlot]));
			return true;
		}

		// @@Rework Make all inventory changes pass through AddItemToInventory
        public InventorySlotData InteractWithInventoryWithMouse(int inventorySlotIndex)
        {
            if (inventorySlotIndex >= inventoryData.Length)
            {
                Debug.LogError("Out of bounds exception");
                return default;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (inventoryData[inventorySlotIndex].amount == 0)
                {
                    inventoryData[inventorySlotIndex] = MouseInventorySlot.itemData;
                    MouseInventorySlot.SetItemData(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.amount == 0)
                {
                    MouseInventorySlot.SetItemData(inventoryData[inventorySlotIndex], this, inventorySlotIndex);
                    inventoryData[inventorySlotIndex] = new InventorySlotData();
                }
                else if(inventoryData[inventorySlotIndex].itemID.itemID == MouseInventorySlot.itemData.itemID.itemID)
                {
                    inventoryData[inventorySlotIndex].amount += MouseInventorySlot.itemData.amount;
                    MouseInventorySlot.SetItemData(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if(inventoryData[inventorySlotIndex].itemID.itemID != MouseInventorySlot.itemData.itemID.itemID)
                {
                    InventorySlotData temp = inventoryData[inventorySlotIndex];
                    inventoryData[inventorySlotIndex] = MouseInventorySlot.itemData;
                    MouseInventorySlot.SetItemData(temp, this, inventorySlotIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (MouseInventorySlot.itemData.amount > 0 && 
                    (MouseInventorySlot.itemData.itemID.itemID == inventoryData[inventorySlotIndex].itemID.itemID ||
                    inventoryData[inventorySlotIndex].amount == 0))
                {
                    inventoryData[inventorySlotIndex].itemID = MouseInventorySlot.itemData.itemID;
                    inventoryData[inventorySlotIndex].amount++;
                    MouseInventorySlot.SetItemData(new InventorySlotData(MouseInventorySlot.itemData.itemID, 
                        MouseInventorySlot.itemData.amount - 1), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.amount == 0)
                {
                    InventorySlotData mouseInventorySlotData = inventoryData[inventorySlotIndex];
                    if (inventoryData[inventorySlotIndex].amount % 2 == 1)
                        mouseInventorySlotData.amount = mouseInventorySlotData.amount / 2 + 1;
                    else
                        mouseInventorySlotData.amount /= 2;
                    inventoryData[inventorySlotIndex].amount /= 2;
                    MouseInventorySlot.SetItemData(mouseInventorySlotData, this, inventorySlotIndex);
                }
            }
            return inventoryData[inventorySlotIndex];
        }
    }
}
