using UnityEngine;

namespace Firestone.Gather
{
	[System.Serializable]
	public struct Drop
	{
		public bool SpawnInIdleState;
		public Vector3 StartPos;
		public float StartPosDeviation;
		public float DistanceToDrop;
        public float DistanceToDropDeviation;
		public float Decelleration;
		public float Speed;
	}
}
