using UnityEngine;

namespace Firestone.Core.ScriptableObjectVariables
{
    [CreateAssetMenu(fileName = "New IntVariable", menuName = "Variables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public int Value;
    }
}