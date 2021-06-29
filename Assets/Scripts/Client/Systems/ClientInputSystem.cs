using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
public class ClientInputSystem : SystemBase
{
    private ClientSimulationSystemGroup m_ClientSimulationSystemGroup;

    protected override void OnCreate()
    {
        m_ClientSimulationSystemGroup = World.GetExistingSystem<ClientSimulationSystemGroup>();
    }

    protected override void OnUpdate()
    {
        var targetEntity = GetSingleton<CommandTargetComponent>().targetEntity;

        if (targetEntity == Entity.Null)
        {
            return;
        }

        var inputBuffer = GetBuffer<PlayerInput>(targetEntity);

        inputBuffer.AddCommandData(new PlayerInput
        {
            Tick = m_ClientSimulationSystemGroup.ServerTick,
            Movement = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        });
    }
}
