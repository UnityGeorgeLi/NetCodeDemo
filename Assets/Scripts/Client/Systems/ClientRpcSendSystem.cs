using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ClientRpcSendSystem : ComponentSystem
{
    private bool _hasSpawnedPlayerArmy = false;
    
    protected override void OnCreate()
    {
        _hasSpawnedPlayerArmy = false;
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        if (!_hasSpawnedPlayerArmy)
        {
            var request = PostUpdateCommands.CreateEntity();
            var rpc = new SpawnPlayerArmyServerRPC
            {
                PlayerCharacter = 0,
                Direction = (ushort)ClientConfig.Direction,
                MinionsCount = (ushort) ClientConfig.MinionsCount
            };
            PostUpdateCommands.AddComponent(request, rpc);
            PostUpdateCommands.AddComponent(request, new SendRpcCommandRequestComponent());
            _hasSpawnedPlayerArmy = true;
        }
        
        
        //Other RPCs
    }
}
