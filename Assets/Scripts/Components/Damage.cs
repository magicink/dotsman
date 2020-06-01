using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    public struct Damage : IComponentData
    {
        public float value;
    }
}
