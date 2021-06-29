using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct SpawnedPlayer : ISystemStateComponentData
{
    public Entity Value;
    public Entity FirstMinion;
}
