using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    public struct Kill : IComponentData
    {
        public float value;
    }
}
