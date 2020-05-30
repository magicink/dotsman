using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Movable : IComponentData
    {
        public float Speed;
        public float3 Direction;
    }
}
