using System;
using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct CollisionBuffer : IBufferElementData
    {
        public Entity Entity;
    }
}
