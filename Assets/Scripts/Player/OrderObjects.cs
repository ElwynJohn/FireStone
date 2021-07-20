using Firestone.Core.ScriptableObjectVariables;
using UnityEngine;

namespace Firestone.Player
{
    public class OrderObjects : MonoBehaviour
    {
        [SerializeField] Vector3Variable playerPosition = default;
        [SerializeField] SpriteRenderer mySpriteRenderer = default;

        private void Update()
        {
            if (gameObject.transform.position.y > playerPosition.Value.y) // if gameObject is behind player, check that object is on lower sorting layer
            {
                if (mySpriteRenderer.sortingLayerName != "Environment Behind")
                {
                    mySpriteRenderer.sortingLayerName = "Environment Behind";
                }
            }
            else if (mySpriteRenderer.sortingLayerName != "Environment Front") //else check that gameObject is on higher sorting layer
            {
                mySpriteRenderer.sortingLayerName = "Environment Front";
            }
        }
    }
}