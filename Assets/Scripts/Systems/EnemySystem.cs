using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class EnemySystem : SystemBase
    {
        private Unity.Mathematics.Random _random = new Unity.Mathematics.Random(8138);
        
        protected override void OnUpdate()
        {
            var randomResult = _random;
            _random.NextInt();
            var rayCaster = new MovementRayCast()
            {
                PhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld
            };
            Entities.ForEach((ref Enemy enemy, ref Movable movable, in Translation translation) => {
                if (math.distance(translation.Value, enemy.PreviousCell) > 0.9f)
                {
                    enemy.PreviousCell = math.round(translation.Value);
                    var validDirections = new NativeList<float3>(Allocator.Temp);

                    var north = new float3(0, 0, 1);
                    var south = new float3(0, 0, -1);
                    var east = new float3(1, 0, 0);
                    var west = new float3(-1, 0, 0);

                    if (!rayCaster.CheckRay(translation.Value, north, movable.Direction))
                        validDirections.Add(north);
                    if (!rayCaster.CheckRay(translation.Value, south, movable.Direction))
                        validDirections.Add(south);
                    if (!rayCaster.CheckRay(translation.Value, east, movable.Direction))
                        validDirections.Add(east);
                    if (!rayCaster.CheckRay(translation.Value, west, movable.Direction))
                        validDirections.Add(west);

                    if (validDirections.Length > 0)
                       movable.Direction = validDirections[randomResult.NextInt(validDirections.Length)];
                    
                    validDirections.Dispose();
                }
                
            }).Schedule();
        }
        
        private struct MovementRayCast
        {
            [ReadOnly] public PhysicsWorld PhysicsWorld;
            public bool CheckRay(float3 position, float3 direction, float3 currentDirection)
            {
                if (direction.Equals(-currentDirection)) return true;
                var ray = new RaycastInput
                {
                    Start = position,
                    End = position + direction * 0.9f,
                    Filter = new CollisionFilter
                    {
                        GroupIndex = 0,
                        BelongsTo = 1u << 1,
                        CollidesWith = 1u << 2
                    }
                };
                return PhysicsWorld.CastRay(ray);
            }
        }
    }
}
