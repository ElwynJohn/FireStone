using UnityEngine;



namespace Firestone.ProceduralGeneration.MapGenerator
{
    public static class InitializeNewMap
    {
        public static int[,] ReturnNewMap(int meshWidth, int meshHeight, int initChance)
        {
            int[,] map = new int[meshWidth, meshHeight];
            for (int x = 0; x < meshWidth; x++)
            {
                for (int y = 0; y < meshHeight; y++)
                {
                    if (Random.Range(0, 100) <= initChance)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }
            return map;
        }
    }
}