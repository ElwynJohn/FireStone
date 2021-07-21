using Firestone.Core;

namespace Firestone.Inventory
{
    public struct InventorySlotData
    {
        public InventorySlotData(ItemID itemID, int amount)
        {
            ItemID = itemID;
            Amount = amount;
        }
        public ItemID ItemID { get; set; }
        public int Amount { get; set; }
    }
}
