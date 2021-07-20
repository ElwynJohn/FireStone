using UnityEngine;
using System.Collections.Generic;

namespace Firestone.ProceduralGeneration.CollidersGenerator
{
    public struct ColliderMapDataPoint
    {
        public ColliderMapDataPoint(bool wall, bool wallHandled, int sidesHandled)
        {
            Wall = wall;
            WallHandled = wallHandled;
            SidesHandled = sidesHandled;
            PointsHandled = new List<Vector2Int>();
        }
        public bool Wall;
        public bool WallHandled;
        public int SidesHandled;
        public List<Vector2Int> PointsHandled;
    }



    public static class HandleColliders
    {
        public static void ReturnColliderPoints(int[,] map, GameObject gameObject, ref List<List<Vector2>> debugListOfColliderLists)
        {
            Vector2Int[] leftUpRightDown = new Vector2Int[4]
            {
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 1,  0),
                new Vector2Int( 0, -1)
            };
            Vector2Int noVectorFound = new Vector2Int(-1, -1);

            int meshWidth = map.GetLength(0);
            int meshHeight = map.GetLength(1);
            ColliderMapDataPoint[,] colliderMap = new ColliderMapDataPoint[meshWidth, meshHeight];



            for (int x = 0; x < meshWidth; x++)
            {
                for (int y = 0; y < meshHeight; y++)
                {
                    colliderMap[x, y] = new ColliderMapDataPoint(false, false, 0);
                    if (map[x, y] == 0)
                    {
                        foreach (var pos in leftUpRightDown)
                        {
                            if (x + pos.x >= 0 && x + pos.x < meshWidth && y + pos.y >= 0 && y + pos.y < meshHeight)
                            {
                                if (map[x + pos.x, y + pos.y] == 1)
                                {
                                    colliderMap[x, y] = new ColliderMapDataPoint(true, false, 0);
                                }
                            }
                        }
                    }
                }
            }



            bool allColliderPointsFound = false;
            int index1 = 0;

            while (!allColliderPointsFound && index1 < 15)
            {
                index1++;
                Debug.Log("new collider: " + index1);
                allColliderPointsFound = true;


                //create starting points for colliderPointPosition and colliderMapPosition
                bool breakLoops = false;
                Vector2Int colliderPointPosition = Vector2Int.zero;
                Vector2Int colliderMapPosition = Vector2Int.zero;

                for (int x = 0; x < meshWidth; x++)
                {
                    for (int y = 0; y < meshHeight; y++)
                    {
                        if (colliderMap[x, y].Wall == true && colliderMap[x, y].WallHandled == false)
                        {
                            colliderMapPosition = new Vector2Int(x, y);

                            breakLoops = true;
                            allColliderPointsFound = false;
                            break;
                        }
                    }

                    if (breakLoops)
                    {
                        break;
                    }
                }
                if (allColliderPointsFound)
                {
                    break;
                }



                List<Vector2Int> colliderPointsListInt = new List<Vector2Int>();
                bool firstListAdded = false;

                bool colliderMapPositionFound = true;
                int index2 = 0;

                while (colliderMapPositionFound && index2 < 220)
                {
                    Debug.Log("colliderMapPosition: " + colliderMapPosition);
                    index2++;

                    List<Vector2Int> colliderPointsToAdd = ReturnCellColliderPointsClass.ReturnCellColliderPoints

                        (colliderMapPosition, ref colliderPointPosition, map, firstListAdded, ref colliderMap[colliderMapPosition.x, colliderMapPosition.y]);

                    firstListAdded = true;

                    colliderPointsListInt.AddRange(colliderPointsToAdd);



                    colliderMap[colliderMapPosition.x, colliderMapPosition.y].WallHandled = 
                        IsWallHandled.ReturnWallHandledBool(colliderMapPosition, map, colliderMap[colliderMapPosition.x, colliderMapPosition.y].SidesHandled);

                    colliderMapPosition = ReturnNextColliderMapPositionClass.ReturnNextColliderMapPosition(colliderMapPosition, colliderPointPosition, colliderMap, map);
                    if (colliderMapPosition == noVectorFound)
                    {
                        colliderMapPositionFound = false;
                    }
                }



                colliderPointsListInt.Add(colliderPointsListInt[0]);
                List<Vector2> colliderPointsList = new List<Vector2>();
                foreach (Vector2Int colliderPoint in colliderPointsListInt)
                {
                    colliderPointsList.Add(colliderPoint);
                }
                Vector2[] colliderPoints = colliderPointsList.ToArray();


                EdgeCollider2D myEdgeCollider = gameObject.AddComponent<EdgeCollider2D>();
                myEdgeCollider.points = colliderPoints;

                debugListOfColliderLists.Add(colliderPointsList);
            }
        }
    }
}
    