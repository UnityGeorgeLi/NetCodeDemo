using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class ServerRpcReceiveSystem : ComponentSystem
{
    protected override void OnCreate()
    {
        
        RequireSingletonForUpdate<GameConfig>();
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity rpcEntity, 
            ref SpawnPlayerArmyServerRPC rpcCommand, 
            ref ReceiveRpcCommandRequestComponent request) => {

                var connectionEntity = request.SourceConnection;
                if (!EntityManager.HasComponent<SpawnedPlayer>(connectionEntity))
                {
                    var ecb = new EntityCommandBuffer(Allocator.Temp);
                    var playerPrefab = GetSingleton<GameConfig>().PlayerPrefab;
                    var minionPrefab = GetSingleton<GameConfig>().PlayerMinion;

                    var dirMultiplier = (rpcCommand.Direction == 0) ? 1.0f : -1.0f;
                    var rot = dirMultiplier > 0.0f ? quaternion.identity : quaternion.EulerYXZ(0.0f,  math.PI, 0.0f);
                    
                    var playerPosition = new Translation
                        {
                            Value = new float3 { x = 0.0f, y = 0.0f, z = -dirMultiplier * 5.0f }
                        };
                    var player = ecb.Instantiate(playerPrefab);
                    var spawnedPlayer = new SpawnedPlayer {Value = player};
                    ecb.AddComponent(connectionEntity, spawnedPlayer);
                    ecb.SetComponent(player, playerPosition);
                    ecb.SetComponent(player, new Rotation{ Value = rot });
                    
                    int minionsCount = rpcCommand.MinionsCount;
                    float dist = 2.0f;
                    int columns = (int)math.sqrt(minionsCount);
                    if (columns * columns != minionsCount)
                    {
                        ++columns;
                    }

                    int row = 0, col = 0;
                    Entity prevMinion = Entity.Null;
                    for (int i = 0; i < minionsCount; ++i)
                    {
                        var minion = ecb.Instantiate(minionPrefab);
                        if (i == 0)
                        {
                            spawnedPlayer.FirstMinion = minion;
                            prevMinion = minion;
                        }
                        else
                        {
                            ecb.SetComponent(prevMinion, new SpawnedMinion() { NextMinion = minion, PlayerEntity = player } );
                            //The following doesn't work because the ecb exists. Use ecb instead 
                            //EntityManager.SetComponentData<SpawnedMinion>(prevMinion, new SpawnedMinion() { NextMinion = minion, PlayerEntity = player });
                            prevMinion = minion;
                        }
                        ecb.AddComponent(minion, new SpawnedMinion {PlayerEntity = player});
                        ecb.SetComponent(minion, new Translation
                        {
                            Value = new float3(playerPosition.Value.x - dist * ((columns >> 1) - col),
                                0.0f,
                                playerPosition.Value.z - dist * (1 + row) * dirMultiplier)
                        });
                        ecb.SetComponent(minion, new Rotation { Value = rot });

                        ++col;
                        if (col >= columns)
                        {
                            col = 0;
                            ++row;
                        }
                    }

                    ecb.Playback(EntityManager);
                    ecb.Dispose();
                }
                    
                PostUpdateCommands.DestroyEntity(rpcEntity);
        });
    }
}
