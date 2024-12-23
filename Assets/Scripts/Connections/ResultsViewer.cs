using System.Collections;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace Connections.Results
{
    [System.Serializable]
    public class ServerResponse
    {
        public List<string> images;
        public List<string> log;
        public List<string> xml;
    }


    public class ResultsViewer : MonoBehaviour
    {
        public Transform contentPanel; // Панель, где будут созданы ссылки
        public GameObject linkPrefab; // Префаб для отображения ссылки (например, текст или кнопка)

        public void FetchAndDisplayFiles(string text)
        {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(text);

            foreach (string imagePath in response.images)
            {
                CreateLink("Image: " + imagePath, imagePath);
            }

            foreach (string logPath in response.log)
            {
                CreateLink("Log: " + logPath, logPath);
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