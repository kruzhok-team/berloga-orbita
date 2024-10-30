using System.Collections.Generic;
using UnityEngine;

namespace UnitCreation
{
    public class ContentFIller : MonoBehaviour
    {
        public GameObject devicePrefab;
        public Transform contentTransform;
    
        private CsvLoader _csvLoader;
        private List<Device> _devices;
    
        private void Start()
        {
            _csvLoader = GetComponent<CsvLoader>();
            _devices = _csvLoader.GetDevices();
            FillContent();
        }
    
        private void FillContent()
        {
            foreach (var device in _devices)
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
}
