using UnityEngine;
using UnityEngine.UI;

namespace Firestone.UI
{
    public class UIGridImage : UIGrid
    {
        [Header ("Image Fields")]
        [SerializeField] private Sprite[] sprites = default;

        public void ChangeGridElementSprite(int element, Sprite sprite)
        {
            Image childImage = gameObject.transform.GetChild(element).gameObject.GetComponent<Image>();
            childImage.sprite = sprite;
        }
        public void ChangeGridElementColor(int element, Color newColor)
        {
            gridElements[element].GetComponent<Image>().color = newColor;
        }

        protected override void Awake()
        {
            base.Awake();
            InstantiateGridElementImages();
        }

        protected void InstantiateGridElementImages()
        {
            int spritesIndex = 0;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject childGameObject = gameObject.transform.GetChild(i).gameObject;
                Image img = childGameObject.AddComponent(typeof(Image)) as Image;
                img.sprite = sprites[spritesIndex];

                if (spritesIndex < sprites.Length - 1)
                    spritesIndex++;
            }
        }
    }
}
