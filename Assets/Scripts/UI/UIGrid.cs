using UnityEngine;

namespace Firestone.UI
{
    public abstract class UIGrid : MonoBehaviour
    {
        [SerializeField] protected string gridElementName = "grid element";
        [SerializeField] protected int rowCount = 0;
        [SerializeField] protected int columnCount = 0;
        [Header("Grid Spacing")]
        [SerializeField] protected Vector2 position = default;
        [SerializeField] public Vector2 elementSizePixels = default;
        [SerializeField] protected int elementPaddingPixelsBetweenRows = 0;
        [SerializeField] protected int elementPaddingPixelsBetweenColumns = 0;

        public GameObject[] gridElements { get; protected set; }
        protected Vector2 totalGridSize { get; set; }

        protected virtual void Awake()
        {
            totalGridSize = new Vector2(elementSizePixels.x * columnCount + elementPaddingPixelsBetweenRows * (columnCount - 1),
                elementSizePixels.y * rowCount + elementPaddingPixelsBetweenRows * (rowCount - 1));
            gridElements = new GameObject[rowCount * columnCount];
            InstantiateGridElements();
            gameObject.transform.localPosition = CalcualteScreenPosition();
        }

        public void DisplayRangeOfGridElements(bool displayGridElements, int inclusiveStartIndex, int exclusiveEndIndex)
        {
            if (inclusiveStartIndex > gridElements.Length)
            {
                Debug.LogError("UIGrid range out of bounds");
                return;
            }
            for (int i = inclusiveStartIndex; i < exclusiveEndIndex && i < gridElements.Length; i++)
            {
                gridElements[i].SetActive(displayGridElements);
            }
        }

        protected void InstantiateGridElements()
        {
            int index = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    //instantiate prefab instead
                    gridElements[index] = new GameObject();
                    gridElements[index].name = gridElementName + index;
                    gridElements[index].transform.parent = gameObject.transform;
                    gridElements[index].layer = LayerMask.NameToLayer("UI");
                    RectTransform rect = gridElements[index].AddComponent<RectTransform>();
                    rect.sizeDelta = elementSizePixels;
                    gridElements[index].transform.localPosition = CalculateGridElementLocalScreenPosition(index);
                    index++;
                }
            }
        }
        protected Vector2 CalcualteScreenPosition()
        {
            Vector2 viewPortPoint = new Vector2(position.x, position.y);
            Vector2 screenPoint = Camera.main.ViewportToScreenPoint(viewPortPoint);
            return screenPoint;
        }
        protected Vector2 CalculateGridElementLocalScreenPosition(int indexOfGridElement)
        {
            Vector2 screenPoint = new Vector2( (elementSizePixels.x + elementPaddingPixelsBetweenRows) * (indexOfGridElement % columnCount),
                (elementSizePixels.y + elementPaddingPixelsBetweenColumns) * (indexOfGridElement / columnCount));

            Vector2 offsetToCentreGrid = elementSizePixels / 2 - totalGridSize / 2;
            screenPoint += offsetToCentreGrid;
            return screenPoint;
        }
    }
}
