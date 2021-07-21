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
				this.inventoryData[i].ItemID = ItemID.NotAnItem;
                this.inventoryData[i] = inventoryData[i];
				if (GameObjectData.IsAnItem(inventoryData[i].ItemID))
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
				if (inventoryData[i].ItemID == item.ItemID)
				{
					inventoryData[i].Amount += item.Amount;
					OnInventoryUpdate(new InventoryUpdateEventArgs
						(i, inventoryData[i]));
					return true;
				}
				if (!GameObjectData.IsAnItem(inventoryData[i].ItemID))
					firstEmptySlot = i;
			}
			if (firstEmptySlot < 0)
				return false;
			inventoryData[firstEmptySlot].Amount += item.Amount;
			inventoryData[firstEmptySlot].ItemID = item.ItemID;
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
                if (inventoryData[inventorySlotIndex].Amount == 0)
                {
                    inventoryData[inventorySlotIndex] = MouseInventorySlot.itemData;
                    MouseInventorySlot.SetItemData(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.Amount == 0)
                {
                    MouseInventorySlot.SetItemData(inventoryData[inventorySlotIndex], this, inventorySlotIndex);
                    inventoryData[inventorySlotIndex] = new InventorySlotData();
                }
                else if(inventoryData[inventorySlotIndex].ItemID == MouseInventorySlot.itemData.ItemID)
                {
                    inventoryData[inventorySlotIndex].Amount += MouseInventorySlot.itemData.Amount;
                    MouseInventorySlot.SetItemData(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if(inventoryData[inventorySlotIndex].ItemID != MouseInventorySlot.itemData.ItemID)
                {
                    InventorySlotData temp = inventoryData[inventorySlotIndex];
                    inventoryData[inventorySlotIndex] = MouseInventorySlot.itemData;
                    MouseInventorySlot.SetItemData(temp, this, inventorySlotIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (MouseInventorySlot.itemData.Amount > 0 && 
                    (MouseInventorySlot.itemData.ItemID == inventoryData[inventorySlotIndex].ItemID ||
                    inventoryData[inventorySlotIndex].Amount == 0))
                {
                    inventoryData[inventorySlotIndex].ItemID = MouseInventorySlot.itemData.ItemID;
                    inventoryData[inventorySlotIndex].Amount++;
                    MouseInventorySlot.SetItemData(new InventorySlotData(MouseInventorySlot.itemData.ItemID, 
                        MouseInventorySlot.itemData.Amount - 1), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.Amount == 0)
                {
                    InventorySlotData mouseInventorySlotData = inventoryData[inventorySlotIndex];
                    if (inventoryData[inventorySlotIndex].Amount % 2 == 1)
                        mouseInventorySlotData.Amount = mouseInventorySlotData.Amount / 2 + 1;
                    else
                        mouseInventorySlotData.Amount /= 2;
                    inventoryData[inventorySlotIndex].Amount /= 2;
                    MouseInventorySlot.SetItemData(mouseInventorySlotData, this, inventorySlotIndex);
                }
            }
            return inventoryData[inventorySlotIndex];
        }
    }
}
