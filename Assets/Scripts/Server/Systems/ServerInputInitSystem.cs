using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class ServerInputInitSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithChangeFilter<CommandTargetComponent>()
            .ForEach((Entity connectionEntity
                , ref CommandTargetComponent commandTarget
                , in NetworkIdComponent networkId
                , in SpawnedPlayer player) =>
            {
                if (commandTarget.targetEntity == Entity.Null)
                {
                    ecb.AddBuffer<PlayerInput>(player.Value);
                    ecb.SetComponent(player.Value, new GhostOwnerComponent
                    {
                        NetworkId = networkId.Value
                    });

                    commandTarget.targetEntity = player.Value;
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
