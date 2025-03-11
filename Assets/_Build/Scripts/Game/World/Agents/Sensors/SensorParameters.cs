using UnityEngine;

namespace LostKaiju.Game.World.Agents.Sensors
{    
    [System.Serializable]
    public class SensorParameters
    {
        [field: SerializeField] public Transform RayOrigin { get; private set; }
        [field: SerializeField] public Vector3 RayOffset { get; private set; }
        [field: SerializeField] public Vector3 RandomOffset { get; private set; }
        [field: SerializeField] public float RayDistance { get; private set; }
        [field: SerializeField] public float RayCooldown { get; private set; }
        [field: SerializeField] public LayerMask RayMask { get; private set; }
    }
}