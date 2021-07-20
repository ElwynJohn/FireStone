using UnityEditor;
using Firestone.ProceduralGeneration;
using UnityEditor.UIElements;

namespace Firestone.Editor
{
    [CustomEditor(typeof(ProceduralGenerator))]
    public class InspectorHandler : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ProceduralGenerator proceduralGenerator = (ProceduralGenerator)target;
            proceduralGenerator.test = EditorGUILayout.Vector2IntField("Tile Position", proceduralGenerator.test);
            UnityEngine.Debug.Log(proceduralGenerator.test);
            //EditorGUILayout.EditorToolbar(new Vector2IntField[] { new Vector2IntField("TilePosition", proceduralGenerator.test) });
        }
    }
}