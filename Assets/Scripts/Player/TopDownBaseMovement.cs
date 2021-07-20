using Firestone.Core.ScriptableObjectVariables;
using UnityEngine;

namespace Firestone.Player
{
    public class TopDownBaseMovement : MonoBehaviour
    {
        [SerializeField] BoolVariable canRun = default;
        [SerializeField] BoolVariable isRunning = default;
        [SerializeField] float moveSpeed = 1f;

        Rigidbody2D rb;



        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Run();
        }

        private void Run()
        {
            if (canRun.Value)
            {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");
                var move = new Vector2(horizontalMove * moveSpeed, verticalMove * moveSpeed);

                rb.velocity = move;
                if (move.magnitude > Mathf.Epsilon)
                {
                    isRunning.Value = true;
                }
                else
                {
                    isRunning.Value = false;
                }
            }
        }
    }
}