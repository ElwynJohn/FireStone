using Firestone.Inventory;
using UnityEngine;

namespace Firestone.Core.ScriptableObjectVariables
{
    [CreateAssetMenu(fileName = "New InventoryDataVariable", menuName = "Variables/InventoryDataVariable")]
    public class InventoryDataVariable : ScriptableObject
    {
        public aaOldInventoryData InvData;
    }
}