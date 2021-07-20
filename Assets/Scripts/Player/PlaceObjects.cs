using Firestone.Core;
using Firestone.Core.ScriptableObjectVariables;
using Firestone.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Firestone.Player
{
    public class PlaceObjects : MonoBehaviour
    {
        [SerializeField] IntVariable hotkeyIndex = default;
        [SerializeField] aaOldInventoryData invData = default;
        [SerializeField] Transform parent = default;

        private List<int> hotkeyDataHolder;
        private int lastFrameHotKey;

        GameObjectData gameObjectData;
        SpriteRenderer hoverSprite;


        void Start()
        {
            //create list to check against for changes (mimic of hotkey data from Player Inventory Data)
            hotkeyDataHolder = new List<int>();
            hotkeyDataHolder.Add(0);
            hotkeyDataHolder.Add(-1);
        }

        void Update()
        {
            UpdateHotkeyDataHolder();

            if (!invData.Open)
            {
                ControlPlacement();
            }
            else
            {
                if (hoverSprite)
                {
                    Destroy(hoverSprite.gameObject);
                }
            }
        }



        private void ControlPlacement()
        {
            //if hotkey changed, destroy old hoverSprite
            if (hotkeyIndex.Value != lastFrameHotKey)
            {
                lastFrameHotKey = hotkeyIndex.Value;
                if (hoverSprite)
                {
                    Destroy(hoverSprite.gameObject);
                }
            }

            if (hotkeyDataHolder[0] > 0 && gameObjectData.CanBePlaced && gameObjectData.PlaceablePrefab) //if amount of item > 0, we can place
            {
                //find position on grid for item to go
                Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawnPos.x = Mathf.Floor(spawnPos.x) + 0.5f;
                spawnPos.y = Mathf.Floor(spawnPos.y) + 0.5f;
                spawnPos.z = 0;

                //handle hoverSprite (lets the player see where the object will be placed before he places it)
                if (hoverSprite && gameObjectData.Sprite == hoverSprite.sprite)
                {
                    hoverSprite.gameObject.transform.position = spawnPos;
                }
                else
                {
                    GameObject newObj = new GameObject();
                    newObj.name = "Hover Sprite";
                    hoverSprite = newObj.AddComponent<SpriteRenderer>();
                    hoverSprite.sprite = gameObjectData.Sprite;
                    hoverSprite.gameObject.transform.position = spawnPos;
                }
                //

                if (Input.GetMouseButtonDown(0))
                {
                    //spawn gameobject and reset hoverSprite
                    GameObject spawnedObj = Instantiate(gameObjectData.PlaceablePrefab, spawnPos, Quaternion.identity, parent)
                        as GameObject; //Spawn Prefab
                    spawnedObj.GetComponent<Animator>().SetTrigger("Spawned");

                    Destroy(hoverSprite.gameObject);
                    //

                    //Update hotkey Data
                    hotkeyDataHolder[0] -= 1;
                    if (hotkeyDataHolder[0] == 0)
                    {
                        hotkeyDataHolder[1] = -1;
                    }
                    invData.UpdateItemFrame(hotkeyDataHolder[0], hotkeyDataHolder[1], hotkeyIndex.Value, invData.List);
                }
            }
        }

        private void UpdateHotkeyDataHolder()
        {
            if (hotkeyDataHolder[0] != invData.List[hotkeyIndex.Value][0])
            {
                hotkeyDataHolder[0] = invData.List[hotkeyIndex.Value][0];
            }

            if (hotkeyDataHolder[1] != invData.List[hotkeyIndex.Value][1])
            {
                hotkeyDataHolder[1] = invData.List[hotkeyIndex.Value][1];
                if (hotkeyDataHolder[1] != -1)
                {
                    gameObjectData = Resources.Load<GameObjectData>(hotkeyDataHolder[1].ToString());
                }
            }
        }
    }
}