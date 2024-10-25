#define NO_SERVER

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Connections;
using TelemetryVisualization;
using UnityEngine;
using UnityEngine.Serialization;
using XML;

public sealed class GameManager : MonoBehaviour
{
    private static XModule xModule = new XModule();
    private static XModule xModuleBallistic = new XModule();
    private static ConnectionsModule _connectionsModule = new ConnectionsModule();
    private List<IXHandler> handlers = new List<IXHandler>();
    private IXHandler ballisticHandler  = null;

    private string ballisticsPath = Application.dataPath + "/Resources";
    private string ballisticsFileName = "ballistics.xml";
    
    [SerializeField] public string outputPath = Application.dataPath + "/Out";
    [SerializeField] public string inputPath =  Application.dataPath + "/Input";
    
    [SerializeField] public string fileName =  "";
#if NO_SERVER  // options used after manually add file in input folder
    public bool addedTelemetryFile=false;
#endif
    private void Awake()
    {
        //xModule = new XModule();
    }

    private void Start()
    {
        RegisterHandlers();
        xModuleBallistic.FileName = ballisticsFileName;
        xModuleBallistic.FilePath = ballisticsPath;
    }

    private void Update()
    {
#if NO_SERVER
        if (addedTelemetryFile)
        {
            addedTelemetryFile = false;
            VisualizeTelemetry(Application.dataPath + "/Out" + "/log.log");
        }
#else
        // TODO: server logic
#endif
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

    private void VisualizeTelemetry(string path)
    {
        LogParser parser = FindFirstObjectByType<LogParser>();
        TelemetryVisualizer vs = FindFirstObjectByType<TelemetryVisualizer>();
        parser.ParseLogFile(path);
        vs.Visualize(parser.telemetryDataList);
    }
    
    
    private void RegisterHandlers()
    {
        foreach (var handler in FindObjectsByType<XHandlerPasteAttribute>(FindObjectsSortMode.None))
        {
            handlers.Add(handler);
        }
        ballisticHandler = FindFirstObjectByType<XHandlerInsertValue>();
  
    }
    public XModule GetXModule()
    {
        return xModule;
    }

    public XModule GetXModuleBallistics()
    {
        return xModuleBallistic;
    }

    public void BallisticCalculatorBtnPushDown()
    {
        xModuleBallistic.Reload();
        ballisticHandler.CallBack();
        xModuleBallistic.LogDocument();
    }

}
