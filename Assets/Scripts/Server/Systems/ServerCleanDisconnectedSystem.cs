using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public class ServerCleanDisconnectedSystem : SystemBase
{
    private EntityQuery m_ClosedConnections;

    protected override void OnCreate()
    {
        m_ClosedConnections = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] {typeof(SpawnedPlayer)},
            None = new ComponentType[] {typeof(NetworkIdComponent)}
        });
    }

    protected override void OnUpdate()
    {
        var players = m_ClosedConnections.ToComponentDataArray<SpawnedPlayer>(Allocator.Temp);
        EntityManager.DestroyEntity(players.Reinterpret<Entity>());
        EntityManager.RemoveComponent<SpawnedPlayer>(m_ClosedConnections);
    }
}
