using Firestone.Core.ScriptableObjectVariables;
using UnityEngine;

namespace Firestone.Player
{
    public class CharacterAnimations : MonoBehaviour
    {
        [SerializeField] BoolVariable isRunning = default;

        Animator myAnimator;
        Camera mainCam;

        private void Start()
        {
            myAnimator = gameObject.GetComponent<Animator>();
            mainCam = Camera.main;
        }

        void Update()
        {
            HandleAnimations();
            FlipSprite();
        }



        private void HandleAnimations()
        {
            if (isRunning.Value)
            {
                myAnimator.SetBool("isRunning", true);
                myAnimator.SetBool("isIdle", false);
            }
            else
            {
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isIdle", true);
            }
        }



        private void FlipSprite()
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (gameObject.transform.localScale.x > 0 != mouseWorldPos.x > gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3
                    (-1 * gameObject.transform.localScale.x,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
            }
        }
    }
}