using UnityEngine;

namespace Firestone.Gather
{
    public class IconAnimations : MonoBehaviour
    {
        [Header("floating Data")]
        [SerializeField] float floatHeight = 0.16f;
        [SerializeField] float floatFrequency = 1f;


        [Header("Other Drop Data")]
        [Tooltip("Starting x Position on the sin graph that determines the y movement.")]
        [SerializeField] float dropAcceleration = 10f;
        [SerializeField] float moveSpeed = 1f;
        [Tooltip("How quickly the item slows down after hitting the ground.")]
        [SerializeField] float moveSpeedDecay = 1.5f;

        private const float startingPoint = 1.573f;
        private Vector3 movementThisFrame;
        private float timer = 0f;
        private float distanceDropped = 0f;



        private float distanceToDrop = 0.4f;
        private float distanceToDropDeviation = 0.1f;
        private bool isIdle = true;
        private float dropDecelleration = 20f;
        private float dropSpeed = 2f;

        public void SetVariables
            (bool isIdle, float distanceToDrop, float distanceToDropDeviation, float dropDecelleration, float dropSpeed)
        {
            this.isIdle = isIdle;
            this.distanceToDrop = distanceToDrop;
            this.distanceToDropDeviation = distanceToDropDeviation;
            this.dropDecelleration = dropDecelleration;
            this.dropSpeed = dropSpeed;
        }



        void Start()
        {
            distanceToDrop = distanceToDrop += Random.Range(-distanceToDropDeviation, distanceToDropDeviation);
            if (isIdle)
            {
                moveSpeed = 0f;
            }
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (!isIdle) //control drop path
            {
                float sinOperand = timer * dropAcceleration;
                if (sinOperand + startingPoint > 4.71f)
                {
                    sinOperand = 4.71f - startingPoint;
                }

                movementThisFrame = new Vector3
                    (Time.deltaTime * moveSpeed, Mathf.Sin(sinOperand + startingPoint) * dropSpeed * Time.deltaTime, 0);



                //check if object has landed
                distanceDropped -= movementThisFrame.y;
                if (distanceDropped >= distanceToDrop)
                {
                    isIdle = true;
                    timer = 0f;
                }
            }
            else //object has "landed", now the object transitions into a floating state
            {
                //control x movement
                if (moveSpeed > Mathf.Epsilon)
                {
                    moveSpeed -= Time.deltaTime * moveSpeedDecay;
                }
                else
                {
                    moveSpeed = 0;
                }

                //control y movement and set movement this frame
                if (dropSpeed > Mathf.Epsilon)
                {
                    dropSpeed -= Time.deltaTime * dropDecelleration;
                    movementThisFrame = new Vector3(Time.deltaTime * moveSpeed, -dropSpeed * Time.deltaTime, 0);
                }
                else
                {
                    movementThisFrame = new Vector3
                        (moveSpeed * Time.deltaTime, Mathf.Sin(floatFrequency * timer) * floatHeight * Time.deltaTime, 0);
                }
            }
            gameObject.transform.position += movementThisFrame;
        }
    }
}