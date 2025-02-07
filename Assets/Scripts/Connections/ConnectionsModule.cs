using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Connections
{
    [System.Serializable]
    public class CalculationRequest
    {   
        public string model;
        public string xml;
    }
    
    [System.Serializable]
    public class ModelRequest
    {   
        public string model;
    }
    
    [System.Serializable]
    public class CheckRequest
    {   
        public string model;
        public string id;
    }
    [System.Serializable]
    public class ParametersResponse
    {
       
        public List<string> devices = new List<string>();
        public bool need_construction;
        public string probe_radius;
        public List<double> start_height = new List<double>();
        public string program;
    }

    [System.Serializable]
    public class CalculationResponse
    {
        public string id;
    }

    [System.Serializable]
    public class ModelMission
    {
        public string model;
        public string mission;
    }
    
    [System.Serializable]
    public class CalculationStatus
    {
        public string status;
    }
    
    [System.Serializable]
    public class WrappedData
    {
        public string data;
    }

    public class ConnectionsModule : MonoBehaviour
    {
        public static string logFilePath = null;
        public static string shortLogFilePath = null;
        
        public const String HostUrl = "http://orbita.kruzhok.org/";
        private readonly byte[] jsonData = Encoding.UTF8.GetBytes("{\"model\":\"planets\"}");

        public List<Device> devices = null;
        public ParametersResponse settings = null;
        public CalculationStatus calculationStatus = null;
        public string sample = null;
        
        public bool devicesGot = false;
        
        public string result = null;
        public bool resultGot = false;
        public bool statusGot = false;
        public bool settingsGot = false;
        public bool sampleGot = false;
        
        //public bool logDownloaded = false;

        public void Awake()
        {
            logFilePath = Path.Combine(Application.persistentDataPath, "log.log");
            shortLogFilePath = Path.Combine(Application.persistentDataPath, "log.xml");
        }

        public bool IsConnectionAvailable()
        {
            // TODO: need check out of this context
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Интернет-соединение отсутствует.");
                return false;
            }

            return true;
        }
        
        public IEnumerator DownloadAndSaveLog(string url, string path)
        {
          //  logDownloaded = false;
            // Получаем путь к кэшу
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        File.WriteAllBytes(path, request.downloadHandler.data);
                        Debug.Log($"Файл успешно сохранён по пути: {path}");
                    }
                    catch (IOException e)
                    {
                        Debug.LogError($"Ошибка записи файла: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogError($"Ошибка загрузки файла: {request.error}");
                }
            }

           // logDownloaded = true;
        }
        
        
        public void DownloadLogs(NecessaryLogs logsUrls)
        {
            //logDownloaded = false;
            StartCoroutine(DownloadAndSaveLog(logsUrls.shortLogUrl, shortLogFilePath));
            StartCoroutine(DownloadAndSaveLog(logsUrls.logUrl,logFilePath));
        }
        

        public IEnumerator GetServerInfo()
        {
            UnityWebRequest request = UnityWebRequest.Get($"{HostUrl}/server");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server Info: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to get server info: " + request.error);
            }
        }

        // GET /parameters
        public IEnumerator GetParameters(string model, string mission)
        {
            string url = HostUrl + "parameters";
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            
            var query = new ModelMission() { model = model, mission = mission};
            
            string jsonBody = JsonUtility.ToJson(query);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();
            settings = null;
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                try
                {
                    settings = JsonConvert.DeserializeObject<ParametersResponse>(responseText);
                    if (settings.program != null)
                    { 
                        settings.program = Regex.Replace(settings.program, @"^[\s\-]+", ""); 
                    }
                    else
                    {
                        settings.program = "";
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
            settingsGot = true;
            
        }

        // GET /sample
        public IEnumerator GetSample(string model, string mission)
        {
            sampleGot = false;
            string url = HostUrl + "sample";
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            
            var query = new ModelMission() { model = model, mission = mission};
            
            string jsonBody = JsonUtility.ToJson(query);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);
                
                var jsonResponse = JsonUtility.FromJson<WrappedData>(responseText);
                this.sample = jsonResponse.data;
            }
            else
            {
                sample = null;
                Debug.LogError("Error: " + request.error);
            }
            sampleGot = true;
        }
       
        

        // GET /devices
        public IEnumerator GetDevices()
        {
            devicesGot = false;
            string url = HostUrl + "devices";
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            
            request.uploadHandler = new UploadHandlerRaw(jsonData);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                
                devices = XmlProcessor.ParseDevicesFromJson(responseText);
                Debug.Log($"Successfully got devices");
               
            }
            else
            {
                devices = null;
                Debug.LogError("Error: " + request.error);
            }
            devicesGot = true;
        }
        

        // POST /calculation
        public IEnumerator PostCalculation(string model, string xmlData, Action<string> callback)
        {
            UnityWebRequest request = new UnityWebRequest($"{HostUrl}/calculation", "POST");
            request.SetRequestHeader("Content-Type", "application/json");

            // Создаем тело запроса
            var query = new CalculationRequest() { model = model, xml = xmlData };
            string jsonBody = JsonUtility.ToJson(query);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                CalculationResponse response = JsonUtility.FromJson<CalculationResponse>(responseText);
                
                if (response != null)
                {
                    callback?.Invoke(response.id);
                    Debug.Log("Extracted ID: " + response.id);
                }
                else
                {
                    Debug.LogError("Failed to parse JSON");
                }
            }
            else
            {
                Debug.LogError("Failed to start calculation: " + request.error);
            }
        }

        
        // GET /status
        public IEnumerator GetCalculationStatus(string model, string id)
        {
            statusGot = false;
            UnityWebRequest request = UnityWebRequest.Get($"{HostUrl}status");
            request.SetRequestHeader("Content-Type", "application/json");
            
            var query = new CheckRequest() { model = model, id = id};
            
            string jsonBody = JsonUtility.ToJson(query);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseText = request.downloadHandler.text;
                calculationStatus = JsonUtility.FromJson<CalculationStatus>(responseText);
                Debug.Log("Calculation Status: " + calculationStatus.status);
            }
            else
            {
                calculationStatus = null;
                Debug.LogError("Failed to get status: " + request.error);
            }
            statusGot = true;
        }

        
        // GET /result
        public IEnumerator GetCalculationResult(string model, string id)
        {
            resultGot = false;
            UnityWebRequest request = UnityWebRequest.Get($"{HostUrl}/result?model={model}&id={id}");
            request.SetRequestHeader("Content-Type", "application/json");
            
            var query = new CheckRequest() { model = model, id = id};
            
            string jsonBody = JsonUtility.ToJson(query);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Calculation Result: " + request.downloadHandler.text);
                this.result = request.downloadHandler.text;
            }
            else
            {
                result = null;
                Debug.LogError("Failed to get result: " + request.error);
            }
            this.resultGot = true;
        }
        

    }
}
