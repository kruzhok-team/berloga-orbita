using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace TelemetryVisualization
{
    [System.Serializable]
    public class TelemetryData
    {
        public float time;
        public float height;
        public float verticalSpeed;

        public TelemetryData(float time, float height, float verticalSpeed)
        {
            this.time = time;
            this.height = height;
            this.verticalSpeed = verticalSpeed;
        }
    }

    public class FinalResults
    {
        public bool IsSuccess = false;
        public string FinalScore = null;
    }

    public class LogParser : MonoBehaviour
    {
        [HideInInspector] public List<TelemetryData> telemetryDataList = new List<TelemetryData>();
        [HideInInspector] public FinalResults results = new FinalResults();

        public void ParseShortLogFile(string path)
        {
            XDocument doc = XDocument.Parse(File.ReadAllText(path));
            string result = doc.Root?.Descendants("result").FirstOrDefault()?.Value ?? "N/A";
            string reason = doc.Descendants("reason").FirstOrDefault()?.Value ?? "N/A";
            string scientificInfo = doc.Descendants("scientificinformation").FirstOrDefault()?.Value ?? "0";

            Debug.Log($"Result: {result}");
            Debug.Log($"Reason: {reason}");
            Debug.Log($"Scientific Information: {scientificInfo}");

            if (result == "landing" || (result == "terminated" && reason == "limit"))
            {
                results.IsSuccess = true;
                results.FinalScore = scientificInfo;
            }
        }
        
        public void ParseLogFile(string filePath)
        {
            telemetryDataList.Clear();
            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            int startTelemetry = 1;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith("Flight telemetry:"))
                {
                    startTelemetry = i + 1;
                    break;
                }
            }

            for (int i = startTelemetry; i + 1 < lines.Length; i++)
            {
                string line = lines[i];
                
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                //Debug.Log("line " + i + " : " + line);
                ParseTelemetryLine(line);
            }
        }

        private void ParseTelemetryLine(string line)
        {
            // Пример строки: Ti=00:00:10 H=049920.9 Vy=-0015.5 Ac=001.5 Ae=000.0
            string[] parts = line.Split(' ');

            float time = ParseTime(parts[0]);
            float height = ParseValue(parts[1]);
            float verticalSpeed = ParseValue(parts[2]);

            telemetryDataList.Add(new TelemetryData(time, height, verticalSpeed));
        }

        private float ParseTime(string timePart)
        {
            // Пример времени: Ti=00:00:10
            string timeString = timePart.Substring(3); // Убираем "Ti="
            string[] timeComponents = timeString.Split(':');

            // Конвертируем в секунды
            return float.Parse(timeComponents[0]) * 3600 + float.Parse(timeComponents[1]) * 60 + float.Parse(timeComponents[2]);
        }

        private float ParseValue(string valuePart)
        {
            // Пример значения: H=049920.9
            string valueString = valuePart.Split("=")[1];
            return float.Parse(valueString, CultureInfo.InvariantCulture);
        }
    }
}