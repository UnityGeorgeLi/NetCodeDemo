using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ClientVisualManager : MonoBehaviour
{
    private static ClientVisualManager _inst = null;

    [SerializeField] private GameObject _playerCharacterPrefab;

    private GameObject _localPlayerCharacter = null;

    static public ClientVisualManager Instance => _inst;
    
    private void Awake()
    {
        _inst = this;
    }

    public void InstantiateLocalPlayerCharacter()
    {
        _localPlayerCharacter = Instantiate(_playerCharacterPrefab);
    }

    public void SetPlayerLocation(Vector3 position)
    {
        if (_localPlayerCharacter)
        {
            _localPlayerCharacter.transform.position = position;
        }
    }

    public void SetPlayerRotation(Quaternion quaternion)
    {
        if (_localPlayerCharacter)
        {
            _localPlayerCharacter.transform.rotation = quaternion;
        }
    }
    
}
