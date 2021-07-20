using UnityEngine;
using Firestone.Core;

namespace Firestone.Inventory
{
    public class InventoryData
    {
        public InventoryData()
        {

        }
        public InventoryData(InventorySlotData[] inventoryData, int inventorySize)
        {
            this.inventoryData = new InventorySlotData[inventorySize];
            for (int i = 0; i < inventoryData.Length && i < this.inventoryData.Length; i++)
            {
                this.inventoryData[i] = inventoryData[i];
            }
        }

        public InventorySlotData[] inventoryData { get; private set; }

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
