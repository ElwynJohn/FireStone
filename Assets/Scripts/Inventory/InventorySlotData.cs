using Firestone.Core;

namespace Firestone.Inventory
{
    public struct InventorySlotData
    {
        public InventorySlotData(ItemID itemID, int amount)
        {
            this.itemID = itemID;
            this.amount = amount;
        }
        public ItemID itemID;
        public int amount;
    }
}
