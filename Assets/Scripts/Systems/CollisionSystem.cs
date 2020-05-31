using Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    public class CollisionSystem : SystemBase
    {
        private struct CollisionSystemJob : ICollisionEventsJob
        {
            public BufferFromEntity<CollisionBuffer> Collisions;

            public void Execute(CollisionEvent collisionEvent)
            {
                if (Collisions.Exists(collisionEvent.Entities.EntityA))
                    Collisions[collisionEvent.Entities.EntityA].Add(new CollisionBuffer
                        {Entity = collisionEvent.Entities.EntityB});
                if (Collisions.Exists(collisionEvent.Entities.EntityB))
                    Collisions[collisionEvent.Entities.EntityB].Add(new CollisionBuffer
                        {Entity = collisionEvent.Entities.EntityA});
            }
        }

        private struct TriggerEventsJob : ITriggerEventsJob
        {
            public BufferFromEntity<TriggerBuffer> Collisions;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (Collisions.Exists(triggerEvent.Entities.EntityA))
                    Collisions[triggerEvent.Entities.EntityA].Add(new TriggerBuffer
                    {
                        Entity = triggerEvent.Entities.EntityB
                    });
                if (Collisions.Exists(triggerEvent.Entities.EntityB))
                    Collisions[triggerEvent.Entities.EntityB].Add(new TriggerBuffer
                    {
                        Entity = triggerEvent.Entities.EntityA
                    });
            }
        }

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
    }
}