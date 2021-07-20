using UnityEngine;
using Firestone.Core.Utilities;
using Firestone.ProceduralGeneration.MapGenerator;
using Firestone.ProceduralGeneration.CollidersGenerator;
using Firestone.ProceduralGeneration.MeshGenerator;
using TMPro;
using System.Collections.Generic;
using System.Data;

namespace Firestone.ProceduralGeneration
{
    public class ProceduralGenerator : MonoBehaviour
    {
        [SerializeField] int meshHeight = 0;
        [SerializeField] int meshWidth = 0;

        [Range(0, 100)]
        [SerializeField] int initChance = 50;
        [Range(0, 8)]
        [SerializeField] int birthLimit = 2;
        [Range(0, 8)]
        [SerializeField] int deathLimit = 2;
        [Range(0, 8)]
        [SerializeField] int numRecursions = 2;

        [SerializeField] Canvas testCanvas = default;
        private float timer = 0f;
        private bool timing = false;
        TextMeshProUGUI timerText;

        [SerializeField] bool debugDrawColliders = true;
        List<List<Vector2>> debugListOfColliderLists;



        int[,] map;
        public Vector2Int test = new Vector2Int(23, 12);
        [SerializeField] Vector2 textureSize = new Vector2(324, 180);
        const int tileSize = 36;
        [SerializeField] Vector2Int[] tilePositions = new Vector2Int[]
        {
        new Vector2Int (180, 108), //dark grass
        new Vector2Int (36, 108), //light grass
        new Vector2Int (0, 0) //other
        };

        GameObject colliderGameObject;



        private void Start()
        {
            timerText = InstantiateGameObjects.CreateTMPText("000", "timer test", testCanvas.transform, new Vector2(800, 450));
            debugListOfColliderLists = new List<List<Vector2>>();



            map = InitializeNewMap.ReturnNewMap(meshWidth, meshHeight, initChance);
            map = DevelopMap.ReturnDevelopedMap(map, numRecursions, birthLimit, deathLimit);
            map = EncloseMap.ReturnEnclosedMap(map);
            map = RuleTileLogic.ReturnUVMap(map, tilePositions);
            Debug.Log(map[0, 0]);


            GetComponent<MeshFilter>().mesh = CreateTileMesh.ReturnTileMesh(map, tilePositions, tileSize, textureSize);

            colliderGameObject = new GameObject();
            colliderGameObject.transform.parent = gameObject.transform;
            HandleColliders.ReturnColliderPoints(map, colliderGameObject, ref debugListOfColliderLists);
        }

        private void Update()
        {

            if (timing)
            {
                timer = Time.timeSinceLevelLoad - timer;
                timerText.text = "time to DevelopMap: " + timer.ToString();
                timing = false;
                timer = 0f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                timer = Time.timeSinceLevelLoad;
                timing = true;

                map = DevelopMap.ReturnDevelopedMap(map, numRecursions, birthLimit, deathLimit);
                map = EncloseMap.ReturnEnclosedMap(map);


                GetComponent<MeshFilter>().mesh = CreateTileMesh.ReturnTileMesh(map, tilePositions, tileSize, textureSize);
                Destroy(colliderGameObject);
                colliderGameObject = new GameObject("Wall Colliders");
                colliderGameObject.transform.parent = gameObject.transform;
                debugListOfColliderLists.Clear();
                HandleColliders.ReturnColliderPoints(map, colliderGameObject, ref debugListOfColliderLists);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                map = InitializeNewMap.ReturnNewMap(meshWidth, meshHeight, initChance);
                GetComponent<MeshFilter>().mesh = CreateTileMesh.ReturnTileMesh(map, tilePositions, tileSize, textureSize);
            }


            if (debugDrawColliders)
            {
                foreach (List<Vector2> colliderList in debugListOfColliderLists)
                {
                    Vector2 lastColliderPoint = colliderList[0];
                    foreach (Vector2 colliderPoint in colliderList)
                    {
                        Debug.DrawLine(lastColliderPoint, colliderPoint, Color.white, 0f, false);
                        lastColliderPoint = colliderPoint;
                    }
                }
            }
        }
    }
}