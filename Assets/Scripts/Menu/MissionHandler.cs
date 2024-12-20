using Connections;
using UnityEngine;

public class MissionHandler : MonoBehaviour
{
    [SerializeField] public int missionId;
    private readonly ConnectionsModule _server = new ConnectionsModule();
    
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
    void Start()
    {
        _ShowAndroidToastMessage("Mission started.");
    }

  
    void Update()
    {
        
    }
}
