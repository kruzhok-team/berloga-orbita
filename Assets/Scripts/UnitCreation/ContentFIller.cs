using System.Collections;
using System.Collections.Generic;
using Connections;
using UnityEngine;

namespace UnitCreation
{
    public class ContentFiller : MonoBehaviour
    {
        public GameObject devicePrefab;
        public Transform contentTransform;
        
        private List<Device> _devices;
        
        private GameManager _manager;
    
        private void Start()
        {
            _manager = FindFirstObjectByType<GameManager>();
            StartCoroutine(FetchContent());
        }

        private IEnumerator FetchContent()
        {
            while (!GameManager.devicesGot)
            {
                yield return new WaitForSeconds(0.1f);
            }
            _devices = _manager.GetDevices();
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
