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
    public GameObject startPanel = null;
    public static bool devicesGot = false;
    public static string devicePrefixPath = null;
    
    private static XModule xModule;
    private static XModule xModuleBallistic;
    
    private MissionHandler missionHandler;
    private XGeneration unitCreationHandler = null;
    private IXHandler ballisticHandler  = null;
    private TabSystem tabSystem = null;
    private Validator validator;

    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        devicePrefixPath = "Devices/";
    }
    private void Start()
    {
        startPanel?.SetActive(true);
        missionHandler = GetComponent<MissionHandler>();
        validator = GetComponent<Validator>();
        tabSystem = FindFirstObjectByType<TabSystem>();
        StartCoroutine(FindFirstObjectByType<ContentFiller>().FetchContent());
        RegisterHandlers();
        StartCoroutine(PrepareMission());
        StartCoroutine(GetAllowedDevices());
    }
    

    private IEnumerator PrepareMission()
    {
        while (missionHandler.isReady != 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        xModule = new XModule(missionHandler.GetMissionXmlPath());
        xModuleBallistic = new XModule(missionHandler.GetBallisticCalculatorXmlPath());
        
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
        startPanel?.SetActive(false);
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
        var visualizer = FindFirstObjectByType<TelemetryVisualizer>();
        parser.ParseLogFile(ConnectionsModule.logFilePath);
        parser.ParseShortLogFile(ConnectionsModule.shortLogFilePath);
        
        visualizer.Visualize(parser.telemetryDataList, parser.results);
    }
    
    
    public void ExecuteModel()
    {
        xModule.Reload();
        List<IXHandler> handlers = unitCreationHandler.CollectParts();
        foreach (IXHandler handler in handlers)
        {
            handler.CallBack();
        }
        ValidatorState state = validator.Validate();
        if (state != ValidatorState.Correct)
        {
            switch (state)
            {
                case ValidatorState.ProblemWithCode:
                    missionHandler.DisplayStatus("проблема с кодом");
                    return;
                case ValidatorState.ProblemWithConstruction:
                    missionHandler.DisplayStatus("проблема с аппаратом");
                    return;
            }
            Debug.LogWarning("Were unhandled case of validator, counting as it's not valid" + state);
            return;
        }
        missionHandler.StartCalculation(xModule.ToString(), "planets");
        xModule.LogDocument();
        validator.executions.Add(StartCoroutine(missionHandler.GetCalculationResults("planets")));
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
        
        tabSystem.ActivateTab(TabType.RunAndExecute);
        missionHandler.StartCalculation(xModuleBallistic.ToString(), "planets_gravity");
        StartCoroutine(missionHandler.GetCalculationResults("planets_gravity"));
    }
    
}
