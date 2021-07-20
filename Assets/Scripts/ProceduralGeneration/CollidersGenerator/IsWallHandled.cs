using UnityEngine;



namespace Firestone.ProceduralGeneration.CollidersGenerator
{
    public static class IsWallHandled
    {
        public static bool ReturnWallHandledBool(Vector2Int colliderMapPosition, int[,] map, int sidesHandled)
        {
            int meshWidth = map.GetLength(0);
            int meshHeight = map.GetLength(1);
            int wallsRequired = 0;

            Vector2Int[] LeftUpDownRight = new Vector2Int[4]
            {
                new Vector2Int(-1,  0),
                new Vector2Int( 0,  1),
                new Vector2Int( 1,  0),
                new Vector2Int( 0, -1)
            };
            foreach (Vector2Int pos in LeftUpDownRight)
            {
                if (colliderMapPosition.x + pos.x >= 0 && colliderMapPosition.x + pos.x < meshWidth && colliderMapPosition.y + pos.y >= 0 && colliderMapPosition.y + pos.y < meshHeight &&
                    map[colliderMapPosition.x + pos.x, colliderMapPosition.y + pos.y] == 1)
                {
                    wallsRequired++;
                }
            }
            if (wallsRequired == sidesHandled)
            {
                return true;
            }
            else if (wallsRequired < sidesHandled)
            {
                Debug.LogWarning("More wall colliders handled than are required to be when generating procgen colliders. " +
                    "Number of walls required = " + wallsRequired + ". Number of walls created = " + sidesHandled + ". This warning occured at colliderMapPosition " + colliderMapPosition + 
                    ". ColliderMapDataPoint.WallHandled was set to true to avoid errors.");
                return true;
            }
            return false;
        }
    }
}
