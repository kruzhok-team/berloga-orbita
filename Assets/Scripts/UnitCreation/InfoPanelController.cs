using System;
using Connections;
using UnitCreation;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public FieldDeviceInfoSetter setter;
    public Button closed;
    
    public void Start()
    {
        closed.onClick.AddListener(delegate { gameObject.SetActive(false); });
    }

    public void Initialize(Device device)
    {
        setter.Initialize(device);
    }
}
