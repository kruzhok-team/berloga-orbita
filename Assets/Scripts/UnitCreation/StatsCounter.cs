using System;
using System.Collections.Generic;
using System.Globalization;
using Connections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using XML;

namespace UnitCreation
{
    
    public class StatsCounter : MonoBehaviour
    {
        public UnityEvent<List<Device>> onValueChanged;
        
        public TextMeshProUGUI maxMassField;
        public TextMeshProUGUI maxVolumeField;
        public TextMeshProUGUI totalMassField;
        public TextMeshProUGUI totalVolumeField;
        
        private List<Device> deviceStats = new List<Device>();

        public void AddNewDevice(Device device)
        {
            deviceStats.Add(device);
            onValueChanged.Invoke(deviceStats);
        }

        public void RemoveDevice(Device device)
        {
            deviceStats.Remove(device);
            onValueChanged.Invoke(deviceStats);
        }

        public void UpdateMaximums(double maxM, double maxV)
        {
            maxMassField.text =  Math.Round(maxM, 2).ToString(CultureInfo.InvariantCulture);
            maxVolumeField.text = Math.Round(maxV, 2).ToString(CultureInfo.InvariantCulture);
        }
        public void UpdateValues(double totalMass, double totalVolume)
        {
            totalMassField.text = Math.Round(totalMass, 2).ToString(CultureInfo.InvariantCulture);
            totalVolumeField.text =  Math.Round(totalVolume, 2).ToString(CultureInfo.InvariantCulture);
        }
        
    }
}
