using Components;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            
            Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer, ref Health health) => {
                for (var i = 0; i < collisionBuffer.Length; i++)
                {
                    if (!HasComponent<Damage>(collisionBuffer[i].Entity) || health.InvincibleTimer > 0) continue;
                    health.Value -= GetComponent<Damage>(collisionBuffer[i].Entity).Value;
                    health.InvincibleTimer = 1;
                }
            }).Schedule();

            Entities
                .WithNone<Kill>()
                .ForEach((Entity entity, ref Health health) =>
            {
                health.InvincibleTimer -= deltaTime;
                if (health.Value <= 0)
                {
                    EntityManager.AddComponentData(entity, new Kill {Timer = health.KillTimer} );
                }
            }).WithStructuralChanges().Run();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Kill kill) =>
            {
                kill.Timer -= deltaTime;
                if (kill.Timer <= 0)
                {
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }
            }).Schedule();
            
            entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
