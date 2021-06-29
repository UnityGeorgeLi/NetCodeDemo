using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateAfter(typeof(ServerRpcReceiveSystem))]
public class ServerSpawnSystem : SystemBase
{
    //private EntityQuery m_NewConnectionQuery;
    
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameConfig>();
        //RequireForUpdate(m_NewConnectionQuery);
    }

    protected override void OnUpdate()
    {
        /* No need to do it here. ServerRpcReceiveSystem already did it for spawning player army
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var playerPrefab = GetSingleton<GameConfig>().PlayerPrefab;
        var playerDummy1Prefab = GetSingleton<GameConfig>().PlayerDummyPrefab;
        var minionPrefab = GetSingleton<GameConfig>().PlayerMinion;

        Entities.WithStoreEntityQueryInField(ref m_NewConnectionQuery)
            .WithNone<SpawnedPlayer, SpawnedMinion>()
            .ForEach((Entity connectionEntity, in NetworkIdComponent networkId) =>
            {
                var playerPosition = new Translation {Value = float3.zero};
                
                var player = ecb.Instantiate(playerPrefab);
                ecb.AddComponent(connectionEntity, new SpawnedPlayer { Value = player });
                ecb.SetComponent(player, playerPosition);

                int minionsCount = 100;
                float dist = 2.0f;
                int columns = (int)math.sqrt(minionsCount);
                if (columns * columns != minionsCount)
                {
                    ++columns;
                }

                int row = 0, col = 0;
                for (int i = 0; i < minionsCount; ++i)
                {
                    var minion = ecb.Instantiate(minionPrefab);
                    ecb.AddComponent(connectionEntity, new SpawnedMinion { PlayerEntity = player });
                    ecb.SetComponent(minion, new Translation
                    {
                        Value = new float3(playerPosition.Value.x - dist * ((columns >> 1) - col), 
                            0.0f, 
                            playerPosition.Value.z - dist * (1 + row))
                    });

                    ++col;
                    if (col >= columns)
                    {
                        col = 0;
                        ++row;
                    }
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        */
        
        //Spawn something else
        //...
    }
}
