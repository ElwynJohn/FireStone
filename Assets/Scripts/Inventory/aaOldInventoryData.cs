using System.Collections.Generic;
using UnityEngine;

namespace Firestone.Inventory
{
    [CreateAssetMenu]
    public class aaOldInventoryData : ScriptableObject
    {
        //public variables
        public List<List<int>> List; // filled with itemData
        public int hotbarLength = 9;
        public int inventoryLength = 9;
        public int inventoryWidth = 3;
        public bool Open = false;



        //state variables
        List<int> itemData; // element 0 : item amount, element 1 : item gameID
        int invDataSize = 0;


        //functions
        private void OnEnable()
        {
            List = new List<List<int>>();
            itemData = new List<int>();
            itemData.Add(0);
            itemData.Add(-1);
            invDataSize = inventoryWidth * inventoryLength + hotbarLength;
            for (int i = 0; i < invDataSize; i++)
            {
                List.Add(itemData);
            }

            Open = false;
        }

        public void SetVariables(int hotbarLength, int inventoryLength, int inventoryWidth)
        {
            this.hotbarLength = hotbarLength;
            this.inventoryLength = inventoryLength;
            this.inventoryWidth = inventoryWidth;
            Open = false;
        }



        public void UpdateItemFrame(int amount, int gameID, int frameIndex, List<List<int>> listToChange)
        {
            List<int> newList = new List<int>();
            newList.Add(amount);
            newList.Add(gameID);
            listToChange[frameIndex] = newList;
        }

        public void PickUpItem(int amount, int gameID)
        {
            //find position for change to occur in array
            bool slotFound = false;
            for (int i = 0; i < invDataSize; i++)
            {
                if (List[i][1] == gameID && slotFound == false) //look for a location that is already holding this gameID
                {
                    slotFound = true;

                    List[i][0] += amount; //dont make new list since one has already been made


                }
            }
            if (slotFound == false)
            {
                for (int i = 0; i < invDataSize; i++)
                {
                    if (List[i][0] == 0 && slotFound == false) //look for empty slot
                    {
                        slotFound = true;

                        //make new list and replace array[index] with this list. if we dont make new list, other lists will be affected by changes
                        List<int> itemData = new List<int>();    //making a new list here doesnt affect any other lists because its local?!?!?
                        itemData.Add(List[i][0] + amount);
                        itemData.Add(gameID);
                        List[i] = itemData;
                    }
                }
            }
        }
    }
}