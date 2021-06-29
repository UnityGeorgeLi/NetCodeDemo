using System.Collections;
using System.Collections.Generic;
using Unity.NetCode;
using UnityEngine;

public struct SpawnPlayerArmyServerRPC : IRpcCommand
{
    public ushort PlayerCharacter;
    public ushort Direction;
    public ushort MinionsCount;
}
