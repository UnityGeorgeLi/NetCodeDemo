using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

[UpdateInGroup(typeof(ServerInitializationSystemGroup))]
public class ServerInitSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ep = NetworkEndPoint.AnyIpv4.WithPort(7979);
        World.GetExistingSystem<NetworkStreamReceiveSystem>().Listen(ep);
        Enabled = false;
    }
}
