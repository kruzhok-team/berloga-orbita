using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XML;

public class GameManager : MonoBehaviour
{
    private static XModule xModule;

    private void Start()
    {
        xModule = new XModule(XModule.DefaultFilePath, "test.xml");
    }
    
    public XModule GetXModule()
    {
        return xModule;
    }
}
