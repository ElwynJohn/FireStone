using UnityEngine;
using TMPro;

namespace Firestone.Core.Utilities
{
    public static class InstantiateGameObjects
    {
        public static TextMeshProUGUI CreateTMPText(string textToDisplay, string objectName, Transform parent, Vector2 localPosition)
        {
            GameObject newObj = new GameObject (objectName);
            newObj.transform.SetParent(parent);
            TextMeshProUGUI newText = newObj.AddComponent<TextMeshProUGUI>();
            newText.text = textToDisplay;
            newText.transform.localPosition = localPosition;
            return newText;
        }
    }
}
