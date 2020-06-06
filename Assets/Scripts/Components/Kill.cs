using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Kill : IComponentData
    {
        public float Timer;
    }
}
