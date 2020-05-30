using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    public class PlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            Entities.WithAll<Player>().ForEach((ref Movable movable) => {
                movable.Direction = new float3(x, 0, y);
            }).Schedule();
        }
    }
}
