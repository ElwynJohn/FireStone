using Firestone.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Firestone.Gather
{
    public class HandleGather : MonoBehaviour
    {
        //config paramss
        [Header("Canvas UI")]
        [Tooltip("Slider used to show progress when gathering")]
        [SerializeField] Slider sliderPrefab = default;
        [Tooltip("Offset in pixels.")]
        [SerializeField] int sliderOffsetY = 50;
        [SerializeField] GameObject canvasUIPrefab = default;

        [Header("Positioning the drops.")]
        [Tooltip("Each item will be randomly dropped within this distance of each other")]
        [SerializeField] float dropPositionVariance = 0.15f;
        [Tooltip("How far this item will drop")]

        [Header("isGathering Data")]
        [SerializeField] MouseOverThisCollider mouseOverThisColliderClass = default;
        [SerializeField] InRangeCollider inRangeClass = default;

        [Header("GameObject Data")]
        [Tooltip("This holds data about the item that is being gathered")]
        [SerializeField] GameObjectData gameObjectData = default;



        //state variables

        private bool isGathering = false;
        private bool animatorSet = false;

        private Vector3 sliderPosition;
        private float timer = 0f;
        private int amountToDrop;

        //cache ref
        Animator m_Animator;
        Slider sliderInstance;
        RectTransform sliderRect;
        GameObject canvasInstance;
        Transform pickupsTransform;

        //functions

        private void Start()
        {
            m_Animator = gameObject.GetComponent<Animator>();
            if (GameObject.Find("Interactables/Pickups") != null)
            {
                pickupsTransform = GameObject.Find("Interactables/Pickups").transform;
            }
            else
            {
                Debug.LogWarning("'Gather' component tried to find 'Interactables/Pickups' in the hierarchy but was unsuccessful.",
                    gameObject);
            }
        }

        private void Update()
        {
            HandleGathering();
            if (m_Animator)
            {
                HandleGatheringAnimations();
            }
            HandleUI();
        }



        private void HandleGathering()
        {
            //simple timer
            if (mouseOverThisColliderClass.mouseOverThisCollider && inRangeClass.inRange && Input.GetMouseButton(0))
            {
                timer += Time.deltaTime;
                isGathering = true;
            }
            else
            {
                timer = 0;
                isGathering = false;
            }

            //timer complete, do something
            if (timer >= gameObjectData.TimeToGather)
            {
                amountToDrop = Random.Range(gameObjectData.AmountDropped - gameObjectData.AmountDroppedDeviation, gameObjectData.AmountDropped + gameObjectData.AmountDroppedDeviation);
                for (int i = 0; i < amountToDrop; i++)
                {
                    //create position for spawning drop
                    float positionVarianceX = Random.Range(-dropPositionVariance, dropPositionVariance);
                    float positionVarianceY = Random.Range(-dropPositionVariance, dropPositionVariance);
                    var positionVariance = new Vector3(positionVarianceX, positionVarianceY, 0);
                    var dropPositionWithVariance =
                        gameObject.transform.position + gameObjectData.dropPositionOffset + positionVariance;

                    //spawn object and set attributes
                    GameObject objectDropped =
                        Instantiate(gameObjectData.PickUpPrefab, dropPositionWithVariance, Quaternion.identity, pickupsTransform)
                        as GameObject;
                    objectDropped.GetComponent<IconAnimations>().SetVariables
                        (gameObjectData.SpawnInIdleState,
                        gameObjectData.DistanceToDrop,
                        gameObjectData.DistanceToDropDeviation,
                        gameObjectData.DropDecelleration,
                        gameObjectData.DropSpeed);
                }
                Destroy(gameObject);
                if (canvasInstance)
                {
                    Destroy(canvasInstance);
                }
            }
        }

        private void HandleGatheringAnimations()
        {
            if (isGathering && !animatorSet)
            {
                m_Animator.SetBool("isGathering", true);
                animatorSet = true;
            }
            else if (!isGathering)
            {
                animatorSet = false;
                m_Animator.SetBool("isGathering", false);
            }
        }

        private void HandleUI()
        {
            if (isGathering)
            {
                if (!sliderInstance) //instantiate slider 
                {
                    if (canvasInstance)
                    {
                        sliderInstance = Instantiate
                            (sliderPrefab, Vector3.zero, Quaternion.identity, canvasInstance.GetComponent<RectTransform>())
                            as Slider;
                    }
                    else //no canvas, instantiate canvas
                    {
                        canvasInstance = Instantiate(canvasUIPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                        sliderInstance = Instantiate
                            (sliderPrefab, Vector3.zero, Quaternion.identity, canvasInstance.GetComponent<RectTransform>())
                            as Slider;
                    }
                    sliderRect = sliderInstance.gameObject.GetComponent<RectTransform>();
                }

                //activate canvas and slider
                if (!canvasInstance.activeSelf)
                {
                    canvasInstance.SetActive(true);
                }

                if (!sliderInstance.gameObject.activeSelf)
                {
                    sliderInstance.gameObject.SetActive(true);
                }



                //set slider position fill
                sliderPosition = Input.mousePosition;
                sliderPosition.y -= sliderOffsetY;
                sliderRect.position = sliderPosition;

                sliderInstance.value = timer / gameObjectData.TimeToGather;
            }
            else
            {
                if (sliderInstance && sliderInstance.gameObject.activeSelf)
                {
                    sliderInstance.gameObject.SetActive(false);
                }

                if (canvasInstance && canvasInstance.activeSelf)
                {
                    canvasInstance.SetActive(false);
                }
            }
        }
    }
}