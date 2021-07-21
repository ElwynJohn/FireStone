using UnityEngine;

namespace Firestone.Core
{
    [CreateAssetMenu(fileName = "New GameObjectData", menuName = "GameObjectData")]
    public class GameObjectData : ScriptableObject
    {
        public ItemID gameID;

        public string displayedName;
        public Sprite icon;

        //data for placing GameObject
        [Header("Placement Data")]
        public bool CanBePlaced;
        public Sprite Sprite;
        public GameObject PlaceablePrefab;
        public ParticleSystem PlacementPS; //Particles for placed object

        //data for gathering GameObject
        [Header("Gather Data")]
        public float TimeToGather; //how long it takes to collect this GameObject
        public int AmountDropped;
        public int AmountDroppedDeviation; // can drop an amount between AmountDropped +-1 
        public GameObject PickUpPrefab; // prefab for dropped items that can be picked up
        public Vector3 dropPositionOffset; //local position to drop the object

        [Header("Item Drop Dynamics")]
        public bool SpawnInIdleState = true;
        public float DistanceToDrop;
        public float DistanceToDropDeviation;
        public float DropSpeed = 5f;
        public float DropDecelleration = 20f;


		public override string ToString() => gameID.ToString();
		public static bool IsAnItem(ItemID id) => id != ItemID.NotAnItem;
    }
}
