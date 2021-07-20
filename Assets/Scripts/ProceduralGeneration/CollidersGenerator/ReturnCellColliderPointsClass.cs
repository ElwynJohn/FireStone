using UnityEngine;
using System.Collections.Generic;



namespace Firestone.ProceduralGeneration.CollidersGenerator
{
    public static class ReturnCellColliderPointsClass
    {
        public static List<Vector2Int> ReturnCellColliderPoints(Vector2Int colliderMapPosition, ref Vector2Int colliderPointPosition, int[,] map, bool firstListAdded,
            ref ColliderMapDataPoint colliderMapDataPoint)
        {
            int meshWidth = map.GetLength(0);
            int meshHeight = map.GetLength(1);
            List<Vector2Int> colliderPointsToAdd = new List<Vector2Int>();

            Vector2Int[] leftUpRightDown = new Vector2Int[4]
            {
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 1,  0),
                new Vector2Int( 0, -1)
            };
            bool[] beenInitialised = new bool[4]
            {
                false,
                false,
                false,
                false
            };


        StartOfSearch:
            for (int iteration = 0; iteration < leftUpRightDown.Length; iteration++)
            {
                foreach (Vector2Int pos in leftUpRightDown)
                {
                    if (pos.x == -1 && pos.y == 0 && (colliderMapPosition.x + pos.x) >= 0 && map[colliderMapPosition.x + pos.x, colliderMapPosition.y + pos.y] == 1)
                    {
                        if (firstListAdded)
                        {
                            AddPointIfValid(new Vector2Int(0, 0), new Vector2Int(0, 1), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);

                            AddPointIfValid(new Vector2Int(0, 1), new Vector2Int(0, 0), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);

                        }
                        else
                        {
                            colliderPointPosition = colliderMapPosition + new Vector2Int(0, 1);
                            colliderPointsToAdd.Add(colliderPointPosition);
                            firstListAdded = true;
                            goto StartOfSearch;
                        }
                    }



                    if (pos.x == 0 && pos.y == 1 && (colliderMapPosition.y + pos.y) < meshHeight && map[colliderMapPosition.x + pos.x, colliderMapPosition.y + pos.y] == 1)
                    {
                        if (firstListAdded)
                        {
                            AddPointIfValid(new Vector2Int(0, 1), new Vector2Int(1, 1), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);

                            AddPointIfValid(new Vector2Int(1, 1), new Vector2Int(0, 1), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);
                        }
                        else
                        {
                            colliderPointPosition = colliderMapPosition + new Vector2Int(1, 1);
                            colliderPointsToAdd.Add(colliderPointPosition);
                            firstListAdded = true;
                            goto StartOfSearch;
                        }
                    }



                    if (pos.x == 1 && pos.y == 0 && (colliderMapPosition.x + pos.x) < meshWidth && map[colliderMapPosition.x + pos.x, colliderMapPosition.y + pos.y] == 1)
                    {
                        if (firstListAdded)
                        {
                            AddPointIfValid(new Vector2Int(1, 1), new Vector2Int(1, 0), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);

                            AddPointIfValid(new Vector2Int(1, 0), new Vector2Int(1, 1), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);
                        }
                        else
                        {
                            colliderPointPosition = colliderMapPosition + new Vector2Int(1, 0);
                            colliderPointsToAdd.Add(colliderPointPosition);
                            firstListAdded = true;
                            goto StartOfSearch;
                        }
                    }



                    if (pos.x == 0 && pos.y == -1 && (colliderMapPosition.y + pos.y) >= 0 && map[colliderMapPosition.x + pos.x, colliderMapPosition.y + pos.y] == 1)
                    {
                        if (firstListAdded)
                        {
                            AddPointIfValid(new Vector2Int(0, 0), new Vector2Int(1, 0), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);

                            AddPointIfValid(new Vector2Int(1, 0), new Vector2Int(0, 0), colliderMapPosition, ref colliderPointsToAdd, ref colliderPointPosition, ref colliderMapDataPoint);
                        }
                        else
                        {
                            colliderPointPosition = colliderMapPosition + new Vector2Int(0, 0);
                            colliderPointsToAdd.Add(colliderPointPosition);
                            firstListAdded = true;
                            goto StartOfSearch;
                        }
                    }
                }
            }
            return colliderPointsToAdd;
        }



        private static void AddPointIfValid(Vector2Int oldPositionOffset, Vector2Int newPositionOffset, Vector2Int colliderMapPosition, 

            ref List<Vector2Int> colliderPointsToAdd, ref Vector2Int colliderPointPosition, ref ColliderMapDataPoint colliderMapDataPoint)
        {
            if (colliderMapPosition + oldPositionOffset == colliderPointPosition)
            {
                Vector2Int newColliderPointPosition = colliderMapPosition + newPositionOffset;

                if (!colliderMapDataPoint.PointsHandled.Contains(newColliderPointPosition))
                {
                    colliderPointsToAdd.Add(newColliderPointPosition);
                    colliderMapDataPoint.PointsHandled.Add(colliderPointPosition);

                    colliderPointPosition = newColliderPointPosition;
                    colliderMapDataPoint.SidesHandled++;
                    Debug.Log("colliderPointPosition: " + colliderPointPosition);
                }
            }
        }

        private static void AddPointsIfNotAlreadyAdded(Vector2Int colliderMapPosition, Vector2Int colliderPointOffset1, Vector2Int colliderPointOffset2,
            
            ref List<Vector2Int> colliderPointsToAdd, ref ColliderMapDataPoint colliderMapDataPoint)
        {
            Vector2Int newPoint1 = colliderMapPosition + colliderPointOffset1;
            Vector2Int newPoint2 = colliderMapPosition + colliderPointOffset2;

            if (!colliderMapDataPoint.PointsHandled.Contains(newPoint1))
            {
                colliderPointsToAdd.Add(newPoint1);
                colliderMapDataPoint.PointsHandled.Add(newPoint1);
            }

            if (!colliderMapDataPoint.PointsHandled.Contains(newPoint2))
            {
                colliderPointsToAdd.Add(newPoint2);
                colliderMapDataPoint.PointsHandled.Add(newPoint2);
            }
        }
    }
}
