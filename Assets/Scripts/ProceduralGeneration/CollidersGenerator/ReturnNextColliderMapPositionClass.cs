using System.Collections.Generic;
using UnityEngine;



namespace Firestone.ProceduralGeneration.CollidersGenerator
{
    public static class ReturnNextColliderMapPositionClass
    {
        public static Vector2Int ReturnNextColliderMapPosition(Vector2Int colliderMapPosition, Vector2Int colliderPointPosition, ColliderMapDataPoint[,] colliderMap, int[,] map)
        {
            int meshWidth = colliderMap.GetLength(0);
            int meshHeight = colliderMap.GetLength(1);

            BoundsInt myBoundsInt2x2 = new BoundsInt(new Vector3Int(-1, -1, 0), new Vector3Int(2, 2, 1));
            Vector2Int[] leftUpRightDown = new Vector2Int[4]
            {
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 1,  0),
                new Vector2Int( 0, -1)
            };
            Vector2Int noVectorFound = new Vector2Int(-1, -1);



            foreach (Vector2Int colliderMapPos in myBoundsInt2x2.allPositionsWithin)
            {
                if (colliderPointPosition.x + colliderMapPos.x >= 0 && colliderPointPosition.y + colliderMapPos.y >= 0 &&
                    colliderPointPosition.x + colliderMapPos.x < meshWidth && colliderPointPosition.y + colliderMapPos.y < meshHeight &&

                    colliderMap[colliderPointPosition.x + colliderMapPos.x, colliderPointPosition.y + colliderMapPos.y].Wall == true &&
                    colliderMap[colliderPointPosition.x + colliderMapPos.x, colliderPointPosition.y + colliderMapPos.y].WallHandled == false)
                {
                    foreach (Vector2Int mapPos in leftUpRightDown)
                    {
                        if (colliderMapPosition != colliderPointPosition + colliderMapPos)
                        {

                            if (mapPos.x == -1 && mapPos.y == 0 && (colliderPointPosition.x + colliderMapPos.x + mapPos.x) >= 0 &&
                                map[colliderPointPosition.x + colliderMapPos.x + mapPos.x, colliderPointPosition.y + colliderMapPos.y + mapPos.y] == 1)
                            {
                                if (colliderPointPosition + colliderMapPos + new Vector2Int(0, 0) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                                else if (colliderPointPosition + colliderMapPos + new Vector2Int(0, 1) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                            }



                            if (mapPos.x == 0 && mapPos.y == 1 && (colliderPointPosition.y + colliderMapPos.y + mapPos.y) < meshHeight &&
                                map[colliderPointPosition.x + colliderMapPos.x + mapPos.x, colliderPointPosition.y + colliderMapPos.y + mapPos.y] == 1)
                            {
                                if (colliderPointPosition + colliderMapPos + new Vector2Int(0, 1) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                                else if (colliderPointPosition + colliderMapPos + new Vector2Int(1, 1) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                            }



                            if (mapPos.x == 1 && mapPos.y == 0 && (colliderPointPosition.x + colliderMapPos.x + mapPos.x) < meshWidth &&
                                map[colliderPointPosition.x + colliderMapPos.x + mapPos.x, colliderPointPosition.y + colliderMapPos.y + mapPos.y] == 1)
                            {
                                if (colliderPointPosition + colliderMapPos + new Vector2Int(1, 1) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                                else if (colliderPointPosition + colliderMapPos + new Vector2Int(1, 0) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                            }



                            if (mapPos.x == 0 && mapPos.y == -1 && (colliderPointPosition.y + colliderMapPos.y + mapPos.y) >= 0 &&
                                map[colliderPointPosition.x + colliderMapPos.x + mapPos.x, colliderPointPosition.y + colliderMapPos.y + mapPos.y] == 1)
                            {
                                if (colliderPointPosition + colliderMapPos + new Vector2Int(0, 0) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                                else if (colliderPointPosition + colliderMapPos + new Vector2Int(1, 0) == colliderPointPosition)
                                {
                                    return colliderPointPosition + colliderMapPos;
                                }
                            }
                        }
                    }
                }
            }
            return noVectorFound;
        }
    }
}