using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ClientInputInitSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        var connectionEntity = GetSingletonEntity<NetworkIdComponent>();
        var networkId = GetComponent<NetworkIdComponent>(connectionEntity).Value;
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        bool found = false;

        Entities
            .ForEach((Entity ghostEntity, in GhostOwnerComponent owner) =>
            {
                if (owner.NetworkId == networkId)
                {
                    found = true;

                    ecb.AddBuffer<PlayerInput>(ghostEntity);
                    ecb.SetComponent(connectionEntity, new CommandTargetComponent
                    {
                        targetEntity = ghostEntity
                    });
                    ecb.SetName(ghostEntity, "Player (local)");
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        if (found)
        {
            Enabled = false;
        }
    }
}
