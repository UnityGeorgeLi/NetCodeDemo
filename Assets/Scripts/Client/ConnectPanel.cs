using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ConnectPanel : MonoBehaviour
{
    [SerializeField] private  TMP_InputField _ServerIP;
    [SerializeField] private TMP_InputField _ServerPort;
    [SerializeField] private TMP_InputField _MinionsCount;
    [SerializeField] private TMP_Dropdown _Direction;
    
    private void OnEnable()
    {
        ClientConfig.Init();
        
        _ServerIP.text = ClientConfig.IpAddress;
        _ServerPort.text = ClientConfig.Port.ToString();
        _MinionsCount.text = ClientConfig.MinionsCount.ToString();
    }

    public void OnConnectClicked()
    {
        ClientConfig.IpAddress = _ServerIP.text;
        ClientConfig.Port = ushort.Parse(_ServerPort.text);
        ClientConfig.MinionsCount = int.Parse(_MinionsCount.text);
        ClientConfig.Direction = _Direction.value;
        ClientConfig.Connect = true;

        gameObject.SetActive(false);
    }

    public void OnDirectionChanged()
    {
        ClientConfig.Direction = _Direction.value;
    }
    
}
