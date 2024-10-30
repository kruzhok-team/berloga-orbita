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
    
    private XGeneration unitCreationHandler = null;
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
        xModule.FilePath = inputPath;
        xModule.FileName = fileName;
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

    public void GenerateFinalXml()
    {
        xModule.Reload();
        List<IXHandler> handlers = unitCreationHandler.CollectParts();
        foreach (IXHandler handler in handlers)
        {
            handler.CallBack();
        }
        
        xModule.LogDocument();
        Debug.LogWarning("No server implemented, but xml generated");
    }
    private void RegisterHandlers()
    {
        unitCreationHandler = FindFirstObjectByType<XGeneration>();
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
        Debug.LogWarning("Correctly generated XML output, but logic not implemented as no server exist");
        // TODO: work with server and show results
    }

}
