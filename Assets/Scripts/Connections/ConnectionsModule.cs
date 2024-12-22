using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        public string data;
    }
    
    public class CalculationResponse
    {
        public string id;
    }
    
    
    public class ConnectionsModule : MonoBehaviour
    {
        private const String HostUrl = "http://orbita.kruzhok.org/";
        private readonly byte[] jsonData = Encoding.UTF8.GetBytes("{\"model\":\"planets\"}");

        public void Respond(XmlDocument doc)
        {

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

        public async Task<bool> DownloadImage(string url, string savePath)
        {
            await ImageDownloader.DownloadAndSaveImageAsync(url, savePath);
            return true;
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
        public IEnumerator GetParameters()
        {
            string url = HostUrl + "parameters";
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            
            request.uploadHandler = new UploadHandlerRaw(jsonData);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                var devices = XmlProcessor.ParseParametersFromJson(responseText);
                Debug.Log("Response: " + responseText);
                
                var jsonResponse = JsonUtility.FromJson<ParametersResponse>(responseText);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        // Структура для обработки ответа
       
        

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
                    Debug.Log($"Device Name: {device.Name}, FullName: {device.FullName}");
                }
                // TODO: return this
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
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
                    callback.Invoke(response.id);
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
            }
            else
            {
                Debug.LogError("Failed to get result: " + request.error);
            }
        }

    }
}
