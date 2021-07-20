using UnityEngine;
using Firestone.Core;

namespace Firestone.Inventory
{
    public class ChestInventory : Inventory
    {
        [Header("Range")]
        [SerializeField] InRangeCollider inRangeCollider = default;
        [SerializeField] MouseOverThisCollider mouseOverThisCollider = default;

        protected override void Update()
        {
            AnyClosedThisUpdate = false;
            AnyOpenedThisUpdate = false;
            bool closeInventory = IsOpen && (!inRangeCollider.inRange || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape));
            Toggled = inRangeCollider.inRange && mouseOverThisCollider.mouseOverThisCollider && Input.GetKeyDown(KeyCode.E);
            if (closeInventory)
                Close();
            else if (Toggled || IsOpen)
                Interact();
        }
    }
}
