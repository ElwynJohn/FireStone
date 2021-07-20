using UnityEngine;
using System;

namespace Firestone.UI
{
    public class HoverableColliders
    {
        public HoverableColliders(RectTransform[] rectTransforms)
        {
            colliders = new BoxCollider2D[rectTransforms.Length];
            for (int i = 0; i < rectTransforms.Length; i++)
            {
                colliders[i] = rectTransforms[i].gameObject.AddComponent<BoxCollider2D>();
                colliders[i].size = rectTransforms[i].sizeDelta;
            }
        }

        private BoxCollider2D[] colliders;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>-1 if no collider is hovered</returns>
        public int IndexOfColliderHovered()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.z;
            Ray ray = new Ray(mousePosition, new Vector3(0, 0, 1));
            RaycastHit2D rayHitData = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI"));
            if (rayHitData)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] == rayHitData.collider)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
