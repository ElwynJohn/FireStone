using UnityEngine;
using Firestone.Core;
using Firestone.UI;

namespace Firestone.Inventory
{
    public class Inventory : MonoBehaviour
    {

        [SerializeField] protected int capacity = 0;
        [Header("UI")]
        [SerializeField] protected UIGridImage itemFrameGrid = default;
        [SerializeField] protected UIGridImage itemIconGrid = default;
        [SerializeField] protected UIGridText itemAmountGrid = default;

        protected bool IsOpen { get; set; } = false;
        protected static bool IsAnyOpen { get; set; } = false;
        protected static bool AnyClosedThisUpdate { get; set; } = false;
        protected static bool AnyOpenedThisUpdate { get; set; } = false;
        protected bool Toggled { get; set; } = false;
        public InventoryData InventoryData { get; protected set; } = null;
        protected HoverableColliders ItemFrameColliders { get; set; } = null;
        protected int IndexOfLastInventorySlotHovered { get; set; } = 0;
        protected static MouseInventorySlot MouseInventory { get; set; } = default;

        protected virtual void Start()
        {
            InventorySlotData[] testData = new InventorySlotData[]
            {
                new InventorySlotData(ItemID.Wood, 100),
                new InventorySlotData(ItemID.Wood, 10),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Wood, 1),
                new InventorySlotData(ItemID.Chest, 4),
                new InventorySlotData(ItemID.Stone, 5)
            };
            InventoryData = new InventoryData(testData, capacity);
			InventoryData.InventoryUpdate += UpdateInventorySlot;
            for (int i = 0; i < InventoryData.inventoryData.Length && i < itemIconGrid.gridElements.Length; i++)
            {
                if (InventoryData.inventoryData[i].Amount > 0)
                {
                    GameObjectData goData = Resources.Load<GameObjectData>(InventoryData.inventoryData[i].ItemID.ToString());
                    itemIconGrid.ChangeGridElementSprite(i, goData.icon);
                    itemAmountGrid.ChangeTextToDisplay(i, InventoryData.inventoryData[i].Amount.ToString());
                }
            }
            itemFrameGrid.DisplayRangeOfGridElements(false, 0, itemFrameGrid.gridElements.Length);
            itemIconGrid.DisplayRangeOfGridElements(false, 0, itemFrameGrid.gridElements.Length);
            itemAmountGrid.DisplayRangeOfGridElements(false, 0, itemFrameGrid.gridElements.Length);
        }

        protected virtual void Update()
        {
            Toggled = Input.GetKeyDown(KeyCode.E);
            if (Toggled || IsOpen)
                Interact();
        }
        protected void LateUpdate()
        {
            AnyClosedThisUpdate = false;
            AnyOpenedThisUpdate = false;
        }

        public void Interact()
        {
            if (Toggled)
                Toggle();
            if (IsOpen)
            {
                int indexOfInventorySlotHovered = ItemFrameColliders.IndexOfColliderHovered();
                bool isInventorySlotHovered = indexOfInventorySlotHovered != -1;
                bool isInventorySlotClicked = isInventorySlotHovered && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1));
                if (isInventorySlotClicked)
                {
            		InventoryData.InteractWithInventoryWithMouse(indexOfInventorySlotHovered);
                }

                //give player visual feedback by changing inventory slot colours when hovered
                if (isInventorySlotHovered && IndexOfLastInventorySlotHovered != indexOfInventorySlotHovered)
                {
                    itemFrameGrid.ChangeGridElementColor(indexOfInventorySlotHovered, new Color(0.3f, 0.3f, 0.7f, 1.0f));

                    if (IndexOfLastInventorySlotHovered != -1)
                        itemFrameGrid.ChangeGridElementColor(IndexOfLastInventorySlotHovered, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    IndexOfLastInventorySlotHovered = indexOfInventorySlotHovered;
                }
                if (!isInventorySlotHovered && IndexOfLastInventorySlotHovered != -1)
                {
                    itemFrameGrid.ChangeGridElementColor(IndexOfLastInventorySlotHovered, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    IndexOfLastInventorySlotHovered = -1;
                }
            }
        }

        public virtual void UpdateInventorySlot(object sender, InventoryUpdateEventArgs args)
        {
            if (args.InventorySlotData.Amount == 0)
            {
                itemAmountGrid.DisplayRangeOfGridElements
					(false, args.InventorySlotIndex, args.InventorySlotIndex + 1);
                itemIconGrid.DisplayRangeOfGridElements
					(false, args.InventorySlotIndex, args.InventorySlotIndex + 1);
            }
            else
            {
                GameObjectData gameObjectData = Resources.Load
					<GameObjectData>(args.InventorySlotData.ItemID.ToString());
                itemIconGrid.ChangeGridElementSprite
					(args.InventorySlotIndex, gameObjectData.icon);
                itemAmountGrid.ChangeTextToDisplay
					(args.InventorySlotIndex, args.InventorySlotData.Amount.ToString());
				if (IsOpen)
				{
	                itemIconGrid.DisplayRangeOfGridElements
						(true, args.InventorySlotIndex, args.InventorySlotIndex + 1);
	                itemAmountGrid.DisplayRangeOfGridElements
						(true, args.InventorySlotIndex, args.InventorySlotIndex + 1);
				}
            }
        }

        protected void Open()
        {
            if ((IsAnyOpen && !AnyOpenedThisUpdate) || AnyClosedThisUpdate)
                return;

            itemFrameGrid.DisplayRangeOfGridElements(true, 0, itemFrameGrid.gridElements.Length);
            for (int i = 0; i < itemFrameGrid.gridElements.Length; i++)
            {
                if (InventoryData.inventoryData[i].Amount > 0)
                {
                    itemAmountGrid.DisplayRangeOfGridElements(true, i, i + 1);
                    itemIconGrid.DisplayRangeOfGridElements(true, i, i + 1);
                }
            }
            if (ItemFrameColliders == null)
                InstantiateItemFrameColliders();
            IsOpen = true;
            IsAnyOpen = true;
            AnyOpenedThisUpdate = true;
        }
        protected virtual void Close()
        {
            AnyClosedThisUpdate = true;
            itemFrameGrid.DisplayRangeOfGridElements(false, 0, itemIconGrid.gridElements.Length);
            itemIconGrid.DisplayRangeOfGridElements(false, 0, itemIconGrid.gridElements.Length);
            itemAmountGrid.DisplayRangeOfGridElements(false, 0, itemIconGrid.gridElements.Length);
            IsOpen = false;
            IsAnyOpen = false;
            MouseInventorySlot.Close();
			// @@FIXME: probably dont need this if FIXME01 is fixed
            for (int i = 0; i < InventoryData.inventoryData.Length; i++)
                itemAmountGrid.ChangeTextToDisplay(i, InventoryData.inventoryData[i].Amount.ToString());
        }
        private void Toggle()
        {
            if (IsOpen)
                Close();
            else
                Open();
        }

        protected void InstantiateItemFrameColliders()
        {
            RectTransform[] itemFrameTransforms = new RectTransform[itemFrameGrid.gridElements.Length];
            for (int i = 0; i < itemFrameTransforms.Length; i++)
                itemFrameTransforms[i] = itemFrameGrid.gridElements[i].GetComponent<RectTransform>();
            ItemFrameColliders = new HoverableColliders(itemFrameTransforms);
        }
    }
}
