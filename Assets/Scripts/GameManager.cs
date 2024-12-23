using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelemetryVisualization;
using UnityEngine;
using Connections;
using Menu;
using UnityEngine.Serialization;
using XML;

[RequireComponent(typeof(MissionHandler))]
public sealed class GameManager : MonoBehaviour
{
    public static bool devicesGot = false;
    public static string devicePrefixPath = null;
    
    private static XModule xModule;
    private static XModule xModuleBallistic;
    
    private MissionHandler missionHandler;
    private XGeneration unitCreationHandler = null;
    private IXHandler ballisticHandler  = null;
    private TabSystem tabSystem = null;


    private void Awake()
    {
        devicePrefixPath = "Devices/";
    }
    private void Start()
    {
        missionHandler = GetComponent<MissionHandler>();
        tabSystem = FindFirstObjectByType<TabSystem>();
        RegisterHandlers();
        PrepareMission();
        
        xModule = new XModule(missionHandler.GetMissionXmlPath());
        xModuleBallistic = new XModule(missionHandler.GetBallisticCalculatorXmlPath());
        StartCoroutine(GetAllowedDevices());
    }
    

    private void PrepareMission()
    {
        if (!missionHandler.mission.allowedBallisticCalculator)
        {
            tabSystem.DeactivateTab(TabType.Ballistic);
        }
        if (!missionHandler.mission.allowedUnitCreation)
        {
            tabSystem.DeactivateTab(TabType.UnitCreation);
        }
        if (!missionHandler.mission.allowedCode)
        {
            tabSystem.DeactivateTab(TabType.CodeEditor);
        }
    }

    public List<Device> GetDevices()
    {
        return missionHandler.devices;
    }
    
    private IEnumerator GetAllowedDevices()
    {
        while (missionHandler.isReady != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        devicesGot = true;
    }
    
    public void TestImage(string url = @"https://avatars.mds.yandex.net/i?id=9a7f15bfd1db79112f1fd527886da06e_l-4255244-images-thumbs&n=13")
    {
        Debug.Log(Application.dataPath);
        //_connectionsModule.DownloadImage(url, Application.dataPath + "/Out" + "/test.png");
    }

    private void VisualizeTelemetry(string path)
    {
        var parser = FindFirstObjectByType<LogParser>();
        var vs = FindFirstObjectByType<TelemetryVisualizer>();
        parser.ParseLogFile(path);
        vs.Visualize(parser.telemetryDataList);
    }
    
    public void ExecuteModel()
    {
        xModule.Reload();
        List<IXHandler> handlers = unitCreationHandler.CollectParts();
        foreach (IXHandler handler in handlers)
        {
            handler.CallBack();
        }
        missionHandler.StartMissionCalculation(xModule.ToString());
        xModule.LogDocument();
        Debug.LogWarning("No server implemented, but xml generated");
    }

    public void ShowResults()
    {
        StartCoroutine(missionHandler.GetMissionResults());
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
