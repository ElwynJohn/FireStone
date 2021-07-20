using UnityEngine;



namespace Firestone.ProceduralGeneration.MeshGenerator
{
    public static class CreateTileMesh
    {
        public static Mesh ReturnTileMesh(int[,] map, Vector2Int[] tilePositions, int tileSize, Vector2 textureSize)
        {
            int meshWidth = map.GetLength(0);
            int meshHeight = map.GetLength(1);

            Vector3[] vertices = new Vector3[4 * meshHeight * meshWidth];
            Vector2[] uv = new Vector2[4 * meshHeight * meshWidth];
            int[] triangles = new int[6 * meshHeight * meshWidth];

            Mesh mesh = new Mesh();



            for (int x = 0; x < meshWidth; x++)
            {
                for (int y = 0; y < meshHeight; y++)
                {
                    int index = x * meshHeight + y;

                    vertices[index * 4 + 0] = new Vector3(x + 0, y + 0);
                    vertices[index * 4 + 1] = new Vector3(x + 0, y + 1);
                    vertices[index * 4 + 2] = new Vector3(x + 1, y + 1);
                    vertices[index * 4 + 3] = new Vector3(x + 1, y + 0);

                    uv[index * 4 + 0] = PixelsToUVCoords(tilePositions[map[x, y]], textureSize);
                    uv[index * 4 + 1] = PixelsToUVCoords(tilePositions[map[x, y]] + new Vector2Int(0, tileSize), textureSize);
                    uv[index * 4 + 2] = PixelsToUVCoords(tilePositions[map[x, y]] + new Vector2Int(tileSize, tileSize), textureSize);
                    uv[index * 4 + 3] = PixelsToUVCoords(tilePositions[map[x, y]] + new Vector2Int(tileSize, 0), textureSize);

                    triangles[index * 6 + 0] = index * 4 + 0;
                    triangles[index * 6 + 1] = index * 4 + 1;
                    triangles[index * 6 + 2] = index * 4 + 2;

                    triangles[index * 6 + 3] = index * 4 + 0;
                    triangles[index * 6 + 4] = index * 4 + 2;
                    triangles[index * 6 + 5] = index * 4 + 3;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }



        private static Vector2 PixelsToUVCoords(Vector2 UVPixelPoint, Vector2 textureSize)
        {
            return new Vector2(UVPixelPoint.x / textureSize.x, UVPixelPoint.y / textureSize.y);
        }
    }
}