using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace Connections.Results
{
    [System.Serializable]
    public class ServerResponse
    {
        public List<string> images;
        public string log;
        public string xml;
    }


    public class ResultsViewer : MonoBehaviour
    {
        public Transform contentPanel; // Панель, где будут созданы ссылки
        public GameObject linkPrefab; // Префаб для отображения ссылки (например, текст или кнопка)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>log lile full url</returns>
        public string FetchAndDisplayFiles(string text)
        {
            RemoveAllChildren();
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(text);
            if (text == null || response == null)
            {
                return "";
            }
            
            {
                string xmlPath = response.xml;
                string trimmedPath = xmlPath.Substring(xmlPath.IndexOf('-') + 1);
                CreateLink("Xml: " + trimmedPath, xmlPath);
            }
            
            foreach (string imagePath in response.images)
            {
                string trimmedPath = imagePath.Substring(imagePath.IndexOf('-') + 1);
                CreateLink("Image: " + trimmedPath, imagePath);
            }

            {
                string logPath = response.log;
                string trimmedPath = logPath.Substring(logPath.IndexOf('-') + 1);
                CreateLink("Log: " + trimmedPath, logPath);
                return ConnectionsModule.HostUrl + logPath;

            }
            
        }
        
        private void RemoveAllChildren()
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        private void CreateLink(string displayName, string url)
        {
            url = ConnectionsModule.HostUrl + url;
            GameObject link = Instantiate(linkPrefab, contentPanel);
            TextMeshProUGUI linkText = link.GetComponentInChildren<TextMeshProUGUI>();
            linkText.text = displayName;

            Button button = link.GetComponent<Button>();
            button.onClick.AddListener(() => OpenInBrowser(url));
        }

        private void OpenInBrowser(string url)
        {
            Application.OpenURL(url);
        }
    }

}