using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class GameSimulationSystem : SystemBase
{
    private GhostPredictionSystemGroup _serverSimulationSystemGroup;
    private EntityQuery _MinionsQuery;
    
    protected override void OnCreate()
    {
        _serverSimulationSystemGroup = World.GetExistingSystem<GhostPredictionSystemGroup>();
        _MinionsQuery = GetEntityQuery(
            typeof(SpawnedMinion),
            typeof(Translation)
        );
    }

    protected override void OnUpdate()
    {
        var tick = _serverSimulationSystemGroup.PredictingTick;
        var deltaTime = Time.DeltaTime;

        var minions = _MinionsQuery.ToEntityArray(Allocator.TempJob);
        var minionsData = _MinionsQuery.ToComponentDataArray<SpawnedMinion>(Allocator.TempJob);
        var minionTranslations = _MinionsQuery.ToComponentDataArray<Translation>(Allocator.TempJob); 

        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities
            .WithDisposeOnCompletion(minions)
            .WithDisposeOnCompletion(minionsData)
            .WithDisposeOnCompletion(minionTranslations)
            .ForEach((DynamicBuffer<PlayerInput> playerInput
                , ref Translation translation
                , in Rotation rotation
                , in Entity playerEntity
                , in PredictedGhostComponent prediction) =>
            {
                if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                {
                    return;
                }
                
                if (playerInput.GetDataAtTick(tick, out var data))
                {
                    var moveDist = data.Movement * deltaTime * 5.0f;
                    moveDist *= (rotation.Value == Quaternion.identity) ? 1.0f : -1.0f;
                    translation.Value.xz += moveDist;

                    for (int i = 0; i < minionsData.Length; ++i)
                    {
                        if (minionsData[i].PlayerEntity == playerEntity)
                        {
                            var newTranslation = minionTranslations[i];
                            newTranslation.Value.xz += moveDist;
                            ecb.SetComponent(minions[i], newTranslation);
                        }
                    }
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}