using UnityEngine;

namespace Firestone.Core.ScriptableObjectVariables
{
    [CreateAssetMenu(fileName = "New FloatVariable", menuName = "Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
    }
}