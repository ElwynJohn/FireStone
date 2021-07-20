using Firestone.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Firestone.CraftingScripts
{
    [CreateAssetMenu(fileName = "New Item Crafting Data", menuName = "Item Crafting Data")]
    public class ItemCraftingData : ScriptableObject
    {
        [Serializable]
        public struct ItemAmount
        {
            public GameObjectData GameObjectData;
            public int Amount;
        }

        public List<ItemAmount> Input;
        public ItemAmount Output;

        public bool CanCraft()
        {
            return false;
        }

        public void Craft()
        {

        }
    }
}