using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Connections;
using Connections.Results;
using TMPro;
using UnityEngine;

namespace Menu
{
    [System.Serializable]
    public class Mission
    { 
        //public string missionName;
        public bool allowedBallisticCalculator = true;
        public bool allowedUnitCreation = true;
        public bool allowedCode = true;
    }
    
    public class MissionHandler : MonoBehaviour
    {
        [SerializeField] public string lastMissionId = null;
        public TextMeshProUGUI maxMassField;
        public TextMeshProUGUI maxVolumeField;
        public TextMeshProUGUI startHeightField;
        
        public string missionName;
        
        public Mission Mission { get; private set; } = new Mission();
        public ConnectionsModule Server { get; private set; }
        public List<Device> Devices { get; private set; } = new List<Device>();
        
        
        public int isReady = 1; // 0 = ready
    
        private void Start()
        {
            Server = gameObject.AddComponent<ConnectionsModule>();
            StartCoroutine(Server.GetDevices());
            StartCoroutine(Server.GetSample("planets", missionName));
            StartCoroutine(Server.GetParameters("planets", missionName));
            StartCoroutine(WaitForResults());
        }
        
        
        private IEnumerator WaitForResults()
        {
            while (!Server.devicesGot && !Server.sampleGot && !Server.settingsGot)
            {
                yield return new WaitForSeconds(0.1f);
            }
            foreach (var device in Server.devices.Where(device => Server.settings.devices.Contains(device.Name)))
            {
                Devices.Add(device);
            }

            Mission.allowedUnitCreation = Server.settings.need_construction;
            
            // maxMassField.SetText(Server.settings.maxMass); // TODO: not implemented on server
            maxVolumeField.SetText(Server.settings.probe_radius); // TODO: in server strange output
            startHeightField.SetText(Server.settings.start_height.
                First().ToString(CultureInfo.InvariantCulture)); // TODO: randomize parameters, and let server know what is real
            
            isReady--;
        }

        public void StartMissionCalculation(string xml)
        {
            // TODO: after callback say that we ready, and save id
            lastMissionId = null;
            StartCoroutine(Server.PostCalculation("planets", xml, (s => lastMissionId = s)));
        }

        public IEnumerator GetMissionResults()
        {
            // TODO: collect status first
            while (lastMissionId == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            StartCoroutine(Server.GetCalculationStatus("planets", lastMissionId));
            StartCoroutine(Server.GetCalculationResult("planets", lastMissionId));
            while (!Server.resultGot)
            {
                yield return new WaitForSeconds(1f);
            }

            var rv = FindFirstObjectByType<ResultsViewer>();
            var logUrl = rv.FetchAndDisplayFiles(Server.result);
            StartCoroutine(Server.DownloadAndSaveLog(logUrl));
        }

        public string GetMissionXmlPath()
        {
            var savePath = Application.persistentDataPath + "/main.xml";
            SaveFileToPath(savePath, Server.sample);
            Debug.LogWarning(savePath);
            return savePath;
        }
        

        public string GetBallisticCalculatorXmlPath()
        {
            var path = Application.persistentDataPath + "/ballistic_calculator.xml";
            CopyFileFromResources("ballistics", path);
            return path;
        }

        private void SaveFileToPath(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Get error while writing: {ex.Message}");
            }
        }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Имя файла без расширения</param>
        /// <param name="destinationPath">Целевой путь</param>
        private void CopyFileFromResources(string fileName, string destinationPath)
        {
            // Загрузка файла из Resources
            TextAsset resourceFile = Resources.Load<TextAsset>(fileName);
            if (resourceFile)
            {
                SaveFileToPath(destinationPath, resourceFile.text);
            }
            else
            {
                Debug.LogError($"Файл {fileName} не найден в папке Resources.");
            }
        }
    }
}
