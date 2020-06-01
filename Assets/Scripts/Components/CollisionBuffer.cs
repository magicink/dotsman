using System;
using Unity.Entities;

namespace Components
{
    public struct CollisionBuffer : IBufferElementData
    {
        public Entity Entity;
    }
}
