using UnityEngine;

namespace Firestone.Inventory
{
    class PlayerInventory : Inventory
    {
        [SerializeField] int HotbarLength = 9;

        public int HotbarSlotSelected { get; set; } = 0;

        protected override void Start()
        {
            base.Start();
            itemFrameGrid.DisplayRangeOfGridElements(true, 0, HotbarLength);
            for (int i = 0; i < HotbarLength; i++)
            {
                if (InventoryData.inventoryData[i].amount > 0)
                {
                    itemIconGrid.DisplayRangeOfGridElements(true, i, i + 1);
                    itemAmountGrid.DisplayRangeOfGridElements(true, i, i + 1);
                }
            }
        }
        protected override void Update()
        {
            base.Update();
            if (!IsOpen)
                InteractWithHotbar();
        }
        private void InteractWithHotbar()
        {
            for (int i = 0; i < HotbarLength; i++)
            {
                if (i == HotbarSlotSelected)
                    continue;
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    itemFrameGrid.ChangeGridElementColor(i, new Color(0.7f, 0.7f, 1.0f, 1.0f));
                    itemFrameGrid.ChangeGridElementColor(HotbarSlotSelected, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    HotbarSlotSelected = i;
                    return;
                }
            }
        }

        protected override void Close()
        {
            base.Close();
            itemFrameGrid.DisplayRangeOfGridElements(true, 0, HotbarLength);
            for (int i = 0; i < HotbarLength; i++)
            {
                if (InventoryData.inventoryData[i].amount > 0)
                {
                    itemIconGrid.DisplayRangeOfGridElements(true, i, i + 1);
                    itemAmountGrid.DisplayRangeOfGridElements(true, i, i + 1);
                }
            }
        }
    }
}
