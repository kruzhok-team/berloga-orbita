#define NO_SERVER

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Connections;
using UnityEngine;
using UnityEngine.Serialization;
using XML;


public sealed class GameManager : MonoBehaviour
{
    private static XModule xModule;
    private static ConnectionsModule _connectionsModule = new ConnectionsModule();
    private List<IXHandler> handlers = new List<IXHandler>();

    [SerializeField] public string outputPath = Application.dataPath + "/Out";
    [SerializeField] public string inputPath =  Application.dataPath + "/Input";
    
    [SerializeField] public string fileName =  "";
#if NO_SERVER
    public bool inputFileAppended=false; // option uses after manually add file in input folder
#endif
    private void Awake()
    {
        xModule = new XModule();
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
    }

    public void Test()
    {
#if NO_SERVER
        xModule.FileName = fileName;
        xModule.FilePath = inputPath;
        
#else
        // TODO: server logic
#endif
        xModule.Reload();
        foreach (var handler in handlers)
        {
            handler.CallBack();
        }
        xModule.LogDocument();
        xModule.SaveDocument("out_" + xModule.FileName, outputPath);
    }

    public void TestImage(string url = @"https://avatars.mds.yandex.net/i?id=9a7f15bfd1db79112f1fd527886da06e_l-4255244-images-thumbs&n=13")
    {
        Debug.Log(Application.dataPath);
        _connectionsModule.DownloadImage(url, Application.dataPath + "/Out" + "/test.png");
    }
}
