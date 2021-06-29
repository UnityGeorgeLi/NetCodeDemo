using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

[UpdateInGroup(typeof(ClientInitializationSystemGroup))]
public class ClientInitSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        if (!ClientConfig.Connect)
        {
            return;
        }
        
#if UNITY_EDITOR
        //var ep = NetworkEndPoint.Parse(ClientServerBootstrap.RequestedAutoConnect, ClientConfig.Port);
        var ep = NetworkEndPoint.Parse(ClientConfig.IpAddress , ClientConfig.Port, NetworkFamily.Ipv4);
#else
        //var ep = NetworkEndPoint.LoopbackIpv4.WithPort(7979);
        var ep = NetworkEndPoint.Parse(ClientConfig.IpAddress , ClientConfig.Port, NetworkFamily.Ipv4);
#endif
        World.GetExistingSystem<NetworkStreamReceiveSystem>().Connect(ep);

        ClientConfig.IsConnected = true;
        
        Enabled = false;
    }
}
