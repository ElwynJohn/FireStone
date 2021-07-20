using System.Collections.Generic;
using UnityEngine;
using Firestone.Gather;
using Firestone.Core;
using Firestone.Core.ScriptableObjectVariables;

namespace Firestone.Inventory
{
    public class aaOldChest : MonoBehaviour
    {
        //config params
        [Header("Class Cache")]
        [SerializeField] InventoryDataVariable chestDataInUse = default;
        [SerializeField] InventoryDataVariable chestCanvasDataInUse = default;

        [SerializeField] InRangeCollider inRangeClass = default;
        [SerializeField] MouseOverThisCollider mouseOverThisColliderClass = default;

        [Header("Variables")]
        [SerializeField] float dropPositionVariance = 0.15f;
        [SerializeField] float posVariancePerItemX = 0.35f; //stops items of different types being dropped in exact same spot
        [SerializeField] float posVariancePerItemY = 0.1f; //stops items of different types being dropped in exact same spot

        //state variables
        private bool isQuitting = false;

        //cache ref
        aaOldInventoryData thisChestCanvas;
        aaOldInventoryData thisChestInv;



        //functions

        private void Start()
        {
            //when a chest spawns, give it a new dedicated scriptable object for holding its data
            thisChestInv = ScriptableObject.CreateInstance("aaOldInventoryData") as aaOldInventoryData;
            thisChestInv.SetVariables(0, 3, 2);

            //do the same for the chest's canvas data
            thisChestCanvas = ScriptableObject.CreateInstance("aaOldInventoryData") as aaOldInventoryData;
            thisChestCanvas.SetVariables(0, 3, 2);
        }

        private void Update()
        {
            if (inRangeClass.inRange && mouseOverThisColliderClass.mouseOverThisCollider && Input.GetMouseButtonDown(1))
            {
                if (!thisChestInv.Open)
                {
                    //make this scriptable object the scriptable object that is being used for reading/writing chest data
                    chestDataInUse.InvData = thisChestInv;
                    chestDataInUse.InvData.Open = true;
                    chestCanvasDataInUse.InvData = thisChestCanvas;
                }
            }



            if (!inRangeClass.inRange && thisChestInv.Open == true) //if walk out of range, close
            {
                chestDataInUse.InvData.Open = false;
            }
        }



        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void OnDestroy()
        {
            if (!isQuitting)
            {
                foreach (List<int> itemSpaceData in thisChestInv.List)
                {
                    if (itemSpaceData[0] > 0)
                    {
                        var gameObjectData = Resources.Load<GameObjectData>(itemSpaceData[1].ToString());

                        var positionVariancePerItemType = RandomVectorWithinRect(posVariancePerItemX, posVariancePerItemY);

                        for (int i = 0; i < itemSpaceData[0]; i++)
                        {
                            //create position for spawning drops
                            var positionVariance = RandomVectorWithinRect(dropPositionVariance, dropPositionVariance);
                            var dropPositionWithVariance =
                                gameObject.transform.position + positionVariancePerItemType + positionVariance;

                            GameObject droppedObj =
                                Instantiate(gameObjectData.PickUpPrefab, dropPositionWithVariance, Quaternion.identity)
                                as GameObject;
                            droppedObj.GetComponent<IconAnimations>().SetVariables
                                (gameObjectData.SpawnInIdleState,
                                gameObjectData.DistanceToDrop,
                                gameObjectData.DistanceToDropDeviation,
                                gameObjectData.DropDecelleration,
                                gameObjectData.DropSpeed);
                        }
                    }
                }
            }
        }

        private Vector3 RandomVectorWithinRect(float dropPositionVarianceX, float dropPositionVarianceY)
        {
            float positionVarianceX = Random.Range(-dropPositionVarianceX, dropPositionVarianceX);
            float positionVarianceY = Random.Range(-dropPositionVarianceY, dropPositionVarianceY);

            return new Vector3(positionVarianceX, positionVarianceY, 0);
        }
    }
}