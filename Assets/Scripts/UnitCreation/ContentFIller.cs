using System.Collections.Generic;
using UnityEngine;

public class ContentFIller : MonoBehaviour
{
    public GameObject devicePrefab;
    public Transform contentTransform;
    
    private CsvLoader csvLoader;
    private List<Device> devices;
    
    private void Start()
    {
        csvLoader = GetComponent<CsvLoader>();
        devices = csvLoader.GetDevices();
        FillContent();
    }
    
    private void FillContent()
    {
        foreach (var device in devices)
        {
            AddDeviceToToolbox(device);
        }
    }
    private void AddDeviceToToolbox(Device device)
    {
        GameObject toolBoxElem = Instantiate(devicePrefab, contentTransform);
        
        // testing
        toolBoxElem.GetComponent<DeviceItem>().Setup(device);
        // end
        //TODO: set up params
    }
}
