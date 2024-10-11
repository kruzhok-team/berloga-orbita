using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Connections;
using UnityEngine;
using XML;

public sealed class GameManager : MonoBehaviour
{
    private static XModule xModule;
    private static ConnectionsModule _connectionsModule = new ConnectionsModule();
    private List<IXHandler> handlers = new List<IXHandler>();

    [SerializeField] public static readonly string OutputPath = Application.dataPath + "/Out";
    [SerializeField] public static readonly string InputPath =  Application.dataPath + "/Input"; // Only no server logic

    private void Awake()
    {
        xModule = new XModule(XModule.DefaultFilePath, "test.xml");
    }

    private void Start()
    {
        GetHandlers();
    }
    public XModule GetXModule()
    {
        return xModule;
    }

    private void GetHandlers()
    {
        foreach (var handler in FindObjectsByType<XHandlerPasteAttribute>(FindObjectsSortMode.None))
        {
            handlers.Add(handler);
        }
        foreach (var handler in FindObjectsByType<XHandlerCreateNew>(FindObjectsSortMode.None))
        {
            handlers.Add(handler);
        }
        foreach (var handler in FindObjectsByType<XHandlerInsertInnerValue>(FindObjectsSortMode.None))
        {
            handlers.Add(handler);
        }
    }

    public void Test()
    {
        xModule.Reload();
        foreach (var handler in handlers)
        {
            handler.CallBack();
        }
        xModule.LogDocument();
    }

    public void TestImage(string url = @"https://avatars.mds.yandex.net/i?id=9a7f15bfd1db79112f1fd527886da06e_l-4255244-images-thumbs&n=13")
    {
        Debug.Log(Application.dataPath);
        _connectionsModule.DownloadImage(url, Application.dataPath + "/Out" + "/test.png");
    }
}
