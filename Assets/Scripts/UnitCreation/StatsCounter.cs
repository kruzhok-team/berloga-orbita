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
        
        public double totalMass = 0;
        public double totalVolume = 0;

        public TextMeshProUGUI maxMassField;
        public TextMeshProUGUI maxVolumeField;
        public TextMeshProUGUI totalMassField;
        public TextMeshProUGUI totalVolumeField;
        
        private List<Device> deviceStats = new List<Device>();

        public void AddNewDevice(Device device)
        {
            deviceStats.Add(device);
            onValueChanged.Invoke(deviceStats); // inside should be called UpdateValues
        }

        public void RemoveDevice(Device device)
        {
            deviceStats.Remove(device);
        }
    
        public void UpdateValues(double maxM, double maxV)
        {
            maxMassField.text = maxM.ToString();
            maxVolumeField.text = maxV.ToString();
            totalMassField.text = Math.Round(totalMass, 2).ToString(CultureInfo.InvariantCulture);
            totalVolumeField.text =  Math.Round(totalVolume, 2).ToString(CultureInfo.InvariantCulture);
        }
        
    }
}
