using Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    public class CollisionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
            var simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

            Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer) => { collisionBuffer.Clear(); }).Run();
            Entities.ForEach((DynamicBuffer<TriggerBuffer> triggerBuffer) => { triggerBuffer.Clear(); }).Run();

            var collisionSystemJob = new CollisionSystemJob
            {
                Collisions = GetBufferFromEntity<CollisionBuffer>()
            }.Schedule(simulation, ref physicsWorld, Dependency);
            collisionSystemJob.Complete();

            var triggerEventsJob = new TriggerEventsJob()
            {
                Collisions = GetBufferFromEntity<TriggerBuffer>()
            }.Schedule(simulation, ref physicsWorld, Dependency);
            triggerEventsJob.Complete();
        }

        private struct CollisionSystemJob : ICollisionEventsJob
        {
            public BufferFromEntity<CollisionBuffer> Collisions;

            public void Execute(CollisionEvent collisionEvent)
            {
                if (Collisions.HasComponent(collisionEvent.EntityA))
                    Collisions[collisionEvent.EntityA].Add(new CollisionBuffer
                        {Entity = collisionEvent.EntityB});
                if (Collisions.HasComponent(collisionEvent.EntityB))
                    Collisions[collisionEvent.EntityB].Add(new CollisionBuffer
                        {Entity = collisionEvent.EntityA});
            }
        }

        private struct TriggerEventsJob : ITriggerEventsJob
        {
            public BufferFromEntity<TriggerBuffer> Collisions;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (Collisions.HasComponent(triggerEvent.EntityA))
                    Collisions[triggerEvent.EntityA].Add(new TriggerBuffer
                    {
                        Entity = triggerEvent.EntityB
                    });
                if (Collisions.HasComponent(triggerEvent.EntityB))
                    Collisions[triggerEvent.EntityB].Add(new TriggerBuffer
                    {
                        Entity = triggerEvent.EntityA
                    });
            }
        }
    }
}