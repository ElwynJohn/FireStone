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

		private int inventorySlotsLeft;
		public bool IsFull { get => inventorySlotsLeft == 0; }
        private InventorySlotData[] inventoryData { get; set; }
		public InventorySlotData GetSlotAtIndex(int index) => inventoryData[index];
		public int Length { get => inventoryData.Length; }

		// returns true if the item was successfully added
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
			// if the search did not find a valid slot for the item, return false.
			if (inventoryIndex < 0)
				return false;
			// if inventoryIndex was given by user, it may be a bad index. We guard
			// against that here
			if (!(item.ItemID == inventoryData[inventoryIndex].ItemID 
					|| inventoryData[inventoryIndex].ItemID == ItemID.NotAnItem))
				return false;

			item.Amount += inventoryData[inventoryIndex].Amount;
			SetSlot(inventoryIndex, item);
			return true;
		}
		private void SetSlot(int slotIndex, InventorySlotData item)
		{
			if (inventoryData[slotIndex].ItemID == ItemID.NotAnItem)
				inventorySlotsLeft--;
			else if (item.ItemID == ItemID.NotAnItem)
				inventorySlotsLeft++;

			inventoryData[slotIndex].Amount = item.Amount;
			if (item.Amount == 0)
				inventoryData[slotIndex].ItemID = ItemID.NotAnItem;
			else
				inventoryData[slotIndex].ItemID = item.ItemID;

			OnInventoryUpdate(new InventoryUpdateEventArgs
				(slotIndex, inventoryData[slotIndex]));
		}

        public void InteractWithInventoryWithMouse(int inventorySlotIndex)
        {
			var invSlot = inventoryData[inventorySlotIndex];
			var mouseInvSlot = MouseInventorySlot.itemData;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (invSlot.Amount == 0)
                {
					AddItemToInventory(mouseInvSlot, inventorySlotIndex);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if (mouseInvSlot.Amount == 0)
                {
                    MouseInventorySlot.SetItemData
						(invSlot, this, inventorySlotIndex);
					var itemData = new InventorySlotData
						(invSlot.ItemID, -1 * invSlot.Amount);
					AddItemToInventory(itemData, inventorySlotIndex);
                }
                else if(invSlot.ItemID == mouseInvSlot.ItemID)
                {
					AddItemToInventory(mouseInvSlot, inventorySlotIndex);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(), this, inventorySlotIndex);
                }
                else if(invSlot.ItemID != mouseInvSlot.ItemID)
                {
                    InventorySlotData temp = invSlot;
					var itemData = new InventorySlotData
						(invSlot.ItemID, -1 * invSlot.Amount);
					AddItemToInventory(itemData, inventorySlotIndex);
					AddItemToInventory(mouseInvSlot, inventorySlotIndex);
                    MouseInventorySlot.SetItemData(temp, this, inventorySlotIndex);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (mouseInvSlot.Amount > 0 && 
                    (mouseInvSlot.ItemID == invSlot.ItemID 
					|| invSlot.Amount == 0))
                {
					AddItemToInventory(new InventorySlotData
						(mouseInvSlot.ItemID, 1), inventorySlotIndex);
                    MouseInventorySlot.SetItemData
						(new InventorySlotData(mouseInvSlot.ItemID, 
                        mouseInvSlot.Amount - 1), this, inventorySlotIndex);
                }
                else if (mouseInvSlot.Amount == 0)
                {
                    InventorySlotData mouseInventorySlotData = invSlot;
                    if (invSlot.Amount % 2 == 1)
                        mouseInventorySlotData.Amount = mouseInventorySlotData.Amount / 2 + 1;
                    else
                        mouseInventorySlotData.Amount /= 2;
					var itemData = new InventorySlotData
						(invSlot.ItemID, invSlot.Amount / 2 * -1);
					SetSlot(inventorySlotIndex, new InventorySlotData
						(invSlot.ItemID, 
						invSlot.Amount / 2));
                    MouseInventorySlot.SetItemData(mouseInventorySlotData, this, inventorySlotIndex);
                }
            }
        }
    }
}
