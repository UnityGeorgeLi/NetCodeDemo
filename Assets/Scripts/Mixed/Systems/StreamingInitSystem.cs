using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class StreamingInitSystem : SystemBase
{
    private EntityQuery m_missingStreamInGame;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameConfig>();

        m_missingStreamInGame = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(NetworkIdComponent) },
            None = new ComponentType[] { typeof(NetworkStreamInGame) }
        });
        
        RequireForUpdate(m_missingStreamInGame);
    }

    protected override void OnUpdate()
    {
        EntityManager.AddComponent<NetworkStreamInGame>(m_missingStreamInGame);
    }
}
