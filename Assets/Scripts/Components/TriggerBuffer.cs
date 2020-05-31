using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct TriggerBuffer : IBufferElementData
    {
        public Entity Entity;
    }
}
