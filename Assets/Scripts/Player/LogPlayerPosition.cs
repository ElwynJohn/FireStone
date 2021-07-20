using Firestone.Core.ScriptableObjectVariables;
using UnityEngine;

namespace Firestone.Player
{
    public class LogPlayerPosition : MonoBehaviour
    {
        [SerializeField] Vector3Variable playerPosition = default;

        private void Update()
        {
            playerPosition.Value = gameObject.transform.position;
        }
    }
}