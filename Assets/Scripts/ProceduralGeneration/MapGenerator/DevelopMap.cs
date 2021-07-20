using UnityEngine;



namespace Firestone.ProceduralGeneration.MapGenerator
{
    public static class DevelopMap
    {
        public static int[,] ReturnDevelopedMap(int[,] mapToDevelop, int numRecursions, int birthLimit, int deathLimit)
        {
            int meshWidth = mapToDevelop.GetLength(0);
            int meshHeight = mapToDevelop.GetLength(1);
            int neighbours;
            BoundsInt bounds3x3 = new BoundsInt(new Vector3Int(-1, -1, 0), new Vector3Int(3, 3, 1));

            for (int thisRecursion = 0; thisRecursion < numRecursions; thisRecursion++)
            {
                int[,] newMap = new int[meshWidth, meshHeight];

                for (int x = 0; x < meshWidth; x++)
                {

                    for (int y = 0; y < meshHeight; y++)
                    {
                        neighbours = 0;
                        foreach (var pos in bounds3x3.allPositionsWithin)
                        {
                            if (pos.x == 0 && pos.y == 0) continue;

                            if (x + pos.x >= 0 && x + pos.x < meshWidth && y + pos.y >= 0 && y + pos.y < meshHeight)
                            {
                                if (mapToDevelop[x + pos.x, y + pos.y] == 1)
                                {
                                    neighbours++;
                                }
                            }
                        }

                        if (mapToDevelop[x, y] == 1)
                        {
                            if (neighbours < deathLimit)
                            {
                                newMap[x, y] = 0;
                            }
                            else
                            {
                                newMap[x, y] = 1;
                            }
                        }
                        else if (mapToDevelop[x, y] == 0)
                        {
                            if (neighbours < birthLimit)
                            {
                                newMap[x, y] = 0;
                            }
                            else
                            {
                                newMap[x, y] = 1;
                            }
                        }
                    }
                }
                mapToDevelop = newMap;
            }
            return mapToDevelop;
        }
    }
}