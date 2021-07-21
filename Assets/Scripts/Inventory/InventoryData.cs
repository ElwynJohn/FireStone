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

		//returns true if the item was successfully added
		public bool AddItemToInventory(InventorySlotData item, int inventoryIndex = -1)
		{
			if (inventoryIndex == -1)
				for (int i = inventoryData.Length - 1; i >= 0 ; i--)
				{
					if (inventoryData[i].ItemID == item.ItemID)
					{
						inventoryIndex = i;
						break;
					}
					if (!GameObjectData.IsAnItem(inventoryData[i].ItemID))
						inventoryIndex = i;
				}
			if (inventoryIndex < 0)
				return false;

			item.Amount += inventoryData[inventoryIndex].Amount;
			SetSlot(inventoryIndex, item);
			return true;
		}
		public void SetSlot(int slotIndex, InventorySlotData item)
		{
			if (inventoryData[slotIndex].ItemID == ItemID.NotAnItem)
				inventorySlotsLeft--;
			inventoryData[slotIndex].Amount = item.Amount;
			inventoryData[slotIndex].ItemID = item.ItemID;
			OnInventoryUpdate(new InventoryUpdateEventArgs
				(slotIndex, inventoryData[slotIndex]));
		}

        public void InteractWithInventoryWithMouse(int inventorySlotIndex)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (inventoryData[inventorySlotIndex].Amount == 0)
                {
					SetSlot(inventorySlotIndex, MouseInventorySlot.itemData);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.Amount == 0)
                {
                    MouseInventorySlot.SetItemData
						(inventoryData[inventorySlotIndex], this, inventorySlotIndex);
					SetSlot(inventorySlotIndex, new InventorySlotData());
                }
                else if(inventoryData[inventorySlotIndex].ItemID == MouseInventorySlot.itemData.ItemID)
                {
					AddItemToInventory(MouseInventorySlot.itemData, inventorySlotIndex);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if(inventoryData[inventorySlotIndex].ItemID != MouseInventorySlot.itemData.ItemID)
                {
                    InventorySlotData temp = inventoryData[inventorySlotIndex];
					SetSlot(inventorySlotIndex, MouseInventorySlot.itemData);
                    MouseInventorySlot.SetItemData(temp, this, inventorySlotIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (MouseInventorySlot.itemData.Amount > 0 && 
                    (MouseInventorySlot.itemData.ItemID == inventoryData[inventorySlotIndex].ItemID 
					|| inventoryData[inventorySlotIndex].Amount == 0))
                {
					AddItemToInventory(new InventorySlotData
						(MouseInventorySlot.itemData.ItemID, 1), inventorySlotIndex);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(MouseInventorySlot.itemData.ItemID, 
                        MouseInventorySlot.itemData.Amount - 1), this, inventorySlotIndex);
                }
                else if (MouseInventorySlot.itemData.Amount == 0)
                {
                    InventorySlotData mouseInventorySlotData = inventoryData[inventorySlotIndex];
                    if (inventoryData[inventorySlotIndex].Amount % 2 == 1)
                        mouseInventorySlotData.Amount = mouseInventorySlotData.Amount / 2 + 1;
                    else
                        mouseInventorySlotData.Amount /= 2;
					SetSlot(inventorySlotIndex, new InventorySlotData
						(inventoryData[inventorySlotIndex].ItemID, 
						inventoryData[inventorySlotIndex].Amount / 2));
                    MouseInventorySlot.SetItemData(mouseInventorySlotData, this, inventorySlotIndex);
                }
            }
        }
    }
}
