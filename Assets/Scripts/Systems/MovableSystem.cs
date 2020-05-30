using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MovableSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        
        Entities.ForEach((in Movable movable) =>
        {
            var step = movable.direction * movable.speed;
        }).Schedule();
    }
}
