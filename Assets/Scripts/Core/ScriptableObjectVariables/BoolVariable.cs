using UnityEngine;

namespace Firestone.Core.ScriptableObjectVariables
{
    [CreateAssetMenu(fileName = "New BoolVariable", menuName = "Variables/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;
    }
}