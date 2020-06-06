using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Damage : IComponentData
    {
        public float Value;
    }
}
