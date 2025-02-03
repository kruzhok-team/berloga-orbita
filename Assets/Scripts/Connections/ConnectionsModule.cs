using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

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
    public class WrappedData
    {
        public string data;
    }

    public class ConnectionsModule : MonoBehaviour
    {
        public static string logFilePath = null;
        
        public const String HostUrl = "http://orbita.kruzhok.org/";
        private readonly byte[] jsonData = Encoding.UTF8.GetBytes("{\"model\":\"planets\"}");

        public List<Device> devices = null;
        public ParametersResponse settings = null;
        public string sample = null;
        
        public bool devicesGot = false;
        
        public string result = null;
        public bool resultGot = false;
        public bool settingsGot = false;
        public bool sampleGot = false;
        
        public bool logDownloaded = false;

        public void Awake()
        {
            logFilePath = Path.Combine(Application.persistentDataPath, "log.log");
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
        public IEnumerator DownloadAndSaveLog(string url)
        {
            // Получаем путь к кэшу
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        File.WriteAllBytes(logFilePath, request.downloadHandler.data);
                        Debug.Log($"Файл успешно сохранён по пути: {logFilePath}");
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

            logDownloaded = true;
        }
        
        public void StartDownloadLog(string url)
        {
            logDownloaded = false;
            StartCoroutine(DownloadAndSaveLog(url));
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
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                try
                {
                    settings = JsonConvert.DeserializeObject<ParametersResponse>(responseText);
                    settingsGot = true;
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
            
        }

        // GET /sample
        public IEnumerator GetSample(string model, string mission)
        {
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
                sampleGot = true;
                // TODO: прописать геты всего
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
       
        

        // GET /devices
        public IEnumerator GetDevices()
        {
            string url = HostUrl + "devices";
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            
            request.uploadHandler = new UploadHandlerRaw(jsonData);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                
                List<Device> devices = XmlProcessor.ParseDevicesFromJson(responseText);
                foreach (var device in devices)
                {
                  //  Debug.Log($"Device Name: {device.Name}, FullName: {device.FullName}");
                }
                this.devices = devices;
            }
            else
            {
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
                Debug.Log("Calculation Status: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to get status: " + request.error);
            }
        }

        
        // GET /result
        public IEnumerator GetCalculationResult(string model, string id)
        {
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
                Debug.LogError("Failed to get result: " + request.error);
            }
            this.resultGot = true;
        }
        

    }
}
