using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ClientConfig
{
    static public string IpAddress = "127.0.0.1";
    static public ushort Port = 7979;
    static public int MinionsCount = 100;
    static public int Direction = 0;
    static public bool Connect = false;

    private static bool _isConnected = false;
    private static float3 _playerPosition;
    private static float3 _playerRotation;
    
    static public bool IsConnected
    {
        get
        {
            return _isConnected;
        }

        set
        {
            if (!_isConnected && value != _isConnected)
            {
                ClientVisualManager.Instance.InstantiateLocalPlayerCharacter();
            }

            Connect = false;
            _isConnected = value;
        }
    }

    public static float3 PlayerPosition
    {
        get => _playerPosition;
        set
        {
            _playerPosition = value; 
            ClientVisualManager.Instance.SetPlayerLocation(new Vector3(value.x, value.y, value.z));         
        }
    }

    static public float3 PlayerRotation
    {
        get => _playerRotation;
        set
        {
            _playerRotation = value;
            ClientVisualManager.Instance.SetPlayerRotation(Quaternion.Euler(new Vector3(value.x, value.y, value.z)));
        }
    }

    static public void Init()
    {
        IpAddress = "127.0.0.1";
        Port = 7979;
        MinionsCount = 100;
        Direction = 0;
        Connect = false;
        _isConnected = false;
    }
}
