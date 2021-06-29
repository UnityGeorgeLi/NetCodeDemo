using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public struct SpawnedMinion : ISystemStateComponentData
{
    [GhostField] public Entity PlayerEntity;
    public Entity NextMinion;
}
