using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firestone.Core.ScriptableObjectVariables;
using Firestone.Core;

namespace Firestone.Inventory
{

    /*TODO

    check code is clean

    make it work for all resolutions

    */

    public class aaOldInventory : MonoBehaviour
    {
        //config params
        [Header("Inventory Data")]
        [SerializeField] aaOldInventoryData invData = default;
        [SerializeField] InventoryDataVariable chestDataVariable = default;
        [SerializeField] aaOldInventoryData mouseFrame = default;
        [SerializeField] IntVariable hotkeyIndex = default;

        [Header("Inventory Canvas Data")]
        [SerializeField] aaOldInventoryData invCanvasData = default;
        [SerializeField] InventoryDataVariable chestCanvasDataVariable = default;
        [SerializeField] aaOldInventoryData mouseFrameCanvas = default;

        [Header("Other")]
        [SerializeField] Sprite itemFrame = default;
        [SerializeField] float fontSize = 40f;


        //state variables
        private float invXOffset;
        private bool mouseItemFrameExists = false;
        private bool chestOpen = false;

        //cache ref
        Transform inventoryFramesTransform;
        Transform chestFramesTransform;
        BoxCollider2D cursorCollider;
        Color hoverframeColor = new Color(0.7f, 0.7f, 0.7f, 100f);
        Color previousColor = new Color(1f, 1f, 1f, 1f);
        Color inHandColor = new Color(0.7f, 0.7f, 1f, 1f);
        Image frameImage;
        GameObject mouseFrameHolder;

        //functions
        void Start()
        {
            //Create and Set Parent for tidy Hierarchy
            GameObject parentHolder = new GameObject();
            parentHolder.name = "Frames Holder";

            inventoryFramesTransform = parentHolder.transform;
            inventoryFramesTransform.SetParent(gameObject.transform);
            inventoryFramesTransform.localPosition = Vector2.zero;

            //find offset and padding of item frames based on resolution
            invXOffset = Screen.width * 0.27f;
            //create hotbar frames
            InstantiateItemFrames(new Vector2(invData.hotbarLength, 1), new Vector2(invXOffset, 100), 120,
                inventoryFramesTransform, invData.List);
        }

        void Update()
        {
            HandleInventory();
            HandleMouseFrame();
            HandleChest();
            Hotkeys(invData);
        }



        private void InstantiateItemFrames(Vector2 framesRowsColoumns, Vector2 offset, int padding, Transform parent,
            List<List<int>> List)
        {

            for (int indexY = 0; indexY < framesRowsColoumns.y; indexY++)
            {
                for (int indexX = 0; indexX < framesRowsColoumns.x; indexX++)
                {
                    //create image
                    GameObject newObj = new GameObject();
                    Image newImage = newObj.AddComponent<Image>();

                    //set image attributes and position
                    newObj.name = "itemFrame";
                    newObj.layer = LayerMask.NameToLayer("UI");
                    newImage.sprite = itemFrame;

                    var transform = newObj.GetComponent<RectTransform>();
                    transform.SetParent(parent);
                    transform.localPosition = new Vector2(offset.x + indexX * padding - Screen.width / 2,
                        offset.y + indexY * -padding - Screen.height / 2);
                    transform.anchorMin = new Vector2(0.5f, 0f);
                    transform.anchorMax = new Vector2(0.5f, 0f);


                    int thisChildIndex = parent.childCount - 1;
                    InstantiateAmount(newObj.transform, thisChildIndex, List[thisChildIndex][0]);
                    if (List[thisChildIndex][1] >= 0)
                    {
                        InstantiateImage(newObj.transform, thisChildIndex, List[thisChildIndex][1]);
                    }
                }
            }
        }

        private void InstantiateAmount(Transform parentTransform, int thisChildIndex, int amount) //creates text to show number of items in item frame
        {
            //create
            GameObject newObj = new GameObject();
            TextMeshProUGUI itemText = newObj.AddComponent<TextMeshProUGUI>();

            //set position
            var transform = itemText.GetComponent<RectTransform>();
            transform.SetParent(parentTransform.transform);
            transform.localPosition = new Vector2(120, -30);

            //set attributes
            newObj.name = "itemAmount";
            itemText.text = amount.ToString();
            itemText.fontSize = fontSize;
            TMP_FontAsset font = Resources.Load<TMP_FontAsset>("game_over SDF");
            itemText.font = font;
        }

        private void CheckAmountUpdates(Transform parentTransform, List<List<int>> list)
        {
            for (int frame = 0; frame < parentTransform.childCount; frame++)
            {
                TextMeshProUGUI tmp = parentTransform.GetChild(frame).GetChild(0).GetComponent<TextMeshProUGUI>();
                if (tmp.text != list[frame][0].ToString())
                {
                    tmp.text = list[frame][0].ToString();
                }
            }
        }


        private void InstantiateImage(Transform parentTransform, int thisChildIndex, int gameID)
        {
            if (gameID >= 0)
            {
                if (parentTransform.childCount == 2)
                {
                    Destroy(parentTransform.GetChild(1).gameObject);
                }
                //create
                GameObject newObj = new GameObject();
                Image newImage = newObj.AddComponent<Image>();

                //set position
                var imageTransform = newImage.GetComponent<RectTransform>();
                imageTransform.SetParent(parentTransform);
                imageTransform.localPosition = Vector2.zero;

                //set attributes
                newObj.name = "Item Icon";

                GameObjectData gameObjectData = Resources.Load<GameObjectData>(gameID.ToString());
                newImage.sprite = gameObjectData.icon;
            }

            else if (gameID == -1)
            {
                Destroy(parentTransform.GetChild(1).gameObject);
            }
        }

        private void CheckImageUpdates(Transform parentTransform, List<List<int>> list, aaOldInventoryData canvasScriptableObject)
        {
            for (int frame = 0; frame < parentTransform.childCount; frame++)
            {
                if (canvasScriptableObject.List[frame][1] != list[frame][1]) //if gameID changed, update image
                {
                    canvasScriptableObject.UpdateItemFrame(list[frame][0], list[frame][1], frame, canvasScriptableObject.List);
                    InstantiateImage(parentTransform.GetChild(frame), frame, list[frame][1]);
                }
            }
        }






        private void HandleChest()
        {
            if (chestDataVariable.InvData)
            {
                if (chestDataVariable.InvData.Open && !chestOpen)
                {
                    OpenChest();
                }
                else if (!invData.Open && chestOpen)
                {
                    CloseChest();
                }
                else if (!chestDataVariable.InvData.Open && chestOpen)
                {
                    CloseChest();
                    //close inv
                    invData.Open = false;
                    DeleteItemFrames(invData.hotbarLength, inventoryFramesTransform); //
                }
                if (chestOpen)
                {
                    InteractWithInventory(chestDataVariable.InvData.List, chestFramesTransform, chestDataVariable.InvData);
                    CheckImageUpdates(chestFramesTransform, chestDataVariable.InvData.List, chestCanvasDataVariable.InvData);
                    CheckAmountUpdates(chestFramesTransform, chestDataVariable.InvData.List);
                }
            }
        }

        private void OpenChest()
        {
            chestOpen = true;

            //create parent to chest frames and set its transform
            GameObject chestFrames = new GameObject();
            chestFrames.name = "Chest Frames";

            chestFramesTransform = chestFrames.transform;
            chestFramesTransform.SetParent(gameObject.transform);
            chestFramesTransform.localPosition = Vector2.zero;
            //
            InstantiateItemFrames(new Vector2(chestDataVariable.InvData.inventoryLength, chestDataVariable.InvData.inventoryWidth),
                new Vector2(invXOffset, 900),
                120, chestFrames.transform, chestDataVariable.InvData.List);
            InstantiateFrameColliders(chestFramesTransform);

            OpenInventory();
        }

        private void CloseChest()
        {
            chestOpen = false;

            DeleteItemFrames(0, chestFramesTransform);
            chestDataVariable.InvData.Open = false;
        }



        private void HandleInventory()
        {
            CheckImageUpdates(inventoryFramesTransform, invData.List, invCanvasData);
            CheckAmountUpdates(inventoryFramesTransform, invData.List);

            if (invData.Open)
            {
                InteractWithInventory(invData.List, inventoryFramesTransform, invData);

                CheckImageUpdates(inventoryFramesTransform, invData.List, invCanvasData);
                CheckAmountUpdates(inventoryFramesTransform, invData.List);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!invData.Open)
                {
                    OpenInventory();
                }
                else
                {
                    //close inv
                    invData.Open = false;
                    DeleteItemFrames(invData.hotbarLength, inventoryFramesTransform); //
                }
            }
        }

        private void OpenInventory()
        {
            invData.Open = true;

            InstantiateItemFrames(new Vector2(invData.inventoryLength, invData.inventoryWidth), new Vector2(invXOffset, 580),
                120, inventoryFramesTransform, invData.List);
            InstantiateFrameColliders(inventoryFramesTransform);
        }

        private void InteractWithInventory(List<List<int>> list, Transform itemFramesHolder, aaOldInventoryData scriptableObject)
        {
            // use ray at mouse position to see if/which item frame collider we hit
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z;
            Ray ray = new Ray(mousePos, new Vector3(0, 0, 1));
            RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);

            if (hitInfo)
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("UI") &&
                    hitInfo.collider.gameObject.transform.parent == itemFramesHolder)
                {
                    //deal with color for player feedback
                    if (frameImage)
                    {
                        frameImage.color = previousColor;
                    }
                    frameImage = hitInfo.collider.gameObject.GetComponent<Image>();
                    previousColor = frameImage.color;
                    frameImage.color = hoverframeColor;

                    if (Input.GetMouseButtonDown(0)) //deal with moving items in inventory
                    {
                        int siblingIndex = hitInfo.collider.gameObject.transform.GetSiblingIndex(); //the index of the item frame being affected

                        if (mouseFrame.List[0][0] == 0) //put item in MouseFrame
                        {
                            mouseFrame.List[0][0] = list[siblingIndex][0];
                            mouseFrame.List[0][1] = list[siblingIndex][1];
                            list[siblingIndex][0] = 0;
                            list[siblingIndex][1] = -1;
                        }
                        else if (mouseFrame.List[0][1] == list[siblingIndex][1]) //add item in MouseFrame to item with same gameID in item frame
                        {
                            scriptableObject.UpdateItemFrame(mouseFrame.List[0][0] + list[siblingIndex][0],
                                mouseFrame.List[0][1], siblingIndex, list);
                            mouseFrame.List[0][0] = 0;
                            mouseFrame.List[0][1] = -1;
                        }
                        else if (list[siblingIndex][0] == 0) //put item in MouseFrame into empty item frame
                        {
                            scriptableObject.UpdateItemFrame(mouseFrame.List[0][0], mouseFrame.List[0][1],
                                siblingIndex, list);
                            mouseFrame.List[0][0] = 0;
                            mouseFrame.List[0][1] = -1;
                        }
                        else if (mouseFrame.List[0][0] > 0) //swap mouseFrame item with the item in the item slot that we clicked on
                        {
                            //create a list to hold the mouseFrame data whilst it gets changed
                            var newListInt = new List<int>() { mouseFrame.List[0][0], mouseFrame.List[0][1] };
                            var mouseFrameTemp = new List<List<int>>() { newListInt };

                            mouseFrame.UpdateItemFrame(list[siblingIndex][0], list[siblingIndex][1], 0, mouseFrame.List);

                            scriptableObject.UpdateItemFrame(mouseFrameTemp[0][0], mouseFrameTemp[0][1], siblingIndex, list);
                        }
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        int siblingIndex = hitInfo.collider.gameObject.transform.GetSiblingIndex(); //the index of the item frame being affected

                        if (mouseFrame.List[0][0] > 0 && list[siblingIndex][0] == 0) //put 1 item from mouseFrame to item frame
                        {
                            scriptableObject.UpdateItemFrame(1, mouseFrame.List[0][1], siblingIndex, list);
                            mouseFrame.UpdateItemFrame(mouseFrame.List[0][0] - 1, mouseFrame.List[0][1], 0, mouseFrame.List);
                        }
                        else if (mouseFrame.List[0][0] > 0 && mouseFrame.List[0][1] == list[siblingIndex][1]) //if mouseFrame gameID same as item frame gameID, take 1 item from mouseFrame and put in item frame
                        {
                            scriptableObject.UpdateItemFrame(list[siblingIndex][0] + 1, list[siblingIndex][1], siblingIndex, list);
                            mouseFrame.UpdateItemFrame(mouseFrame.List[0][0] - 1, mouseFrame.List[0][1], 0, mouseFrame.List);
                        }
                        else if (mouseFrame.List[0][0] == 0 && list[siblingIndex][0] > 0) //if mouseFrame empty and click on occupied item frame, half (to ceiling) amount in item frame and put it in mouseFrame
                        {
                            float floatAmountOfItem = list[siblingIndex][0]; //cast to a float so we can divide by 2 on next line
                            var amountToPutInMouseFrame = (int)Mathf.Ceil(floatAmountOfItem / 2f);

                            scriptableObject.UpdateItemFrame
                                (list[siblingIndex][0] - amountToPutInMouseFrame, list[siblingIndex][1], siblingIndex, list);
                            mouseFrame.UpdateItemFrame(amountToPutInMouseFrame, list[siblingIndex][1], 0, mouseFrame.List);
                        }
                    }
                }
            }
            else if (frameImage)
            {
                frameImage.color = previousColor; //
            }
        }



        private void InstantiateFrameColliders(Transform parentOfFrames)
        {
            for (int frameIndex = 0; frameIndex < parentOfFrames.childCount; frameIndex++)
            {
                GameObject frame = parentOfFrames.GetChild(frameIndex).gameObject;
                BoxCollider2D frameCollider = frame.AddComponent<BoxCollider2D>();
                frameCollider.size = frame.GetComponent<RectTransform>().sizeDelta;
            }
        }

        private void DeleteItemFrames(int startingIndex, Transform parent)
        {
            for (int i = startingIndex; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
            for (int i = 0; i < startingIndex; i++)
            {
                Destroy(parent.GetChild(i).gameObject.GetComponent<BoxCollider2D>());
            }
        }



        private void HandleMouseFrame()
        {
            if (mouseFrame.List[0][0] > 0)
            {
                if (mouseItemFrameExists)
                {
                    mouseFrameHolder.transform.position = Input.mousePosition;
                    CheckImageUpdates(mouseFrameHolder.transform, mouseFrame.List, mouseFrameCanvas);
                    CheckAmountUpdates(mouseFrameHolder.transform, mouseFrame.List);
                }
                else
                {
                    //setup mouse frameholder
                    mouseFrameHolder = new GameObject();
                    mouseFrameHolder.name = "Mouse Frame Holder";
                    mouseFrameHolder.transform.SetParent(gameObject.transform);
                    mouseFrameHolder.transform.position = Input.mousePosition;
                    //
                    InstantiateItemFrames(new Vector2(1, 1), new Vector2(Screen.width / 2, Screen.height / 2),
                        0, mouseFrameHolder.transform, mouseFrame.List);
                    mouseItemFrameExists = true;
                }
            }
            else if (mouseItemFrameExists)
            {
                Destroy(mouseFrameHolder); //destroying the parent destroys all children
                mouseItemFrameExists = false;
            }
        }



        private void Hotkeys(aaOldInventoryData scriptableObject)
        {
            for (int hotkey = 0; hotkey < scriptableObject.hotbarLength; hotkey++)
            {
                if (Input.GetKeyDown((hotkey + 1).ToString())) //update hotkey depending on button pressed
                {
                    inventoryFramesTransform.GetChild(hotkeyIndex.Value).gameObject.GetComponent<Image>().color
                        = new Color(1f, 1f, 1f, 1f);
                    inventoryFramesTransform.GetChild(hotkey).gameObject.GetComponent<Image>().color = inHandColor;
                    hotkeyIndex.Value = hotkey;
                }
            }
        }
    }
}