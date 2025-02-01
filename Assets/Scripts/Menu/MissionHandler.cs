using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Connections;
using Connections.Results;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
    [System.Serializable]
    public class Mission
    { 
        public string missionName;
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
        private ConnectionsModule Server { get; set; }
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
            
            
            isReady--;
            // maxMassField.SetText(Server.settings.maxMass); // TODO: not implemented on server
            maxVolumeField.SetText(Server.settings.probe_radius); // TODO: in server strange output
            startHeightField.SetText(Server.settings.start_height.
                First().ToString(CultureInfo.InvariantCulture)); // TODO: randomize parameters, and let server know what is real
            //TODO: use xml and program from Server.settings
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
            if (Mission.missionName == "moon")
            {
                CopyFileFromResources("Xml/Moon", Application.persistentDataPath + "/moon.xml");
                return Application.persistentDataPath + "/moon.xml";
            }
    
            return null;
        }
        

        public string GetBallisticCalculatorXmlPath()
        {
            var path = Application.persistentDataPath + "/ballistic_calculator.xml";
            CopyFileFromResources("ballistics", path);
            return path;
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
            if (resourceFile != null)
            {
                try
                {
                    // Запись содержимого файла в целевой путь
                    File.WriteAllText(destinationPath, resourceFile.text);
                    Debug.Log($"Файл успешно скопирован в {destinationPath}");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Ошибка при записи файла: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"Файл {fileName} не найден в папке Resources.");
            }
        }
    }
}
