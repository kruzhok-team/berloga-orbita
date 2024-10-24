using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Device
{
    public string name;
    public string code;
    public float mass;
    public float volume;
    public float energy;
    public int criticalTemperature;
    public string imagePath;
}

public class CsvLoader : MonoBehaviour
{
    private List<Device> devices = new List<Device>();
    
    public string csvFilePath = "Assets/Resources/devices.csv";
    private string prefixPath = "";

    public List<Device> GetDevices(string path = null)
    {
        LoadCsv(path ?? csvFilePath);
        return devices;
    }
    
    private void LoadCsv(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Check file path of csv file: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length == 7)
            {
                string name = values[0];
                string code = values[1];
                float mass = ParseFloat(values[2]);
                float volume = ParseFloat(values[3]);
                float energy = ParseFloat(values[4]);
                int criticalTemperature = ParseInt(values[5]);
                string imagePath = prefixPath + values[6];

                Device device = new Device() { name = name, code = code, mass = mass, volume = volume, energy = energy, 
                    criticalTemperature = criticalTemperature, imagePath = imagePath};
                devices.Add(device);
            }
        }
    }
    
    private float ParseFloat(string value)
    {
        if (float.TryParse(value, out float result))
        {
            return result;
        }
        return -1f;
    }
    private int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        return -1;
    }
}
