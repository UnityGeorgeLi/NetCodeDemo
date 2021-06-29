using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GameConfig : IComponentData
{
    public Entity PlayerPrefab;
    public Entity PlayerDummyPrefab;
    public Entity PlayerMinion;
}
