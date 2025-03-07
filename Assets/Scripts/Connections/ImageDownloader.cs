using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Connections
{
    public static class ImageDownloader
    {
        public static async Task DownloadAndSaveImageAsync(string url, string savePath)
        {
            using HttpClient client = new HttpClient();
            try
            {
                var imageData = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(savePath, imageData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while saving or downloading: " + e.Message);
            }
        }
    }
}
