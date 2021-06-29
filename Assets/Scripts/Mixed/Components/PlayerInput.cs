using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

public struct PlayerInput : ICommandData
{
    [GhostField] public uint Tick { get; set; }
    [GhostField] public float2 Movement;
}
