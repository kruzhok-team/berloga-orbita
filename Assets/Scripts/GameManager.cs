using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelemetryVisualization;
using UnityEngine;
using Connections;
using Menu;
using UnitCreation;
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

    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        devicePrefixPath = "Devices/";
    }
    private void Start()
    {
        missionHandler = GetComponent<MissionHandler>();
        tabSystem = FindFirstObjectByType<TabSystem>();
        StartCoroutine(FindFirstObjectByType<ContentFiller>().FetchContent());
        RegisterHandlers();
        StartCoroutine(PrepareMission());
        
        xModule = new XModule(missionHandler.GetMissionXmlPath());
        xModuleBallistic = new XModule(missionHandler.GetBallisticCalculatorXmlPath());
        StartCoroutine(GetAllowedDevices());
    }
    

    private IEnumerator PrepareMission()
    {
        while (missionHandler.isReady != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (!missionHandler.Mission.allowedBallisticCalculator)
        {
            tabSystem.DeactivateTab(TabType.Ballistic);
        }
        if (!missionHandler.Mission.allowedUnitCreation)
        {
            tabSystem.DeactivateTab(TabType.UnitCreation);
        }
        if (!missionHandler.Mission.allowedCode)
        {
            tabSystem.DeactivateTab(TabType.CodeEditor);
        }
    }

    public List<Device> GetDevices()
    {
        return missionHandler.Devices;
    }
    
    private IEnumerator GetAllowedDevices()
    {
        while (missionHandler.isReady != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        devicesGot = true;
    }
    

    public void VisualizeTelemetry(GameObject panel)
    {
        panel.SetActive(true);
        var parser = FindFirstObjectByType<LogParser>();
        var vs = FindFirstObjectByType<TelemetryVisualizer>();
        parser.ParseLogFile(ConnectionsModule.logFilePath);
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
        ballisticHandler ??= FindFirstObjectByType<XHandlerInsertValue>();
        ballisticHandler.CallBack();
        xModuleBallistic.LogDocument();
        Debug.LogWarning("Correctly generated XML output, but logic not implemented as no server exist");
        // TODO: work with server and show results
    }

}
