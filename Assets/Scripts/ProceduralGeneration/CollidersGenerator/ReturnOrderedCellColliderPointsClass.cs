using System.Collections.Generic;
using UnityEngine;



namespace Firestone.ProceduralGeneration.CollidersGenerator
{
    public static class ReturnOrderedCellColliderPointsClass
    {
        public static List<Vector2Int> ReturnOrderedCellColliderPoints(ref List<Vector2Int> colliderPointsToAdd)
        {
            int inder = 0;
        AddFirstList:
            inder++;
            List<Vector2Int> orderedColliderPoints = new List<Vector2Int>();
            Vector2Int colliderPointPosition = colliderPointsToAdd[0];
            orderedColliderPoints.Add(colliderPointPosition);

            foreach (Vector2Int colliderPoint in colliderPointsToAdd)
            {
                if (colliderPoint != colliderPointPosition &&
                    colliderPoint.x == colliderPointPosition.x && colliderPoint.y != colliderPointPosition.y ||
                    colliderPoint.x != colliderPointPosition.x && colliderPoint.y == colliderPointPosition.y)
                {
                    colliderPointPosition = colliderPoint;
                    orderedColliderPoints.Add(colliderPointPosition);
                }
            }


            if (orderedColliderPoints.Count == colliderPointsToAdd.Count)
            {
                if (orderedColliderPoints.Count == 4)
                {
                    orderedColliderPoints.Add(orderedColliderPoints[0]);
                }
            }
            else
            {
                if (colliderPointsToAdd.Count == 4)
                {
                    colliderPointsToAdd.Insert(0, colliderPointsToAdd[colliderPointsToAdd.Count - 2]);
                    colliderPointsToAdd.RemoveAt(colliderPointsToAdd.Count - 2);
                }
                else
                {
                    colliderPointsToAdd.Insert(0, colliderPointsToAdd[colliderPointsToAdd.Count - 1]);
                    colliderPointsToAdd.RemoveAt(colliderPointsToAdd.Count - 1);
                }

                if (inder > 10)
                {
                    Debug.LogWarning("Skipped adding first list to collider:");
                    foreach (var colliderPoint in colliderPointsToAdd)
                    {
                        Debug.Log(colliderPoint);
                    }
                    goto Skip;
                }
                goto AddFirstList;
            }
        Skip:;
            return orderedColliderPoints;
        }
    }
}
