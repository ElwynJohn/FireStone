using UnityEngine;
using TMPro;

namespace Firestone.UI
{
    public class UIGridText : UIGrid
    {
        [Header("Text Fields")]
        [SerializeField] string[] textToDisplay = default;
        [SerializeField] TMP_FontAsset font = default;
        [SerializeField] int fontSize = default;

        protected override void Awake()
        {
            base.Awake();
            InstantiateGridElementText();
        }

        public void ChangeTextToDisplay(int gridElementIndex, string textToDisplay)
        {
            gridElements[gridElementIndex].GetComponent<TextMeshProUGUI>().text = textToDisplay;
        }
        private void InstantiateGridElementText()
        {
            int textToDisplayIndex = 0;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject childObject = gameObject.transform.GetChild(i).gameObject;
                TextMeshProUGUI textComponent = childObject.AddComponent<TextMeshProUGUI>();
                textComponent.text = textToDisplay[textToDisplayIndex];
                textComponent.alignment = TextAlignmentOptions.BottomRight;
                textComponent.font = font;
                textComponent.fontSize = fontSize;
                childObject.GetComponent<RectTransform>().sizeDelta = elementSizePixels;

                if (textToDisplayIndex < textToDisplay.Length - 1)
                    textToDisplayIndex++;
            }
        }
    }
}
