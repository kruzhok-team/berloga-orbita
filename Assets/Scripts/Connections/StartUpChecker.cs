using System;
using System.Collections;
using Connections;
using UnityEngine;
using XML;

public class StartUpChecker : MonoBehaviour
{
    private ConnectionsModule module;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        module = gameObject.AddComponent(typeof(ConnectionsModule)) as ConnectionsModule;
        
        //StartCoroutine(module.GetServerInfo());
        ////StartCoroutine(module.GetDevices());
       //StartCoroutine(module.GetParameters("planets", "Moon"));
       //StartCoroutine(module.GetSample("planets", "Moon"));
       /*
       Debug.Log(xModule.ToString());
       StartCoroutine(module.PostCalculation("planets_gravity", xModule.ToString(), 
           (result) =>
           {
               StartCoroutine(Waiting(result));
           }));
           */
       
       Debug.Log("what");
    }

    // Update is called once per frame
    void Update()
    {
       
        //StartCoroutine(module.GetCalculationResult("planets_gravity", "1734519806765909"));
    }

    private IEnumerator Waiting(string id)
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(module.GetCalculationStatus("planets_gravity", id));
        yield return new WaitForSeconds(1);
        StartCoroutine(module.GetCalculationStatus("planets_gravity", id));
        StartCoroutine(module.GetCalculationResult("planets_gravity", id));
    }
}
