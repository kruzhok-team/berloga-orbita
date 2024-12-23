using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Connections;
using Connections.Results;
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
        [SerializeField] public string lastMissionId;
        public Mission mission;
        
        public ConnectionsModule _server { get; private set; }
        
        public List<Device> devices = null;
        
        public int isReady = 1; // 0 = ready
    
        private void Start()
        {
            _server = gameObject.AddComponent<ConnectionsModule>();
            StartCoroutine(_server.GetDevices()); // this need very much
            StartCoroutine(GetAllowedDevices());
        }
        
        
        private IEnumerator GetAllowedDevices()
        {
            while (!_server.devicesGot)
            {
                yield return new WaitForSeconds(1f);
            }
            devices = _server.devices;
            isReady--;
            // TODO: here
        }

        public void StartMissionCalculation(string xml)
        {
            // TODO: after callback say that we ready, and save id
            StartCoroutine(_server.PostCalculation("planets", xml, null));
        }

        public IEnumerator GetMissionResults()
        {
            // TODO: collect status first
            StartCoroutine(_server.GetCalculationResult("planets", lastMissionId));
            while (!_server.resultGot)
            {
                yield return new WaitForSeconds(1f);
            }

            var rv = FindFirstObjectByType<ResultsViewer>();
            rv.FetchAndDisplayFiles(_server.result);
        }

        public string GetMissionXmlPath()
        {
            if (mission.missionName == "moon")
            {
                CopyFileFromResources("Xml/Moon", Application.persistentDataPath + "/moon.xml");
                return Application.persistentDataPath + "/moon.xml";
            }
    
            return null;
        }
        

        public string GetBallisticCalculatorXmlPath()
        {
            CopyFileFromResources("ballistics", Application.persistentDataPath + "/ballistics.xml");
            return Application.persistentDataPath + "/ballistic_calculator.xml";
        }

        private void CopyFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                try
                {
                    File.Copy(sourcePath, destinationPath, overwrite: true);
                    Debug.Log($"Файл успешно скопирован из {sourcePath} в {destinationPath}");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Ошибка при копировании файла: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"Исходный файл не найден: {sourcePath}");
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
