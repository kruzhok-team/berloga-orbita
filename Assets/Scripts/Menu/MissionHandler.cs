using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Connections;
using TMPro;
using UnityEngine;

namespace Menu
{
    [System.Serializable]
    public class Mission
    { 
        public bool allowedBallisticCalculator = true;
        public bool allowedUnitCreation = true;
        public bool allowedCode = true;
    }
    
    [RequireComponent(typeof(Validator))]
    public class MissionHandler : MonoBehaviour
    {
        public BaseParameterSetup baseParameterSetup;
        [SerializeField] public string lastMissionId = null;
        public TextMeshProUGUI resultStatusField;
        
        public string missionName;
        
        public Mission Mission { get; private set; } = new Mission();
        public ConnectionsModule Server { get; private set; }
        public List<Device> Devices { get; private set; } = new List<Device>();
        private Validator validator;
        
        
        public int isReady = 1; // 0 = ready
    
        private void Start()
        {
            isReady = 1;
            Server = gameObject.AddComponent<ConnectionsModule>();
            validator = GetComponent<Validator>();
            StartCoroutine(Server.GetDevices());
            StartCoroutine(Server.GetSample("planets", missionName));
            StartCoroutine(Server.GetParameters("planets", missionName));
            StartCoroutine(WaitForResults());
        }
        
        
        private IEnumerator WaitForResults()
        {
            while (!Server.devicesGot || !Server.sampleGot || !Server.settingsGot)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (Server.devices == null)
            {
                Debug.LogError("No devices got loaded!");
            }
            foreach (var device in Server.devices.Where(device => Server.settings.devices.Contains(device.Name)))
            {
                Devices.Add(device);
            }

            Mission.allowedUnitCreation = Server.settings.need_construction;
            baseParameterSetup.RadiusSettingsSetup(Server.settings.probe_radius);
            baseParameterSetup.HeightSettingsSetup(Server.settings.start_height);
            isReady--;
        }

        public void StartCalculation(string xml, string model)
        {
            validator.StopAllExecutions();
            lastMissionId = null;
            validator.executions.Add(StartCoroutine(
                Server.PostCalculation(model, xml, (s => lastMissionId = s))));
            FindFirstObjectByType<ResultsViewer>()?.RemoveAllChildren();
        }
        
        
        public IEnumerator GetCalculationResults(string model)
        {
            resultStatusField.SetText("запускаем");
            while (lastMissionId == null)
            {
                yield return new WaitForSeconds(0.2f);
            }
            // collecting and awaiting completed status
            validator.executions.Add(StartCoroutine(Server.GetCalculationStatus(model, lastMissionId)));
            while (Server.calculationStatus is not { status: "completed" })
            {
                if (Server.statusGot)
                {
                    validator.executions.Add(StartCoroutine(Server.GetCalculationStatus(model, lastMissionId)));
                }
                yield return new WaitForSeconds(0.5f);
                
            }
            resultStatusField.SetText("вычисляем");
            validator.executions.Add(StartCoroutine(Server.GetCalculationResult(model, lastMissionId)));
            while (!Server.resultGot)
            {
                yield return new WaitForSeconds(0.1f);
            }
            resultStatusField.SetText("получен");
            var rv = FindFirstObjectByType<ResultsViewer>();
            var logUrl = rv.FetchAndDisplayFiles(Server.result);
            if (model != "planets_gravity")
            {
                Server.DownloadLogs(logUrl);
            }
            
        }

        public void DisplayStatus(string status)
        {
            resultStatusField.SetText(status);
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
