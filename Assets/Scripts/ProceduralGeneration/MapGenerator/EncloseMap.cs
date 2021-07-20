using UnityEngine;



namespace Firestone.ProceduralGeneration.MapGenerator
{
    public static class EncloseMap
    {
        public static int[,] ReturnEnclosedMap(int[,] mapToEnclose)
        {
            int meshWidth = mapToEnclose.GetLength(0);
            int meshHeight = mapToEnclose.GetLength(1);
            for (int y = 0; y < meshHeight; y++)
            {
                mapToEnclose[0, y] = 0;
                mapToEnclose[meshWidth - 1, y] = 0;
            }
            for (int x = 0; x < meshWidth; x++)
            {
                mapToEnclose[x, 0] = 0;
                mapToEnclose[x, meshHeight - 1] = 0;
            }
            return mapToEnclose;
        }
    }
}