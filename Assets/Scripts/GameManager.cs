using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XML;

public sealed class GameManager : MonoBehaviour
{
    private static XModule xModule;
    private List<IXHandler> handlers = new List<IXHandler>();

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
}
