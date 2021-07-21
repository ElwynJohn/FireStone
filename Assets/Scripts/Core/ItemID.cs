namespace Firestone.Core
{
	// @@Rework: Make this an enum?
	[System.Serializable]
    public struct ItemID
    {
        public ItemID(int itemID) => this.itemID = itemID;
        public int itemID;
    }
}
