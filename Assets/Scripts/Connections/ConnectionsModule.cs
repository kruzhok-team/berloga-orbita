using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Connections
{
    public class ConnectionsModule
    {
        public void Respond(XmlDocument doc)
        {
            
        }
        void Start()
        {
            // TODO: need check out of this context
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Интернет-соединение отсутствует.");
                return;
            }
        }

        public async void DownloadImage(string url, string savePath)
        {
            await ImageDownloader.DownloadAndSaveImageAsync(url, savePath);
        }
        
    }
}
