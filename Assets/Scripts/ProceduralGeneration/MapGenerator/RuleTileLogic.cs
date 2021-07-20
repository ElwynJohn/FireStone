using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firestone.ProceduralGeneration
{
    public static class RuleTileLogic
    {
        public static int[,] ReturnUVMap(int[,] map, Vector2Int[] tiles)
        {
            int[,] newMap = map;
            BoundsInt bounds3x3 = new BoundsInt(new Vector3Int(-1, -1, 0), new Vector3Int(3, 3, 1));
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    foreach (Vector2Int pos in bounds3x3.allPositionsWithin)
                    {
                        
                    }
                }
            }
            return newMap;

            
        }
    }
}