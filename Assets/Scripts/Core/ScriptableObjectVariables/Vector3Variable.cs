using UnityEngine;

namespace Firestone.Core.ScriptableObjectVariables
{
    [CreateAssetMenu(fileName = "new Vector3Variable", menuName = "Variables/Vector3Variable")]
    public class Vector3Variable : ScriptableObject
    {
        public Vector3 Value;
    }
}