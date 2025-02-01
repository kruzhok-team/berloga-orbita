using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class StatsCounter : MonoBehaviour
{
    public double totalMass = 0;
    public double totalVolume = 0;

    public TextMeshProUGUI maxMassField;
    public TextMeshProUGUI maxVolumeField;
    public TextMeshProUGUI totalMassField;
    public TextMeshProUGUI totalVolumeField;

    public void AddNewDevice(double mass, double volume)
    {
        totalMass += mass;
        totalVolume += volume;
        UpdateValues();
    }

    public void RemoveDevice(double mass, double volume)
    {
        totalMass -= mass;
        totalVolume -= volume;
        UpdateValues();
    }
    
    public void Init(double maxM, double maxV)
    {
        maxMassField.text = maxM.ToString();
        maxVolumeField.text = maxV.ToString();
    }
    
    
    public void UpdateValues()
    {
        totalMassField.text = Math.Round(totalMass, 2).ToString(CultureInfo.InvariantCulture);
        totalVolumeField.text =  Math.Round(totalVolume, 2).ToString(CultureInfo.InvariantCulture);
    }
}
