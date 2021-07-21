using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firestone.Core;

namespace Firestone.Inventory
{
    [RequireComponent(typeof(RectTransform))]
    public class MouseInventorySlot : MonoBehaviour
    {
        [SerializeField] private Vector2 sizePixels = default;
        private static Image itemIcon = default;
        private static Image itemFrame = default;
        private static TextMeshProUGUI textMeshComponent = default;
        public static InventorySlotData itemData { get; private set; } = default;
        private static RectTransform rect = default;
        private static InventoryData lastInventoryTouched = default;
        private static int indexOfLastInventorySlotTouched = 0;
        private static bool isOpen = false;

        private void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = sizePixels;

            itemIcon = gameObject.transform.Find("Item Icon").gameObject.GetComponent<Image>();
            itemFrame = gameObject.transform.Find("Item Frame").gameObject.GetComponent<Image>();
            textMeshComponent = gameObject.transform.Find("Item Amount").gameObject.GetComponent<TextMeshProUGUI>();

            itemIcon.GetComponent<RectTransform>().sizeDelta = sizePixels;
            itemFrame.GetComponent<RectTransform>().sizeDelta = sizePixels;
            textMeshComponent.GetComponent<RectTransform>().sizeDelta = sizePixels;

            Display(false);
        }
        private void Update()
        {
            rect.position = Input.mousePosition + new Vector3(rect.sizeDelta.x / 2, -rect.sizeDelta.y / 2, 0);
        }

        public static void SetItemData(InventorySlotData itemData, InventoryData previousInventory, int previousInventorySlot)
        {
            lastInventoryTouched = previousInventory;
            indexOfLastInventorySlotTouched = previousInventorySlot;
            MouseInventorySlot.itemData = itemData;
            if (itemData.Amount <= 0)
            {
                Display(false);
            }
            else
            {
                Display(true);
                GameObjectData goData = Resources.Load<GameObjectData>(itemData.ItemID.ToString());
                itemIcon.sprite = goData.icon;
                textMeshComponent.text = itemData.Amount.ToString();
            }
        }
        public static void Display(bool display)
        {
            itemIcon.gameObject.SetActive(display);
            itemFrame.gameObject.SetActive(display);
            textMeshComponent.gameObject.SetActive(display);

            if (display)
                isOpen = true;
            else
                isOpen = false;
        }        
        public static void Close()
        {
            if (!isOpen)
                return;

            Display(false);
            lastInventoryTouched.inventoryData[indexOfLastInventorySlotTouched].Amount += itemData.Amount;
            lastInventoryTouched.inventoryData[indexOfLastInventorySlotTouched].ItemID = itemData.ItemID;
            SetItemData(new InventorySlotData(), lastInventoryTouched, indexOfLastInventorySlotTouched);
        }
    }
}
