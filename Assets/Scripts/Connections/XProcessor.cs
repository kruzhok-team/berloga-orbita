using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEngine;


namespace Connections
{
    [Serializable]
    public class JsonResponse
    {
        public string data;
    }
    [Serializable]
    public class Device
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public double Mass { get; set; }
        public double Volume { get; set; }
        public int PowerGeneration { get; set; }
        public int PeriodMin { get; set; }
        public int PeriodMax { get; set; }
        public int TrafficGeneration { get; set; }
        public int CriticalTemperature { get; set; }
    }

    public class XmlProcessor
    {
        public static List<Device> ParseDevicesFromJson(string jsonResponse)
        {
            List<Device> devices = new List<Device>();
            var jsonData = JsonUtility.FromJson<JsonResponse>(jsonResponse);
            string xmlString = jsonData.data;
            
            string filePath = Application.persistentDataPath + "/devices.xml";
            
            WriteToFile(filePath, xmlString); // crutch
            
            XmlDocument xmlDoc = new XmlDocument();
            
            xmlDoc.Load(filePath);
            
            XmlNodeList deviceNodes = xmlDoc.GetElementsByTagName("device");
            
            foreach (XmlNode deviceNode in deviceNodes)
            {
                string name = deviceNode.Attributes["name"].Value;
                string fullName = deviceNode.Attributes["full_name"].Value;
                string code = deviceNode.Attributes["code"].Value;

                XmlNode typeNode = deviceNode.SelectSingleNode("type");
                XmlNode massNode = deviceNode.SelectSingleNode("mass");
                XmlNode volumeNode = deviceNode.SelectSingleNode("volume");
                XmlNode powerGenerationNode = deviceNode.SelectSingleNode("power_generation");
                XmlNode periodMinNode = deviceNode.SelectSingleNode("period_min");
                XmlNode periodMaxNode = deviceNode.SelectSingleNode("period_max");
                XmlNode trafficGenerationNode = deviceNode.SelectSingleNode("traffic_generation");
                XmlNode criticalTemperatureNode = deviceNode.SelectSingleNode("critical_temperature");
                
                Device device = new Device();
                device.Name = name;
                device.FullName = fullName;
                device.Code = code;
                device.Type = typeNode?.InnerText;
                device.Mass = double.Parse(massNode?.InnerText ?? "0", CultureInfo.InvariantCulture);
                device.Volume = double.Parse(volumeNode?.InnerText  ?? "0", CultureInfo.InvariantCulture);
                device.PowerGeneration = int.Parse(powerGenerationNode?.InnerText  ?? "0");
                device.PeriodMin = int.Parse(periodMinNode?.InnerText  ?? "0");
                device.PeriodMax = int.Parse(periodMaxNode?.InnerText  ?? "0");
                device.TrafficGeneration = int.Parse(trafficGenerationNode?.InnerText  ?? "0");
                device.CriticalTemperature = int.Parse(criticalTemperatureNode?.InnerText  ?? "0");
                devices.Add(device);
            }
            return devices;
        }
        
         public static List<Device> ParseParametersFromJson(string jsonResponse)
        {
            List<Device> devices = new List<Device>();
            var jsonData = JsonUtility.FromJson<JsonResponse>(jsonResponse);
            string xmlString = jsonData.data;
            
            string filePath = Application.dataPath + "/parameters.xml";
            
            WriteToFile(filePath, xmlString); // crutch
            
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
            }
            catch (XmlException ex)
            {
                Debug.Log(ex.Message);
            }
            
            Debug.Log(xmlDoc.OuterXml);
            
            //XmlNodeList deviceNodes = xmlDoc.GetElementsByTagName("device");
            
            return devices;
        }

        private static void WriteToFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                Debug.Log($"Файл успешно записан по пути: {filePath}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"Ошибка при записи в файл: {ex.Message}");
            }
        }

    }
}