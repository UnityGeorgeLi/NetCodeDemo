using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup((typeof(ClientSimulationSystemGroup)))]
public class ClientGameplaySystem : SystemBase
{
    private Transform _playerCameraTransform;
    private Vector3 _cameraOffsetVector = new Vector3(0.0f, 15.0f, -25.0f);

    protected override void OnCreate()
    {
        _playerCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
  
        RequireSingletonForUpdate<GameConfig>();
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        var connectionEntity = GetSingletonEntity<NetworkIdComponent>();
        var networkId = GetComponent<NetworkIdComponent>(connectionEntity).Value;
        
        Entities.WithoutBurst().ForEach((Entity playerEntity, in GhostOwnerComponent owner) =>
        {
            if (owner.NetworkId == networkId)
            {
                var translation = EntityManager.GetComponentData<Translation>(playerEntity);
                var rotation = EntityManager.GetComponentData<Rotation>(playerEntity);

                var pos = new Vector3(translation.Value.x, translation.Value.y + 1.0f, translation.Value.z);
                var rot = new Quaternion(rotation.Value.value.x, rotation.Value.value.y, rotation.Value.value.z,
                    rotation.Value.value.w);

                ClientVisualManager.Instance.SetPlayerLocation(pos);
                ClientVisualManager.Instance.SetPlayerRotation(rot);
                
                _playerCameraTransform.position = rot * _cameraOffsetVector + pos;
                _playerCameraTransform.LookAt(pos);
            }
        }).Run();
    }
}
