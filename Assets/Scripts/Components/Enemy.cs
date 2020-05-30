using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Enemy : IComponentData
    {
        public float3 PreviousCell;
    }
}